// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAPump1 : HMIISABase
	{
		static HMIISAPump1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAPump1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAPump1)));
		}
	}
}
