// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Indicators
{
	public class HMICompass1 : HMIIndicatorBase
	{
		static HMICompass1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMICompass1), new System.Windows.FrameworkPropertyMetadata(typeof(HMICompass1)));
		}
	}
}
