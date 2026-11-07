// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class HMILogFileViewer : CommonControl
	{
		static HMILogFileViewer()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMILogFileViewer), new System.Windows.FrameworkPropertyMetadata(typeof(HMILogFileViewer)));
		}
	}
}
