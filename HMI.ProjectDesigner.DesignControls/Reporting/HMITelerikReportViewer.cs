// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Reporting
{
	public class HMITelerikReportViewer : CommonControl
	{
		static HMITelerikReportViewer()
		{
			DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITelerikReportViewer), new FrameworkPropertyMetadata(typeof(HMITelerikReportViewer)));
		}
	}
}
