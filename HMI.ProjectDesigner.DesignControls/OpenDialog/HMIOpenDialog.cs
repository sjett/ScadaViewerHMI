// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.OpenDialog
{
	public class HMIOpenDialog : CommonControl
	{
		static HMIOpenDialog()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIOpenDialog), new System.Windows.FrameworkPropertyMetadata(typeof(HMIOpenDialog)));
		}
	}
}
