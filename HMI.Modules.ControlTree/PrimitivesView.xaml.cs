// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;

namespace HMI.Modules.ControlTree
{
    /// <summary>
    /// PrimitivesView.xaml 的交互逻辑
    /// </summary>
    public partial class PrimitivesView : UserControl, IPrimitivesView
    {
        private Point dragStartPoint;
        public PrimitivesView()
		{
			this.InitializeComponent();
            CreateControls();
		}
		private void PrimitivesView_PreviewMouseMove(object sender, MouseEventArgs e)
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
		private void PrimitivesView_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{
            dragStartPoint = e.GetPosition(null);
		}
		private void CreateControls()
		{
			this.uxEllipseContainer.Content = Activator.CreateInstance("HMI.ProjectDesigner.DesignControls", "HMI.ProjectDesigner.DesignControls.Primitives.HMIEllipse1").Unwrap();
			this.uxRectangleContainer.Content = Activator.CreateInstance("HMI.ProjectDesigner.DesignControls", "HMI.ProjectDesigner.DesignControls.Primitives.HMIRectangle1").Unwrap();
			this.uxTriangleContainer.Content = Activator.CreateInstance("HMI.ProjectDesigner.DesignControls", "HMI.ProjectDesigner.DesignControls.Primitives.HMITriangle1").Unwrap();
			this.uxTrapezeContainer.Content = Activator.CreateInstance("HMI.ProjectDesigner.DesignControls", "HMI.ProjectDesigner.DesignControls.Primitives.HMITrapeze1").Unwrap();
			this.uxAnimatedEllipseContainer.Content = Activator.CreateInstance("HMI.ProjectDesigner.DesignControls", "HMI.ProjectDesigner.DesignControls.Primitives.HMIAnimatedEllipse1").Unwrap();
			this.uxAnimatedRectangleContainer.Content = Activator.CreateInstance("HMI.ProjectDesigner.DesignControls", "HMI.ProjectDesigner.DesignControls.Primitives.HMIAnimatedRectangle1").Unwrap();
			this.uxAnimatedTriangleContainer.Content = Activator.CreateInstance("HMI.ProjectDesigner.DesignControls", "HMI.ProjectDesigner.DesignControls.Primitives.HMIAnimatedTriangle1").Unwrap();
			this.uxAnimatedTrapezeContainer.Content = Activator.CreateInstance("HMI.ProjectDesigner.DesignControls", "HMI.ProjectDesigner.DesignControls.Primitives.HMIAnimatedTrapeze1").Unwrap();
		}
		private void StartDrag(MouseEventArgs e)
		{
			FrameworkElement frameworkElement = e.Source as FrameworkElement;
			if (frameworkElement != null)
			{
				DragDrop.DoDragDrop(frameworkElement, frameworkElement.Tag, DragDropEffects.Copy);
			}
		}
    }
}
