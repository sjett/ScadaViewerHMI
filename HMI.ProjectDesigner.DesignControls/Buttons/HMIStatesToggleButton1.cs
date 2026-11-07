// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using HMI.ProjectDesigner.DesignControls.Buttons.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Buttons
{
	public class HMIStatesToggleButton1 : HMIStatesButtonBase
	{
		static HMIStatesToggleButton1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIStatesToggleButton1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIStatesToggleButton1)));
		}
		protected override void UpdateIconPlacement(ButtonIconPlacement buttonIconPlacement)
		{
			System.Windows.Controls.Image image = base.GetTemplateChild("uxButtonImageChecked") as System.Windows.Controls.Image;
			System.Windows.Controls.Image image2 = base.GetTemplateChild("uxButtonImageUnchecked") as System.Windows.Controls.Image;
			if (image != null)
			{
				System.Windows.Controls.Grid.SetRow(image, base.GetIconGridRow(buttonIconPlacement));
				System.Windows.Controls.Grid.SetColumn(image, base.GetIconGridColumn(buttonIconPlacement));
			}
			if (image2 == null)
			{
				return;
			}
			System.Windows.Controls.Grid.SetRow(image2, base.GetIconGridRow(buttonIconPlacement));
			System.Windows.Controls.Grid.SetColumn(image2, base.GetIconGridColumn(buttonIconPlacement));
		}
	}
}
