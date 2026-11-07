// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.InputOutput
{
	public class HMINumericDisplay1 : CommonControl
	{
		static HMINumericDisplay1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMINumericDisplay1), new System.Windows.FrameworkPropertyMetadata(typeof(HMINumericDisplay1)));
		}
	}
}
