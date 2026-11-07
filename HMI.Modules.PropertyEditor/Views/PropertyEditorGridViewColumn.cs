// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using HMI.Infrastructure.Models;
namespace HMI.Modules.PropertyEditor.Views
{
	public class PropertyEditorGridViewColumn : GridViewBoundColumnBase
	{
		public static readonly DependencyProperty PropertyIndexProperty = 
			DependencyProperty.Register("PropertyIndex", typeof(int), typeof(PropertyEditorGridViewColumn), null);
		public int PropertyIndex
		{
			get
			{
				return (int)base.GetValue(PropertyIndexProperty);
			}
			set
			{
				base.SetValue(PropertyIndexProperty, value);
			}
		}
		public override FrameworkElement CreateCellElement(GridViewCell cell, object dataItem)
		{
			HorizontalEditorItem horizontalEditorItem = (HorizontalEditorItem)dataItem;
			cell.DataContext = horizontalEditorItem.Properties[PropertyIndex];
			return base.CreateCellElement(cell, horizontalEditorItem.Properties[PropertyIndex]);
		}
	}
}
