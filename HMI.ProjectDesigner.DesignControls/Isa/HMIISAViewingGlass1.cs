// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAViewingGlass1 : HMIISABase
	{
		static HMIISAViewingGlass1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAViewingGlass1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAViewingGlass1)));
		}
	}
}
