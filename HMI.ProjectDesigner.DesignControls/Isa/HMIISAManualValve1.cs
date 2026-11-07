// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAManualValve1 : HMIISABase
	{
		static HMIISAManualValve1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAManualValve1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAManualValve1)));
		}
	}
}
