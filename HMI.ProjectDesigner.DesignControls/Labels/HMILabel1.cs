// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Labels
{
	public class HMILabel1 : HMIStatesControlBase
	{
		static HMILabel1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMILabel1), new System.Windows.FrameworkPropertyMetadata(typeof(HMILabel1)));
		}
	}
}
