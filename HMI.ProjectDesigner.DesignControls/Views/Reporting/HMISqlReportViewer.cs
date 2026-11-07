// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Views.Reporting
{
	public class HMISqlReportViewer : CommonControl
	{
		static HMISqlReportViewer()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMISqlReportViewer), new System.Windows.FrameworkPropertyMetadata(typeof(HMISqlReportViewer)));
		}
	}
}
