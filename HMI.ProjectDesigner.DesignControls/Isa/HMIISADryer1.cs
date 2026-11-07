// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISADryer1 : HMIISABase
	{
		static HMIISADryer1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISADryer1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISADryer1)));
		}
	}
}
