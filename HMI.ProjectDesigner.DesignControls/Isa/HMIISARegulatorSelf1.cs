// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISARegulatorSelf1 : HMIISABase
	{
		static HMIISARegulatorSelf1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISARegulatorSelf1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISARegulatorSelf1)));
		}
	}
}
