// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Gauges
{
	public class HMIBargraph2 : HMIBargraphBase
	{
		static HMIBargraph2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIBargraph2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIBargraph2)));
		}
	}
}
