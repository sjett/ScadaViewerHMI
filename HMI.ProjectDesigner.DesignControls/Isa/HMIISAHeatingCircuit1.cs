// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAHeatingCircuit1 : HMIISABase
	{
		static HMIISAHeatingCircuit1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAHeatingCircuit1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAHeatingCircuit1)));
		}
	}
}
