// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Indicators
{
	public class HMIIndicator1 : HMIIndicatorBase
	{
		static HMIIndicator1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIIndicator1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIIndicator1)));
		}
	}
}
