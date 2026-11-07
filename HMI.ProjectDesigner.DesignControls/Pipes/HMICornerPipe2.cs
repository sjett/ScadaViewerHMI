// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Pipes
{
	public class HMICornerPipe2 : PipeControl
	{
		static HMICornerPipe2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMICornerPipe2), new System.Windows.FrameworkPropertyMetadata(typeof(HMICornerPipe2)));
		}
	}
}
