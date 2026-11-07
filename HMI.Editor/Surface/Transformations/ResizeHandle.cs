// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls.Primitives;
namespace HMI.Editor.Surface.Transformations
{
	public class ResizeHandle : Thumb
	{
		public static readonly DependencyProperty HorizontalHandlePositionProperty;
		public static readonly DependencyProperty VerticalHandlePositionProperty;
		public static readonly DependencyProperty ResizeDecoratorProperty;
		public HorizontalAlignment HorizontalHandlePosition
		{
			get
			{
				return (HorizontalAlignment)GetValue(HorizontalHandlePositionProperty);
			}
			set
			{
                SetValue(HorizontalHandlePositionProperty, value);
			}
		}
		public VerticalAlignment VerticalHandlePosition
		{
			get
			{
				return (VerticalAlignment)GetValue(VerticalHandlePositionProperty);
			}
			set
			{
                SetValue(VerticalHandlePositionProperty, value);
			}
		}
		public ResizeDecorator ResizeDecorator
		{
			get
			{
				return (ResizeDecorator)GetValue(ResizeDecoratorProperty);
			}
			set
			{
                SetValue(ResizeDecoratorProperty, value);
			}
		}
		static ResizeHandle()
		{
            HorizontalHandlePositionProperty = DependencyProperty.Register("HorizontalHandlePosition", typeof(HorizontalAlignment), typeof(ResizeHandle), null);
            VerticalHandlePositionProperty = DependencyProperty.Register("VerticalHandlePosition", typeof(VerticalAlignment), typeof(ResizeHandle), null);
            ResizeDecoratorProperty = DependencyProperty.Register("ResizeDecorator", typeof(ResizeDecorator), typeof(ResizeHandle), null);
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ResizeHandle), new FrameworkPropertyMetadata(typeof(ResizeHandle)));
		}
		public ResizeHandle()
		{
            DragStarted += new DragStartedEventHandler(ResizeHandle_DragStarted);
            DragDelta += new DragDeltaEventHandler(ResizeHandle_DragDelta);
            DragCompleted += new DragCompletedEventHandler(ResizeHandle_DragCompleted);
		}
		private void ResizeHandle_DragDelta(object sender, DragDeltaEventArgs e)
		{
			try
			{
                ResizeDecorator.OnMoveResizeHandle(this);
			}
			catch (InvalidOperationException)
			{
			}
			e.Handled = true;
		}
		private void ResizeHandle_DragStarted(object sender, DragStartedEventArgs e)
		{
            ResizeDecorator.OnResizeStarted();
			e.Handled = true;
		}
		private void ResizeHandle_DragCompleted(object sender, DragCompletedEventArgs e)
		{
            ResizeDecorator.OnResizeCompleted(sender, e);
		}
	}
}
