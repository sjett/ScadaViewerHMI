// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.InputOutput
{
	public class HMINumericBox1 : CommonControl
	{
		static HMINumericBox1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMINumericBox1), new System.Windows.FrameworkPropertyMetadata(typeof(HMINumericBox1)));
		}
	}
}
