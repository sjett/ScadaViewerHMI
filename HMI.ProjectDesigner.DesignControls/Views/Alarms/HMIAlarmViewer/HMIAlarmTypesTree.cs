// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Views.Alarms.HMIAlarmViewer
{
	public class HMIAlarmTypesTree : CommonControl
	{
		static HMIAlarmTypesTree()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAlarmTypesTree), new System.Windows.FrameworkPropertyMetadata(typeof(HMIAlarmTypesTree)));
		}
	}
}
