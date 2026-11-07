// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.InputOutput
{
	public class HMITextBox2 : CommonControl
	{
		static HMITextBox2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITextBox2), new System.Windows.FrameworkPropertyMetadata(typeof(HMITextBox2)));
		}
	}
}
