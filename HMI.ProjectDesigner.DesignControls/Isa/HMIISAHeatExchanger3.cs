// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAHeatExchanger3 : HMIISABase
	{
		static HMIISAHeatExchanger3()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAHeatExchanger3), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAHeatExchanger3)));
		}
	}
}
