// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAFlap3 : HMIISABase
	{
		static HMIISAFlap3()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAFlap3), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAFlap3)));
		}
	}
}
