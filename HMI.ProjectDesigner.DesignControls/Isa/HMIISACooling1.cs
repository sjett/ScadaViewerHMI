// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISACooling1 : HMIISABase
	{
		static HMIISACooling1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISACooling1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISACooling1)));
		}
	}
}
