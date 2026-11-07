// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Buttons
{
	public class HMIButton1 : CommonControl
	{
		static HMIButton1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIButton1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIButton1)));
		}
	}
}
