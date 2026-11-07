// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Html
{
	public class HMIScenarioManagerPro : CommonControl
	{
		static HMIScenarioManagerPro()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIScenarioManagerPro), new System.Windows.FrameworkPropertyMetadata(typeof(HMIScenarioManagerPro)));
		}
	}
}
