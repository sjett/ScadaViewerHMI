// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.Infrastructure.Presentation.Views
{
    /// <summary>
    /// 选择控件，遮罩层
    /// </summary>
    public class SelectionControl : CommonControl
	{
		static SelectionControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(SelectionControl), 
				new FrameworkPropertyMetadata(typeof(SelectionControl)));
		}
	}
}
