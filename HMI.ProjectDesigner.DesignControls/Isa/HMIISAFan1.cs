// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAFan1 : HMIISABase
	{
		static HMIISAFan1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAFan1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAFan1)));
		}
	}
}
