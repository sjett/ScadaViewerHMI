// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Views;
using HMI.Editor.Common;

namespace HMI.Editor.Surface.Transformations
{
	public class DragDecorator : Thumb
	{
		private const double DragThreshold = 5.0;
		private Point initialCoords;
		private bool isInDragMode;
		private double mouseX;
		private double mouseY;
		private SnapManager snapManager;

		public static readonly DependencyProperty ControlContainerProperty;
		private ICommonControlViewModel ViewModel
		{
			get
			{
				return DataContext as ICommonControlViewModel;
			}
		}
		public ControlContainer ControlContainer
		{
			get
			{
				return (ControlContainer)GetValue(ControlContainerProperty);
			}
			set
			{
				base.SetValue(ControlContainerProperty, value);
			}
		}
		static DragDecorator()
		{
            ControlContainerProperty = DependencyProperty.Register("ControlContainer", typeof(ControlContainer), typeof(DragDecorator), null);
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DragDecorator), new FrameworkPropertyMetadata(typeof(DragDecorator)));
		}
		public DragDecorator()
		{
            Loaded += new RoutedEventHandler(DragDecorator_Loaded);
		}
		private void DragDecorator_Loaded(object sender, RoutedEventArgs e)
		{
            ControlContainer.KeyDown += new KeyEventHandler(ControlContainer_KeyDown);
            DragStarted += new DragStartedEventHandler(DragDecorator_DragStarted);
            DragDelta += new DragDeltaEventHandler(DragDecorator_DragDelta);
            DragCompleted += new DragCompletedEventHandler(DragDecorator_DragCompleted);
            Loaded -= new RoutedEventHandler(DragDecorator_Loaded);
            Unloaded += new RoutedEventHandler(DragDecorator_Unloaded);
		}
		private void DragDecorator_Unloaded(object sender, RoutedEventArgs e)
		{
            ControlContainer.KeyDown -= new KeyEventHandler(ControlContainer_KeyDown);
            DragStarted -= new DragStartedEventHandler(DragDecorator_DragStarted);
            DragDelta -= new DragDeltaEventHandler(DragDecorator_DragDelta);
            DragCompleted -= new DragCompletedEventHandler(DragDecorator_DragCompleted);
            Unloaded -= new RoutedEventHandler(DragDecorator_Unloaded);
            Loaded += new RoutedEventHandler(DragDecorator_Loaded);
		}
		private void ControlContainer_KeyDown(object sender, KeyEventArgs e)
		{
			if (ViewModel.ShowDragDecorator)
			{
                PerformKeyDrag(e);
			}
		}
		private static Point GetKeyDrag(KeyEventArgs e)
		{
			Point result = default;
			double keyDragIncrement = GetKeyDragIncrement();
            Key actualKey = e.Key == Key.System ? e.SystemKey : e.Key;//系统键的特殊性?：某些按键（如 Alt、F10 等）会被 Windows 系统用于触发菜单、切换窗口等全局操作，因此 WPF 将其归类为“系统
            switch (actualKey)
			{
			case Key.Left:
				result = new Point(-keyDragIncrement, 0.0);
				e.Handled = true;
				break;
			case Key.Up:
				result = new Point(0.0, -keyDragIncrement);
				e.Handled = true;
				break;
			case Key.Right:
				result = new Point(keyDragIncrement, 0.0);
				e.Handled = true;
				break;
			case Key.Down:
				result = new Point(0.0, keyDragIncrement);
				e.Handled = true;
				break;
			}
			return result;
		}
		private static double GetKeyDragIncrement()
		{
			double result = 1.0;
			if ((Keyboard.Modifiers & ModifierKeys.Alt) == ModifierKeys.Alt)
			{
				result = 0.01;
			}
			return result;
		}
		private void PerformKeyDrag(KeyEventArgs e)
		{
			Point keyDrag = GetKeyDrag(e);
			if (e.Handled)
			{
				double x = keyDrag.X;
				double y = keyDrag.Y;
                snapManager = new SnapManager(this, ControlContainer);
                Drag(ref x, ref y, SnapMode.Ignore);
                snapManager = null;
			}
		}
		private void DragDecorator_DragStarted(object sender, DragStartedEventArgs e)
		{
            snapManager = new SnapManager(this, ControlContainer);
			Point coordinatesRelativeToParent = GetCoordinatesRelativeToParent();
            mouseX = coordinatesRelativeToParent.X;
            mouseY = coordinatesRelativeToParent.Y;
            initialCoords = new Point(mouseX, mouseY);
            isInDragMode = false;
            ControlContainer.DragStarted(coordinatesRelativeToParent);
            ViewModel.TransformationStarted();
		}
		private void DragDecorator_DragDelta(object sender, DragDeltaEventArgs e)
		{
			Point coords = GetCoordinatesRelativeToParent();
			SnapMode snapMode = Keyboard.Modifiers.HasFlag(ModifierKeys.Control) ? SnapMode.Ignore : SnapMode.Regular;
			if (Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
			{
				snapMode = SnapMode.Ignore;
				coords = GetAxisAlignedCoords(coords);
			}
			double horizontalChange = coords.X - mouseX;
			double verticalChange = coords.Y - mouseY;
			if (!isInDragMode)
			{
				if (Math.Abs(horizontalChange) < 5.0 && Math.Abs(verticalChange) < 5.0)
				{
					return;
				}
                isInDragMode = true;
				e.Handled = true;
			}
            Drag(ref horizontalChange, ref verticalChange, snapMode);
            initialCoords.Offset(-horizontalChange, -verticalChange);
			e.Handled = true;
            ControlContainer.DragDelta();
		}
		private Point GetAxisAlignedCoords(Point coords)
		{
			double angleDegrees = Vector.AngleBetween(new Vector(1.0, 0.0), coords - initialCoords);
			double snappedAngleDegrees = Math.Round(angleDegrees / 90.0) * 90.0;
			double dragLength = (coords - initialCoords).Length;
			double angleRadians = snappedAngleDegrees * Math.PI / 180.0;
			return new Point(initialCoords.X + dragLength * Math.Cos(angleRadians), initialCoords.Y + dragLength * Math.Sin(angleRadians));
		}
		private void DragDecorator_DragCompleted(object sender, DragCompletedEventArgs e)
		{
            snapManager = null;
            isInDragMode = false;
            ControlContainer.DragCompleted();
			//e.Handled = true;//阻击事件冒泡
		}
		private void Drag(ref double horizontalChange, ref double verticalChange, SnapMode snapMode)
		{
			if (snapMode == SnapMode.Regular)
			{
				double minHorizontalSnapDelta = double.MaxValue;
				double minVerticalSnapDelta = double.MaxValue;
                snapManager.PerformAnchorSnapping(0.0, 0.0, ref horizontalChange, ref verticalChange, ref minHorizontalSnapDelta, ref minVerticalSnapDelta, snapMode);
                snapManager.PerformAnchorSnapping(ActualWidth, 0.0, ref horizontalChange, ref verticalChange, ref minHorizontalSnapDelta, ref minVerticalSnapDelta, snapMode);
                snapManager.PerformAnchorSnapping(ActualWidth, ActualHeight, ref horizontalChange, ref verticalChange, ref minHorizontalSnapDelta, ref minVerticalSnapDelta, snapMode);
                snapManager.PerformAnchorSnapping(0.0, ActualHeight, ref horizontalChange, ref verticalChange, ref minHorizontalSnapDelta, ref minVerticalSnapDelta, snapMode);
                snapManager.PerformAnchorSnapping(ActualWidth / 2.0, ActualHeight / 2.0, ref horizontalChange, ref verticalChange, ref minHorizontalSnapDelta, ref minVerticalSnapDelta, snapMode);
			}
            ViewModel.Translate(horizontalChange, verticalChange, TransformMode.Relative);
		}
		private Point GetCoordinatesRelativeToParent()
		{
			return MouseUtils.GetMouseCoordinates(ControlContainer);
		}
	}
}
