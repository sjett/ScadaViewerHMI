// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class HMIUserLogin1 : CommonControl
	{
		static HMIUserLogin1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIUserLogin1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIUserLogin1)));
		}
	}
}
