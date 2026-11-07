// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Buttons
{
	public class HMIStatesButton1 : HMIStatesButtonBase
	{
		static HMIStatesButton1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIStatesButton1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIStatesButton1)));
		}
		protected override void UpdateIconPlacement(ButtonIconPlacement buttonIconPlacement)
		{
			System.Windows.Controls.Image image = base.GetTemplateChild("uxButtonImage") as System.Windows.Controls.Image;
			if (image == null)
			{
				return;
			}
			System.Windows.Controls.Grid.SetRow(image, base.GetIconGridRow(buttonIconPlacement));
			System.Windows.Controls.Grid.SetColumn(image, base.GetIconGridColumn(buttonIconPlacement));
		}
	}
}
