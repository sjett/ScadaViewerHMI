// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISACooler1 : HMIISABase
	{
		static HMIISACooler1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISACooler1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISACooler1)));
		}
	}
}
