// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Views.Alarms.HMIAlarmViewer
{
	public class HMIAlarmGroupsTree : CommonControl
	{
		static HMIAlarmGroupsTree()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAlarmGroupsTree), new System.Windows.FrameworkPropertyMetadata(typeof(HMIAlarmGroupsTree)));
		}
	}
}
