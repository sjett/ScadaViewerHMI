// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAFlap2 : HMIISABase
	{
		static HMIISAFlap2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAFlap2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAFlap2)));
		}
	}
}
