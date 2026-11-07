// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISABag1 : HMIISABase
	{
		static HMIISABag1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISABag1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISABag1)));
		}
	}
}
