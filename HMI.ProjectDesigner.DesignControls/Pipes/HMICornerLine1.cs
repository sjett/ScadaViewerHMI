// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Pipes
{
	public class HMICornerLine1 : LineControl
	{
		static HMICornerLine1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMICornerLine1), new System.Windows.FrameworkPropertyMetadata(typeof(HMICornerLine1)));
		}
	}
}
