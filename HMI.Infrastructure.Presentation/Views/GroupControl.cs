// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.Infrastructure.Presentation.Views
{
    /// <summary>
    /// 分组控件
    /// </summary>
    public class GroupControl: CommonControl
    {
        /// <summary>
        /// 静态构造函数
        /// </summary>
        static GroupControl()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GroupControl), new FrameworkPropertyMetadata(typeof(GroupControl)));
        }
    }
}
