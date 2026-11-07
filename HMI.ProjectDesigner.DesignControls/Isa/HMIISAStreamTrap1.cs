// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAStreamTrap1 : HMIISABase
	{
		static HMIISAStreamTrap1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAStreamTrap1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAStreamTrap1)));
		}
	}
}
