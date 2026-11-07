// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAPump2 : HMIISABase
	{
		static HMIISAPump2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAPump2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAPump2)));
		}
	}
}
