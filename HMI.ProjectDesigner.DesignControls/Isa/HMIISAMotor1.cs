// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAMotor1 : HMIISABase
	{
		static HMIISAMotor1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAMotor1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAMotor1)));
		}
	}
}
