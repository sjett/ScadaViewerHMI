// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.InputOutput
{
	public class HMIExtendedNumericDisplay1 : CommonControl
	{
		static HMIExtendedNumericDisplay1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIExtendedNumericDisplay1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIExtendedNumericDisplay1)));
		}
	}
}
