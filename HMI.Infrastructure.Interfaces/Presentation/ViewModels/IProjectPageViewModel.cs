// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Factories;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 项目页面视图模型接口
    /// </summary>
    public interface IProjectPageViewModel : IExplorerNodeViewModel, IGroupContainer, IPage, ISelectionContainer
    {
        /// <summary>
        /// 布局子项集合
        /// </summary>
        IObservableCollectionGroup LayoutChildren
        {
            get;
        }
        /// <summary>
        /// ID
        /// </summary>
        string ID
        {
            get;
            set;
        }
        /// <summary>
        /// 选中项
        /// </summary>
        IExplorerNodeViewModel SelectedItem
        {
            get;
        }
        /// <summary>
        /// 控件工厂
        /// </summary>
        IControlFactory ControlFactory
        {
            get;
        }
        /// <summary>
        /// 背景色
        /// </summary>
        Brush Background
        {
            get;
            set;
        }
        /// <summary>
        /// 信号前缀
        /// </summary>
        string SignalPrefix
        {
            get;
            set;
        }
        /// <summary>
        /// 是否使用页面名称作为信号前缀
        /// </summary>
        bool UsePageNameAsSignalPrefix
        {
            get;
            set;
        }
        /// <summary>
        /// 页面宽度（未缩放）
        /// </summary>
        double Width
        {
            get;
            set;
        }
        /// <summary>
        /// 页面高度（未缩放）
        /// </summary>
        double Height
        {
            get;
            set;
        }
        /// <summary>
        /// 缩放比例
        /// </summary>
        double Scale
        {
            get;
            set;
        }
        /// <summary>
        /// 反向缩放比例
        /// </summary>
        double InverseScale
        {
            get;
        }
        /// <summary>
        /// 缩放后的页面宽度
        /// </summary>
        double ScaledWidth
        {
            get;
        }
        /// <summary>
        /// 缩放后的页面高度
        /// </summary>
        double ScaledHeight
        {
            get;
        }
        /// <summary>
        /// 包含标尺后的缩放页面宽度
        /// </summary>
        double ScaledWidthInclRulers
        {
            get;
        }
        /// <summary>
        /// 包含标尺后的缩放页面高度
        /// </summary>
        double ScaledHeightInclRulers
        {
            get;
        }
        /// <summary>
        /// 页面尺寸选项集合
        /// </summary>
        ObservableCollection<string> PageSizes
        {
            get;
        }
        /// <summary>
        /// 页面尺寸选项
        /// </summary>
        string Size
        {
            get;
            set;
        }
        /// <summary>
        /// 网格尺寸选项集合
        /// </summary>
        ObservableCollection<double> GridSizes
        {
            get;
        }
        /// <summary>
        /// 网格尺寸
        /// </summary>
        double GridSize
        {
            get;
            set;
        }
        /// <summary>
        /// 标尺尺寸
        /// </summary>
        double RulerSize
        {
            get;
            set;
        }
        /// <summary>
        /// 参考线设置视图模型
        /// </summary>
        IGuideSettingsViewModel GuideSettings
        {
            get;
        }
        /// <summary>
        /// 是否拉伸页面以适应窗口大小
        /// </summary>
        bool StretchPages
        {
            get;
            set;
        }
        /// <summary>
        /// 设置页面尺寸
        /// </summary>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        void SetSize(double newWidth, double newHeight);
        /// <summary>
        /// 锁定或解锁页面尺寸
        /// </summary>
        /// <param name="isLocked"></param>
        void LockSize(bool isLocked);
        /// <summary>
        /// 拖放控件
        /// </summary>
        /// <param name="dragDropData">拖放对象</param>
        /// <param name="dropPosition">拖放点</param>
        void DropControl(IDataObject dragDropData, Point dropPosition);
        /// <summary>
        /// 获取选中项集合
        /// </summary>
        /// <returns></returns>
        ICollection<ICommonControlViewModel> GetSelectedItems();
        /// <summary>
        /// 添加顶层子项
        /// </summary>
        /// <param name="items">控件</param>
        /// <param name="offsetIfOverlapping">如果重叠则偏移子项</param>
        void AddTopmostChildren(ICollection<ICommonControlViewModel> items, bool offsetIfOverlapping);
    }
}
