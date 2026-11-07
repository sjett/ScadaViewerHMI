// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAFilter1 : HMIISABase
	{
		static HMIISAFilter1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAFilter1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAFilter1)));
		}
	}
}
