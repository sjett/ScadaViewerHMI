// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class HMILogbook : CommonControl
	{
		static HMILogbook()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMILogbook), new System.Windows.FrameworkPropertyMetadata(typeof(HMILogbook)));
		}
	}
}
