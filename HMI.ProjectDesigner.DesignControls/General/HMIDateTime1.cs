// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class HMIDateTime1 : CommonControl
	{
		static HMIDateTime1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIDateTime1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIDateTime1)));
		}
	}
}
