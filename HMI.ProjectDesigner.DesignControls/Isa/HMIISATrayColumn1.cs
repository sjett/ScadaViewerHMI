// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISATrayColumn1 : HMIISABase
	{
		static HMIISATrayColumn1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISATrayColumn1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISATrayColumn1)));
		}
	}
}
