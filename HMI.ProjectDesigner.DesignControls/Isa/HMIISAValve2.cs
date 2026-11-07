// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAValve2 : HMIISABase
	{
		static HMIISAValve2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAValve2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAValve2)));
		}
	}
}
