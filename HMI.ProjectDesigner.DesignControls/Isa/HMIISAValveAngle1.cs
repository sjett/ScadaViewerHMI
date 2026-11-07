// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAValveAngle1 : HMIISABase
	{
		static HMIISAValveAngle1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAValveAngle1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAValveAngle1)));
		}
	}
}
