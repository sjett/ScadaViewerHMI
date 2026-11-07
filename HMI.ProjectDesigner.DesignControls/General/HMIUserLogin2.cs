// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class HMIUserLogin2 : CommonControl
	{
		static HMIUserLogin2()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIUserLogin2), new System.Windows.FrameworkPropertyMetadata(typeof(HMIUserLogin2)));
		}
	}
}
