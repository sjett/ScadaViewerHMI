// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Pipes
{
	public class HMITPipe3 : PipeControl
	{
		static HMITPipe3()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITPipe3), new System.Windows.FrameworkPropertyMetadata(typeof(HMITPipe3)));
		}
	}
}
