// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Pipes
{
	public class HMICrossLine1 : LineControl
	{
		static HMICrossLine1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMICrossLine1), new System.Windows.FrameworkPropertyMetadata(typeof(HMICrossLine1)));
		}
	}
}
