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

using HMI.Editor.Common;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Views;
using HMI.Editor.Resources;
using HMI.Editor.Surface;
using System.Diagnostics;

namespace HMI.Editor
{
    /// <summary>
    /// DesignerDocument.xaml 的交互逻辑
    /// </summary>
    public partial class DesignerDocument : UserControl
    {
        private const int BorderSize = 2;
        private const int ClickDistanceThreshold = 2;
        private bool centerToMouse;
        private ControlBuilder controlBuilder;
        private Point mouseCoordinatesBeforeScaleChange;
        private Point? mouseDownPosition;
        private double oldZoom = 1.0;

        public IProjectPageViewModel Page
        {
            get
            {
                return base.DataContext as IProjectPageViewModel;
            }
        }

        public DesignerDocument()
        {
            InitializeComponent();
            base.Loaded += new RoutedEventHandler(this.DesignerDocument_Loaded);
            base.DataContextChanged += new DependencyPropertyChangedEventHandler(this.DesignerDocument_DataContextChanged);
        }

        private void DesignerDocument_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            IProjectPageViewModel projectPageViewModel = e.NewValue as IProjectPageViewModel;
            if (projectPageViewModel != null)
            {
                this.controlBuilder = (base.Resources["ControlBuilder"] as ControlBuilder);
                this.controlBuilder.ControlFactory = projectPageViewModel.ControlFactory;
                Trace.TraceInformation("Changed data context on {0}", new object[]
				{
					projectPageViewModel.ID
				});
            }
        }
        private void DesignerDocument_Loaded(object sender, RoutedEventArgs e)
        {
            base.Loaded -= new RoutedEventHandler(this.DesignerDocument_Loaded);
            this.uxHorizontalRuler.MouseLeftButtonDown += new MouseButtonEventHandler(this.uxHorizontalRuler_MouseLeftButtonDown);
            this.uxVerticalRuler.MouseLeftButtonDown += new MouseButtonEventHandler(this.uxVerticalRuler_MouseLeftButtonDown);
            base.PreviewMouseDown += new MouseButtonEventHandler(this.DesignerDocument_PreviewMouseDown);
            base.MouseUp += new MouseButtonEventHandler(this.DesignerDocument_MouseUp);
        }
        private bool IsMousePositionSaved()
        {
            return this.mouseDownPosition.HasValue;
        }
        private Vector GetMousePositionDelta(MouseButtonEventArgs e)
        {
            Point position = e.GetPosition(this);
            return position - this.mouseDownPosition.Value;
        }
        private void DeselectControlsOnClick(MouseButtonEventArgs e)
        {
            if (this.GetMousePositionDelta(e).Length < 2.0)
            {
                this.Page.SetSelection(null);
                this.Page.IsSelected=(true);
                this.uxSurface.Focus();
            }
            this.mouseDownPosition = null;
        }
        private void DesignerDocument_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && this.IsMousePositionSaved())
            {
                // 判断事件来源是否是子控件
                if (e.OriginalSource is FrameworkElement fe)
                {
                    // 如果 DataContext 是你的控件 ViewModel，则说明点在子控件上
                    //if (fe.DataContext is IExplorerNodeViewModel)
                    //{
                    //    return; // 不取消选中
                    //}

                    // 或者判断控件类型
                    if (fe.DataContext != this.DataContext)//如果不是 page,说明是画面中的子控件
                    {
                        return;
                    }
                }

                // 点击空白区域 → 执行取消逻辑
                this.DeselectControlsOnClick(e);
            }
        }
        private void SaveMousePosition(MouseButtonEventArgs e)
        {
            this.mouseDownPosition = new Point?(e.GetPosition(this));
        }
        private void DesignerDocument_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.SaveMousePosition(e);
            }
        }
        private void uxVerticalRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DesignerCanvas designerSurface = this.GetDesignerSurface();
            designerSurface.CreateRulerGuide(Orientation.Vertical, e.GetPosition(designerSurface));
        }
        private DesignerCanvas GetDesignerSurface()
        {
            return VisualTreeNavigator.FindItemsPanel<DesignerCanvas>(this.uxSurface);
        }
        private void uxHorizontalRuler_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DesignerCanvas designerSurface = this.GetDesignerSurface();
            designerSurface.CreateRulerGuide(Orientation.Horizontal, e.GetPosition(designerSurface));
        }
        private void uxDesigner_MouseMove(object sender, MouseEventArgs e)
        {
            Canvas relativeTo = sender as Canvas;
            Point position = e.GetPosition(relativeTo);
            this.uxHorizontalRuler.SetChipPosition(position.X);
            this.uxVerticalRuler.SetChipPosition(position.Y);
            this.uxXCoord.Text = string.Format(System.Globalization.CultureInfo.CurrentCulture, DesignerDocumentTexts.CoordinateFormat, new object[]
			{
				position.X
			});
            this.uxYCoord.Text = string.Format(System.Globalization.CultureInfo.CurrentCulture, DesignerDocumentTexts.CoordinateFormat, new object[]
			{
				position.Y
			});
        }
        private void uxResetZoom_Click(object sender, RoutedEventArgs e)
        {
            this.uxScale.Value = 1.0;
        }
        private void uxDesigner_ZoomFactorChanged(object sender, ZoomFactorChangedEventArgs e)
        {
            this.CenterZoom(e.zoomFactor);
        }
        private void uxAutoFit_Click(object sender, RoutedEventArgs e)
        {
            this.ZoomAutoFit();
            base.UpdateLayout();
            this.ZoomAutoFit();
        }
        private void CenterZoom(double newZoomFactor)
        {
            double horizontalCenter = this.uxDesignerScroll.HorizontalOffset + this.uxDesignerScroll.ViewportWidth / 2.0;
            double verticalCenter = this.uxDesignerScroll.VerticalOffset + this.uxDesignerScroll.ViewportHeight / 2.0;
            double horizontalZoomCenter;
            double verticalZoomCenter;
            if (this.centerToMouse)
            {
                double mouseHorizontalOffset = this.mouseCoordinatesBeforeScaleChange.X - horizontalCenter;
                double mouseVerticalOffset = this.mouseCoordinatesBeforeScaleChange.Y - verticalCenter;
                horizontalZoomCenter = this.mouseCoordinatesBeforeScaleChange.X / this.oldZoom * newZoomFactor - mouseHorizontalOffset;
                verticalZoomCenter = this.mouseCoordinatesBeforeScaleChange.Y / this.oldZoom * newZoomFactor - mouseVerticalOffset;
            }
            else
            {
                horizontalZoomCenter = horizontalCenter / this.oldZoom * newZoomFactor;
                verticalZoomCenter = verticalCenter / this.oldZoom * newZoomFactor;
            }
            double horizontalOffset = horizontalZoomCenter - this.uxDesignerScroll.ViewportWidth / 2.0;
            double verticalOffset = verticalZoomCenter - this.uxDesignerScroll.ViewportHeight / 2.0;
            this.uxDesignerScroll.ScrollToHorizontalOffset(horizontalOffset);
            this.uxDesignerScroll.ScrollToVerticalOffset(verticalOffset);
            this.oldZoom = newZoomFactor;
        }
        private void ZoomAutoFit()
        {
            double availableWidth = this.uxDesignerScroll.ViewportWidth - 2.0;
            double availableHeight = this.uxDesignerScroll.ViewportHeight - 2.0;
            double width = this.uxDesignerWithRulers.Width;
            double height = this.uxDesignerWithRulers.Height;
            double widthScale = availableWidth / width;
            double heightScale = availableHeight / height;
            double fitScale = System.Math.Min(widthScale, heightScale);
            this.uxScale.Value *= fitScale;
        }
        private void uxDesigner_Drop(object sender, DragEventArgs e)
        {
            this.uxSurface.Focus();
            FrameworkElement relativeTo = sender as FrameworkElement;
            IDataObject data = e.Data;
            Point position = e.GetPosition(relativeTo);
            ((IProjectPageViewModel)base.DataContext).DropControl(data, position);
        }
        private void uxDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            Canvas canvas = sender as Canvas;
            canvas.Focus();
            this.uxDesignerScroll.ScrollToTop();
            this.uxDesignerScroll.ScrollToLeftEnd();
        }
        private void designer_RequestBringIntoView(object sender, RequestBringIntoViewEventArgs e)
        {
            e.Handled = true;
        }
        
        private void ControlContainer_Loaded(object sender, RoutedEventArgs e)
        {
            
            ControlContainer container = (ControlContainer)sender;
            if (container.ViewModel != null && container.ViewModel.ShowDragDecorator)
            {
                container.Focus();
                ICommonControlViewModel commonControlViewModel = container.DataContext as ICommonControlViewModel;
                if (commonControlViewModel != null)
                {
                    commonControlViewModel.FocusRequested += new EventHandler((o, ee) => { 
                        Keyboard.Focus(container); 
                    });
                    //commonControlViewModel.FocusRequested += (delegate(object s, System.EventArgs ev)
                    //{
                    //    Keyboard.Focus(container);
                    //});
                }
            }
        }
        private void uxSurface_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.Modifiers.HasFlag(ModifierKeys.Control))
            {
                this.mouseCoordinatesBeforeScaleChange = e.GetPosition(this.uxDesignerWithRulers);
                this.centerToMouse = true;
                this.Page.Scale=(System.Math.Max(0.01, this.Page.Scale + (double)e.Delta / 500.0));
                this.centerToMouse = false;
                e.Handled = true;
            }
        }

    }
}
