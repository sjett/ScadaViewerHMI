// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Gauges
{
	public class HMIBargraph1 : HMIBargraphBase
	{
		static HMIBargraph1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIBargraph1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIBargraph1)));
		}
	}
}
