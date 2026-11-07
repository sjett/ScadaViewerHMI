// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Telerik.Windows.Controls;

namespace HMI.Modules.ControlTree
{
    /// <summary>
    /// ControlTreeView.xaml 的交互逻辑
    /// </summary>
    public partial class ControlTreeView : UserControl, IControlTreeView
    {
        private Point dragStartPoint;
		
		
		public ControlTreeView(IControlTreeViewModel viewModel)
		{
			if (viewModel == null)
			{
				throw new ArgumentNullException("viewModel", "viewModel is null.");
			}
            DataContext = viewModel;
			this.InitializeComponent();
		}
		public ControlTreeView()
		{
			this.InitializeComponent();
		}
		private void TreeView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            dragStartPoint = e.GetPosition(null);
		}
		private void StartDrag(MouseEventArgs e)
		{
			ControlTreeControlItem? controlTreeControlItem = this.uxControlTree.SelectedItem as ControlTreeControlItem;
			if (controlTreeControlItem != null)
			{
				object controlInstance = controlTreeControlItem.ControlInstance;
				if (controlInstance != null)
				{
					DragDrop.DoDragDrop(this.uxControlTree, controlInstance, DragDropEffects.Copy);
				}
			}
		}
		private void uxControlTree_MouseLeave(object sender, MouseEventArgs e)
		{
			if (e.LeftButton == MouseButtonState.Pressed || e.RightButton == MouseButtonState.Pressed)
			{
				Point position = e.GetPosition(null);
				position.Offset(dragStartPoint.X, dragStartPoint.Y);
				if (Math.Abs(position.X) > SystemParameters.MinimumHorizontalDragDistance || Math.Abs(position.Y) > SystemParameters.MinimumVerticalDragDistance)
				{
                    StartDrag(e);
				}
			}
		}
    }
}
