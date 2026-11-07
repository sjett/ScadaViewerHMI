// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISALine1 : HMIISABase
	{
		static HMIISALine1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISALine1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISALine1)));
		}
	}
}
