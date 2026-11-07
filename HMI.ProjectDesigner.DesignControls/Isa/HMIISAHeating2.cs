// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAHeating2 : HMIISABase
	{
		static HMIISAHeating2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAHeating2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAHeating2)));
		}
	}
}
