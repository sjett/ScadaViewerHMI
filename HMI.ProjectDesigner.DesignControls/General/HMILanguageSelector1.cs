// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class HMILanguageSelector1 : CommonControl
	{
		static HMILanguageSelector1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMILanguageSelector1), new System.Windows.FrameworkPropertyMetadata(typeof(HMILanguageSelector1)));
		}
	}
}
