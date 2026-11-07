// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Gauges
{
	public class HMIBargraph3 : HMIBargraphBase
	{
		static HMIBargraph3()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIBargraph3), new System.Windows.FrameworkPropertyMetadata(typeof(HMIBargraph3)));
		}
	}
}
