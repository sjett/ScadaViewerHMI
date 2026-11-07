// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.InputOutput
{
	public class HMINumericBox2 : CommonControl
	{
		static HMINumericBox2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMINumericBox2), new System.Windows.FrameworkPropertyMetadata(typeof(HMINumericBox2)));
		}
	}
}
