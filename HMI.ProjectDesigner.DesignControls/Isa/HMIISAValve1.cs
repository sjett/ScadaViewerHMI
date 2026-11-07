// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAValve1 : HMIISABase
	{
		static HMIISAValve1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAValve1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAValve1)));
		}
	}
}
