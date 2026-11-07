// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAPressure2 : HMIISABase
	{
		static HMIISAPressure2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAPressure2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAPressure2)));
		}
	}
}
