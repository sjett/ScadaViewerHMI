// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Sliders
{
	public class HMISlider1 : CommonControl
	{
		static HMISlider1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMISlider1), new System.Windows.FrameworkPropertyMetadata(typeof(HMISlider1)));
		}
	}
}
