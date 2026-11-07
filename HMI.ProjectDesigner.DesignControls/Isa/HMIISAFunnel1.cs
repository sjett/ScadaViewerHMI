// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAFunnel1 : HMIISABase
	{
		static HMIISAFunnel1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAFunnel1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAFunnel1)));
		}
	}
}
