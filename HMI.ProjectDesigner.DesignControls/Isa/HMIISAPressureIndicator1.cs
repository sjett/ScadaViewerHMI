// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAPressureIndicator1 : HMIISABase
	{
		static HMIISAPressureIndicator1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAPressureIndicator1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAPressureIndicator1)));
		}
	}
}
