// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Navigation
{
	public class HMINavigationParameter : CommonControl
	{
		static HMINavigationParameter()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMINavigationParameter), new System.Windows.FrameworkPropertyMetadata(typeof(HMINavigationParameter)));
		}
	}
}
