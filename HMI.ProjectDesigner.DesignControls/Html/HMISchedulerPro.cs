// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Html
{
	public class HMISchedulerPro : CommonControl
	{
		static HMISchedulerPro()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMISchedulerPro), new System.Windows.FrameworkPropertyMetadata(typeof(HMISchedulerPro)));
		}
	}
}
