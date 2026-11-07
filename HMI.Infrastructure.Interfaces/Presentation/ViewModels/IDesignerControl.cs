// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 设计器控件接口
    /// </summary>
    public interface IDesignerControl
    {
        /// <summary>
        /// 加载完成事件
        /// </summary>
        event RoutedEventHandler Loaded;

        /// <summary>
        /// 设计器期望尺寸
        /// </summary>
        Size? DesignerDesiredSize
        {
            get;
        }
        /// <summary>
        /// 设计器模板Key
        /// </summary>
        string DesignerTemplateKey
        {
            get;
        }
        /// <summary>
        /// 显示名称
        /// </summary>
        string DisplayName
        {
            get;
        }
        /// <summary>
        /// 宽度
        /// </summary>
        double Width
        {
            get;
            set;
        }
        /// <summary>
        /// 高度
        /// </summary>
        double Height
        {
            get;
            set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        string Name
        {
            get;
            set;
        }
        /// <summary>
        /// 设置资源引用
        /// </summary>
        /// <param name="dp">依赖属性</param>
        /// <param name="name">名称</param>
        void SetResourceReference(DependencyProperty dp, object name);
    }
}
