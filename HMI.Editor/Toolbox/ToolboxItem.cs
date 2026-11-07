// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Editor.Toolbox
{
	public class ToolboxItem : ContentControl
	{
		private Point? dragStartPoint = null;
		static ToolboxItem()
		{
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ToolboxItem), new FrameworkPropertyMetadata(typeof(ToolboxItem)));
		}
		protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
		{
			base.OnPreviewMouseDown(e);
            dragStartPoint = new Point?(e.GetPosition(this));
		}
		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (e.LeftButton != MouseButtonState.Pressed)
			{
                dragStartPoint = null;
			}
			if (dragStartPoint.HasValue)
			{
				IDesignerControl? designerControl = Content as IDesignerControl;
				if (designerControl != null)
				{
					DragDrop.DoDragDrop(this, designerControl, DragDropEffects.Copy);
				}
				e.Handled = true;
			}
		}
	}
}
