// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Pipes
{
	public class HMITPipe1 : PipeControl
	{
		static HMITPipe1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITPipe1), new System.Windows.FrameworkPropertyMetadata(typeof(HMITPipe1)));
		}
	}
}
