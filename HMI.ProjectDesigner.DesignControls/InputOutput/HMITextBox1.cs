// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.InputOutput
{
	public class HMITextBox1 : CommonControl
	{
		static HMITextBox1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITextBox1), new System.Windows.FrameworkPropertyMetadata(typeof(HMITextBox1)));
		}
	}
}
