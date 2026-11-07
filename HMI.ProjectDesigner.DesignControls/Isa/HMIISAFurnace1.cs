// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAFurnace1 : HMIISABase
	{
		static HMIISAFurnace1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAFurnace1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAFurnace1)));
		}
	}
}
