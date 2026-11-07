// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
namespace HMI.Editor.Surface.Transformations
{
	public class RotateHandle : Thumb
	{
		public static readonly DependencyProperty RotateDecoratorProperty;
		public RotateDecorator RotateDecorator
		{
			get
			{
				return (RotateDecorator)GetValue(RotateDecoratorProperty);
			}
			set
			{
                SetValue(RotateDecoratorProperty, value);
			}
		}
		static RotateHandle()
		{
            RotateDecoratorProperty = DependencyProperty.Register("RotateDecorator", typeof(RotateDecorator), typeof(RotateHandle), null);
            DefaultStyleKeyProperty.OverrideMetadata(typeof(RotateHandle), new FrameworkPropertyMetadata(typeof(RotateHandle)));
		}
		public RotateHandle()
		{
            Loaded += new RoutedEventHandler(RotateDecorator_Loaded);
            DragStarted += new DragStartedEventHandler(RotateDecorator_DragStarted);
            DragDelta += new DragDeltaEventHandler(RotateDecorator_DragDelta);
            DragCompleted += new DragCompletedEventHandler(RotateHandle_DragCompleted);
		}
		private void RotateHandle_DragCompleted(object sender, DragCompletedEventArgs e)
		{
            RotateDecorator.OnRotateCompleted();
		}
		private void RotateDecorator_DragDelta(object sender, DragDeltaEventArgs e)
		{
            RotateDecorator.OnRotateDelta(e.HorizontalChange, e.VerticalChange);
			e.Handled = true;
		}
		private void RotateDecorator_DragStarted(object sender, DragStartedEventArgs e)
		{
            RotateDecorator.OnRotateStarted(Canvas.GetLeft(this), Canvas.GetTop(this));
			e.Handled = true;
		}
		private void RotateDecorator_Loaded(object sender, RoutedEventArgs e)
		{
		}
	}
}
