// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Buttons
{
	public class HMIToggleButton1 : CommonControl
	{
		static HMIToggleButton1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIToggleButton1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIToggleButton1)));
		}
	}
}
