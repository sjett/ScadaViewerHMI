// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Gauges
{
	public class HMIBargraph4 : HMIBargraphBase
	{
		static HMIBargraph4()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIBargraph4), new System.Windows.FrameworkPropertyMetadata(typeof(HMIBargraph4)));
		}
	}
}
