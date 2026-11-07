// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAFilter2 : HMIISABase
	{
		static HMIISAFilter2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAFilter2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAFilter2)));
		}
	}
}
