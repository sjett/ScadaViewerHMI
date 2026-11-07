// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Pipes
{
	public class HMIULine1 : LineControl
	{
		static HMIULine1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIULine1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIULine1)));
		}
	}
}
