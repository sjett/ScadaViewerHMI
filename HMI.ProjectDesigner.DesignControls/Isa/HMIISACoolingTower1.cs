// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISACoolingTower1 : HMIISABase
	{
		static HMIISACoolingTower1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISACoolingTower1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISACoolingTower1)));
		}
	}
}
