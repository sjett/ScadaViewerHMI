// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAFlap1 : HMIISABase
	{
		static HMIISAFlap1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAFlap1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAFlap1)));
		}
	}
}
