// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAHeatExchanger2 : HMIISABase
	{
		static HMIISAHeatExchanger2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAHeatExchanger2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAHeatExchanger2)));
		}
	}
}
