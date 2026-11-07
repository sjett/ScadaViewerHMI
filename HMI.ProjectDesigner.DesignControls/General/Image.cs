// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.General
{
	public class Image : CommonControl
	{
		static Image()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(Image), new System.Windows.FrameworkPropertyMetadata(typeof(Image)));
		}
	}
}
