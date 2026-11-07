// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Views.InputOutput
{
	public class HMIStateDisplay1 : CommonControl
	{
		static HMIStateDisplay1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIStateDisplay1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIStateDisplay1)));
		}
	}
}
