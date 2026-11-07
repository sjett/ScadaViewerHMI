// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Pipes
{
	public class HMICornerPipe1 : PipeControl
	{
		static HMICornerPipe1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMICornerPipe1), new System.Windows.FrameworkPropertyMetadata(typeof(HMICornerPipe1)));
		}
	}
}
