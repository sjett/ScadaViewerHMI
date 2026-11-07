// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Views;

namespace HMI.Editor.Surface.Transformations
{
	public class RotateDecorator : Control
	{
		private double handleX;
		private double handleY;
		private bool isRotatePerformed;
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
		static RotateDecorator()
		{
            ControlContainerProperty = DependencyProperty.Register("ControlContainer", typeof(ControlContainer), typeof(RotateDecorator), null);
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RotateDecorator), new FrameworkPropertyMetadata(typeof(RotateDecorator)));
		}
		public void OnRotateStarted(double x, double y)
		{
            handleX = x - ActualWidth / 2.0;
            handleY = y - ActualHeight / 2.0;
            ViewModel.ActionMonitor.StartGrouping();
            ViewModel.TransformationStarted();
            isRotatePerformed = false;
		}
		public void OnRotateDelta(double horizontalChange, double verticalChange)
		{
			Vector vector = new Vector(handleX + horizontalChange, handleY + verticalChange);
			Vector vector2 = new Vector(handleX, handleY);
			double num = Vector.AngleBetween(vector2, vector);
            ViewModel.Rotate(num, TransformMode.Relative, Keyboard.Modifiers.HasFlag(ModifierKeys.Shift));
            isRotatePerformed = true;
		}
		public void OnRotateCompleted()
		{
			if (isRotatePerformed)
			{
                ViewModel.ActionMonitor.EndGrouping();
				return;
			}
            ViewModel.ActionMonitor.CancelAction();
		}
	}
}
