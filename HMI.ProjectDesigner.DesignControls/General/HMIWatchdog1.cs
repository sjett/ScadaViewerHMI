// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class HMIWatchdog1 : CommonControl
	{
		static HMIWatchdog1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIWatchdog1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIWatchdog1)));
		}
	}
}
