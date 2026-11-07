// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using HMI.ProjectDesigner.DesignControls.OperationDiary.ViewModels;
using HMI.Infrastructure.Presentation.Views;
using System.Windows.Controls;

namespace HMI.ProjectDesigner.DesignControls.OperationDiary
{
	public class HMIOperationDiary : CommonControl
	{
		static HMIOperationDiary()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIOperationDiary), new System.Windows.FrameworkPropertyMetadata(typeof(HMIOperationDiary)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.SetDefaultGridColumns();
		}
		private void SetDefaultGridColumns()
		{
			DataGrid? dataGrid = base.GetTemplateChild("uxEvents") as DataGrid;
			if (dataGrid != null)
			{
				HMIOperationDiaryViewModel? wFOperationDiaryViewModel = base.DataContext as HMIOperationDiaryViewModel;
				ToolkitDataGridColumnsSetter toolkitDataGridColumnsSetter = new ToolkitDataGridColumnsSetter(dataGrid);
				if (wFOperationDiaryViewModel != null)
				{
					toolkitDataGridColumnsSetter.SetColumns(wFOperationDiaryViewModel.Columns);
				}
			}
		}
	}
}
