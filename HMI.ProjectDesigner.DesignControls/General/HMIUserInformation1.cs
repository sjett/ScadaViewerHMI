// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class HMIUserInformation1 : CommonControl
	{
		static HMIUserInformation1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIUserInformation1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIUserInformation1)));
		}
	}
}
