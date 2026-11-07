// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Pipes
{
	public class HMILinePipe1 : PipeControl
	{
		static HMILinePipe1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMILinePipe1), new System.Windows.FrameworkPropertyMetadata(typeof(HMILinePipe1)));
		}
	}
}
