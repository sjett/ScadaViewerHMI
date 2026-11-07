// SPDX-License-Identifier: GPL-3.0-only
using System;

using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Isa
{
	
	public class HMIISAPipe1 : HMIISABase
	{
		static HMIISAPipe1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIISAPipe1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIISAPipe1)));
		}
	}
}
