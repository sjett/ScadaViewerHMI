// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Views;
using HMI.Editor.Common;

namespace HMI.Editor.Surface.Transformations
{
	public class ResizeDecorator : Control
	{
		private bool isResizePerformed;
		private double mouseX;
		private double mouseY;
		private Point origin;
		private double slope;
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
		static ResizeDecorator()
		{
            ControlContainerProperty = DependencyProperty.Register("ControlContainer", typeof(ControlContainer), typeof(ResizeDecorator), null);
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeDecorator), new FrameworkPropertyMetadata(typeof(ResizeDecorator)));
		}
		private Point GetCoordinatesRelativeToParent()
		{
			FrameworkElement parent = GetParent();
			return MouseUtils.GetMouseCoordinates(parent);
		}
		private FrameworkElement GetParent()
		{
			Visual reference = (Visual)VisualTreeHelper.GetParent(this);
			return (FrameworkElement)VisualTreeHelper.GetParent(reference);
		}
		public void OnResizeStarted()
		{
            ViewModel.ActionMonitor.StartGrouping();
            ViewModel.TransformationStarted();
            isResizePerformed = false;
			Point coordinatesRelativeToParent = GetCoordinatesRelativeToParent();
            mouseX = coordinatesRelativeToParent.X;
            mouseY = coordinatesRelativeToParent.Y;
			FrameworkElement parent = GetParent();
            origin = new Point(parent.ActualWidth / 2.0, parent.ActualHeight / 2.0);
            slope = (coordinatesRelativeToParent.Y - origin.Y) / (coordinatesRelativeToParent.X - origin.X);
            snapManager = new SnapManager(this, ControlContainer);
		}
		public void OnMoveResizeHandle(ResizeHandle resizeHandle)
		{
			Point coords = GetCoordinatesRelativeToParent();
			SnapMode snapMode = (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control ? SnapMode.Ignore : SnapMode.Regular;
			if (ViewModel.ForceProportionalResize || Keyboard.Modifiers.HasFlag(ModifierKeys.Shift))
			{
				coords = GetAspectRatioCoords(coords);
				snapMode = SnapMode.Ignore;
			}
			double dragX = coords.X - mouseX;
			double dragY = coords.Y - mouseY;
            Resize(resizeHandle, dragX, dragY, snapMode);
            isResizePerformed = true;
		}
		private Point GetAspectRatioCoords(Point coords)
		{
			if (!double.IsNaN(slope) && !double.IsInfinity(slope))
			{
				Vector vector = new Vector(coords.X, slope * (coords.X - origin.X) + origin.Y);
				Vector vector2 = new Vector(origin.X + (coords.Y - origin.Y) / slope, coords.Y);
				if (vector.LengthSquared > vector2.LengthSquared)
				{
					coords = (Point)vector;
				}
				else
				{
					coords = (Point)vector2;
				}
			}
			else
			{
				coords = new Point(0.0, coords.Y);
			}
			return coords;
		}
		private void Resize(ResizeHandle resizeHandle, double dragX, double dragY, SnapMode snapMode)
		{
			HorizontalAlignment horizontalHandlePosition = resizeHandle.HorizontalHandlePosition;
			VerticalAlignment verticalHandlePosition = resizeHandle.VerticalHandlePosition;
			if (snapMode == SnapMode.Regular)
			{
				double minHorizontalSnapDelta = double.MaxValue;
				double minVerticalSnapDelta = double.MaxValue;
				double controlRelativeSnapAnchorX = 0.0;
				double controlRelativeSnapAnchorY = 0.0;
				if (horizontalHandlePosition == HorizontalAlignment.Left)
				{
					controlRelativeSnapAnchorX = 0.0;
				}
				if (horizontalHandlePosition == HorizontalAlignment.Right)
				{
					controlRelativeSnapAnchorX = ActualWidth;
				}
				if (verticalHandlePosition == VerticalAlignment.Top)
				{
					controlRelativeSnapAnchorY = 0.0;
				}
				if (verticalHandlePosition == VerticalAlignment.Bottom)
				{
					controlRelativeSnapAnchorY = ActualHeight;
				}
                snapManager.PerformAnchorSnapping(controlRelativeSnapAnchorX, controlRelativeSnapAnchorY, ref dragX, ref dragY, ref minHorizontalSnapDelta, ref minVerticalSnapDelta, snapMode);
			}
            ViewModel.Resize(resizeHandle.HorizontalHandlePosition, resizeHandle.VerticalHandlePosition, dragX, dragY);
			double horizontalMouseOffset = 0.0;
			double verticalMouseOffset = 0.0;
			if (horizontalHandlePosition == HorizontalAlignment.Left)
			{
				horizontalMouseOffset = dragX / 2.0;
			}
			if (horizontalHandlePosition == HorizontalAlignment.Right)
			{
				horizontalMouseOffset = dragX / 2.0;
			}
			if (verticalHandlePosition == VerticalAlignment.Top)
			{
				verticalMouseOffset = dragY / 2.0;
			}
			if (verticalHandlePosition == VerticalAlignment.Bottom)
			{
				verticalMouseOffset = dragY / 2.0;
			}
            mouseX += horizontalMouseOffset;
            mouseY += verticalMouseOffset;
		}
		public void OnResizeCompleted(object sender, DragCompletedEventArgs e)
		{
            snapManager = null;
			if (ViewModel == null)
			{
				return;
			}
			if (isResizePerformed)
			{
                ViewModel.ActionMonitor.EndGrouping();
				return;
			}
            ViewModel.ActionMonitor.CancelAction();
		}
	}
}
