// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class HMIConnectorStatistics : CommonControl
	{
		static HMIConnectorStatistics()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIConnectorStatistics), new System.Windows.FrameworkPropertyMetadata(typeof(HMIConnectorStatistics)));
		}
	}
}
