// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Views.DataTable
{
	public class HMIDataTable : CommonControl
	{
		static HMIDataTable()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIDataTable), new System.Windows.FrameworkPropertyMetadata(typeof(HMIDataTable)));
		}
	}
}
