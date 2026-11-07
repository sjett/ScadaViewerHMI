// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Buttons
{
	public class HMIComboBoxButton1 : CommonControl
	{
		static HMIComboBoxButton1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIComboBoxButton1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIComboBoxButton1)));
		}
	}
}
