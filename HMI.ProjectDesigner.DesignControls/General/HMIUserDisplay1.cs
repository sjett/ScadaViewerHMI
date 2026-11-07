// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class HMIUserDisplay1 : CommonControl
	{
		static HMIUserDisplay1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIUserDisplay1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIUserDisplay1)));
		}
	}
}
