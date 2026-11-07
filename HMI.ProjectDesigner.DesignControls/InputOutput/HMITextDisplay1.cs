// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.InputOutput
{
	public class HMITextDisplay1 : CommonControl
	{
		static HMITextDisplay1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITextDisplay1), new System.Windows.FrameworkPropertyMetadata(typeof(HMITextDisplay1)));
		}
	}
}
