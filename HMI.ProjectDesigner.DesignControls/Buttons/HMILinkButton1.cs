// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Buttons
{
	public class HMILinkButton1 : CommonControl
	{
		static HMILinkButton1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMILinkButton1), new System.Windows.FrameworkPropertyMetadata(typeof(HMILinkButton1)));
		}
	}
}
