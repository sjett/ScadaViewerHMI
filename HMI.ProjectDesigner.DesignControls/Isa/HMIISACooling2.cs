// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISACooling2 : HMIISABase
	{
		static HMIISACooling2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISACooling2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISACooling2)));
		}
	}
}
