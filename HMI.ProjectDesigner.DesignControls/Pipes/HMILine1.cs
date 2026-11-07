// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Pipes
{
	public class HMILine1 : LineControl
	{
		static HMILine1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMILine1), new System.Windows.FrameworkPropertyMetadata(typeof(HMILine1)));
		}
	}
}
