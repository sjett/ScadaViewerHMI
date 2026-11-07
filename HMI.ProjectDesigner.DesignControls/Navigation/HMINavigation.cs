// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Navigation
{
	public class HMINavigation : CommonControl
	{
		static HMINavigation()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMINavigation), new System.Windows.FrameworkPropertyMetadata(typeof(HMINavigation)));
		}
	}
}
