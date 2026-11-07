// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAHeatExchanger1 : HMIISABase
	{
		static HMIISAHeatExchanger1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAHeatExchanger1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAHeatExchanger1)));
		}
	}
}
