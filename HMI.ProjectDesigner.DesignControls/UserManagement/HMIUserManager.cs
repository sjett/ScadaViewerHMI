// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.UserManagement
{
	public class HMIUserManager : CommonControl
	{
		static HMIUserManager()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIUserManager), new System.Windows.FrameworkPropertyMetadata(typeof(HMIUserManager)));
		}
	}
}
