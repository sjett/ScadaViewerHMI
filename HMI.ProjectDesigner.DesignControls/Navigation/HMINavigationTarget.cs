// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Navigation
{
	public class HMINavigationTarget : CommonControl
	{
		static HMINavigationTarget()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMINavigationTarget), new System.Windows.FrameworkPropertyMetadata(typeof(HMINavigationTarget)));
		}
	}
}
