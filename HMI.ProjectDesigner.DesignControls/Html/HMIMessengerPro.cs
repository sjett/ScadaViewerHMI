// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Html
{
	public class HMIMessengerPro : CommonControl
	{
		static HMIMessengerPro()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIMessengerPro), new System.Windows.FrameworkPropertyMetadata(typeof(HMIMessengerPro)));
		}
	}
}
