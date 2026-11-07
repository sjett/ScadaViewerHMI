// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Pipes
{
	public class HMITLine1 : CommonControl
	{
		static HMITLine1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITLine1), new System.Windows.FrameworkPropertyMetadata(typeof(HMITLine1)));
		}
	}
}
