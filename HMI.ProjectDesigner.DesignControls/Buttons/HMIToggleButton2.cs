// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Buttons
{
	public class HMIToggleButton2 : CommonControl
	{
		static HMIToggleButton2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIToggleButton2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIToggleButton2)));
		}
	}
}
