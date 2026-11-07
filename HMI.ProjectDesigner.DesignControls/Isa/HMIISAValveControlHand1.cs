// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAValveControlHand1 : HMIISABase
	{
		static HMIISAValveControlHand1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAValveControlHand1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAValveControlHand1)));
		}
	}
}
