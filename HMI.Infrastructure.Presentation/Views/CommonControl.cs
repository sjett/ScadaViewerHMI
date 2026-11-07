// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HMI.Infrastructure.Presentation.Views
{
    /// <summary>
    /// 通用控件基类
    /// </summary>
    public class CommonControl : Control, IDesignerControl
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static CommonControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CommonControl), new FrameworkPropertyMetadata(typeof(CommonControl)));
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public CommonControl()
        {
            FocusVisualStyle = null;
        }

        public static readonly DependencyProperty DesignerTemplateKeyProperty =
            DependencyProperty.Register(
                nameof(DesignerTemplateKey),
                typeof(string),
                typeof(CommonControl),
                new PropertyMetadata(string.Empty));

        /// <summary>
        /// 设计器模板Key
        /// </summary>
        public string DesignerTemplateKey
        {
            get => (string)GetValue(DesignerTemplateKeyProperty);
            set => SetValue(DesignerTemplateKeyProperty, value);
        }

        /// <summary>
        /// 设计器期望尺寸
        /// </summary>

        [Browsable(false)]
        public Size? DesignerDesiredSize { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName
        {
            get
            {
                return GetType().Name;
            }
        }
     
    }
}
