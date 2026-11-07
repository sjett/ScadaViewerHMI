// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISARegulatorSelf2 : HMIISABase
	{
		static HMIISARegulatorSelf2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISARegulatorSelf2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISARegulatorSelf2)));
		}
	}
}
