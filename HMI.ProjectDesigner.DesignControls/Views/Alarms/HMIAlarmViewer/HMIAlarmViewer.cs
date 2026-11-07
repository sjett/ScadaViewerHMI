// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls.GridView;
using HMI.ProjectDesigner.DesignControls.Alarms;
using HMI.ProjectDesigner.DesignControls.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.Views;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Views.Alarms.HMIAlarmViewer
{
	[System.Windows.TemplatePart(Name = "uxAlarmGrid", Type = typeof(DataGrid))]
	public class HMIAlarmViewer : CommonControl
	{
		private DataGrid alarmGrid;
		public HMIAlarmViewerViewModel ViewModel
		{
			get
			{
				return base.DataContext as HMIAlarmViewerViewModel;
			}
		}
		static HMIAlarmViewer()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAlarmViewer), new System.Windows.FrameworkPropertyMetadata(typeof(HMIAlarmViewer)));
		}
		public HMIAlarmViewer()
		{
			base.DataContextChanged += new System.Windows.DependencyPropertyChangedEventHandler(this.HMIAlarmViewer_DataContextChanged);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.alarmGrid = (base.Template.FindName("uxAlarmGrid", this) as DataGrid);
			this.AttachRowHandlers();
			this.UpdateGridColumns();
		}
		private void AttachRowHandlers()
		{
			if (this.alarmGrid != null && this.ViewModel != null)
			{
				this.alarmGrid.LoadingRow += new System.EventHandler<DataGridRowEventArgs>(this.alarmGrid_LoadingRow);
				this.alarmGrid.UnloadingRow += new System.EventHandler<DataGridRowEventArgs>(this.alarmGrid_UnloadingRow);
				return;
			}
		}
		private void alarmGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			this.SetAlarmRowColors(e.Row);
			this.SetPriorityColumnColor(e.Row);
			this.SetHttpLinkColor(e.Row);
		}
		private void alarmGrid_UnloadingRow(object sender, DataGridRowEventArgs e)
		{
			this.ResetPriorityColorCellBackground(e.Row);
		}
		private int FindColumnIndex(AlarmColumnTypes columnType)
		{
			if (this.alarmGrid == null)
			{
				return -1;
			}
			for (int i = 0; i < this.alarmGrid.Columns.Count; i++)
			{
				if (this.alarmGrid.Columns[i].Header.ToString() == columnType.ToString())
				{
					return i;
				}
			}
			return -1;
		}
		private void SetHttpLinkColor(DataGridRow row)
		{
			int num = this.FindColumnIndex(AlarmColumnTypes.HttpLink);
			if (num == -1)
			{
				return;
			}
			HyperlinkButton hyperlinkButton = this.alarmGrid.Columns[num].GetCellContent(row) as HyperlinkButton;
			if (hyperlinkButton == null)
			{
				return;
			}
			hyperlinkButton.Foreground = row.Foreground;
		}
		private void SetAlarmRowColors(DataGridRow row)
		{
			AlarmDetails alarmDetails = row.DataContext as AlarmDetails;
			if (alarmDetails == null)
			{
				return;
			}
			if (alarmDetails.HasBadQuality)
			{
				row.Background = this.ViewModel.BadQualityAlarmBackground;
				row.Foreground = this.ViewModel.BadQualityAlarmForeground;
				return;
			}
			int num = 0;
			if (this.ViewModel.RowColorDefinitions != null && this.ViewModel.RowColorDefinitions.Count != 0)
			{
				row.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.White);
				row.Foreground = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
				foreach (RowColor current in this.ViewModel.RowColorDefinitions)
				{
					if (!current.Priority.HasValue || !(current.Priority == alarmDetails.Priority))
					{
						string alarmType = current.AlarmType;
						if (num >= 3 || !(alarmType == alarmDetails.AlarmTypeSymbolicTextName))
						{
							string alarmGroup = current.AlarmGroup;
							if (num >= 2 || !(alarmGroup == alarmDetails.AlarmGroupSymbolicTextName))
							{
								if (num < 1 && string.IsNullOrEmpty(alarmGroup) && string.IsNullOrEmpty(alarmType) && current.Priority.HasValue)
								{
									this.ApplyRowColorDefinitionToAlarmRow(current, row, alarmDetails);
									num = 1;
									continue;
								}
								continue;
							}
							else
							{
								this.ApplyRowColorDefinitionToAlarmRow(current, row, alarmDetails);
								num = 2;
							}
						}
						else
						{
							this.ApplyRowColorDefinitionToAlarmRow(current, row, alarmDetails);
							num = 3;
						}
					}
					else
					{
						this.ApplyRowColorDefinitionToAlarmRow(current, row, alarmDetails);
						num = 4;
					}
					break;
				}
			}
			if (num == 0)
			{
				switch (alarmDetails.AlarmType)
				{
				case AlarmState.Acknowledged:
					row.Background = this.ViewModel.AcknowledgedAlarmBackground;
					row.Foreground = this.ViewModel.AcknowledgedAlarmForeground;
					return;
				case AlarmState.Inactive:
					row.Background = this.ViewModel.InactiveAlarmBackground;
					row.Foreground = this.ViewModel.InactiveAlarmForeground;
					return;
				case AlarmState.AcknowledgedAndGone:
					row.Background = this.ViewModel.AcknowledgedAndGoneAlarmBackground;
					row.Foreground = this.ViewModel.AcknowledgedAndGoneAlarmForeground;
					return;
				default:
					row.Background = this.ViewModel.ActiveAlarmBackground;
					row.Foreground = this.ViewModel.ActiveAlarmForeground;
					break;
				}
			}
		}
		private void ApplyRowColorDefinitionToAlarmRow(RowColor rowColor, DataGridRow row, AlarmDetails alarm)
		{
			switch (alarm.AlarmType)
			{
			case AlarmState.Acknowledged:
				row.Background = rowColor.AcknowledgedAlarmBackground;
				row.Foreground = rowColor.AcknowledgedAlarmForeground;
				return;
			case AlarmState.Inactive:
				row.Background = rowColor.InactiveAlarmBackground;
				row.Foreground = rowColor.InactiveAlarmForeground;
				return;
			case AlarmState.AcknowledgedAndGone:
				row.Background = rowColor.AcknowledgedAndGoneAlarmBackground;
				row.Foreground = rowColor.AcknowledgedAndGoneAlarmForeground;
				return;
			default:
				row.Background = rowColor.ActiveAlarmBackground;
				row.Foreground = rowColor.ActiveAlarmForeground;
				return;
			}
		}
		private void SetPriorityColumnColor(DataGridRow row)
		{
			AlarmDetails alarmDetails = row.DataContext as AlarmDetails;
			if (alarmDetails == null)
			{
				return;
			}
			for (int i = 0; i < this.ViewModel.PriorityColorDefinitions.Count; i++)
			{
				if (this.ViewModel.PriorityColorDefinitions[i].Priority == alarmDetails.Priority)
				{
					this.ApplyPriorityColorDefinitionToAlarmRow(this.ViewModel.PriorityColorDefinitions[i], row, alarmDetails);
					return;
				}
			}
		}
		private void ApplyPriorityColorDefinitionToAlarmRow(PriorityColor priorityColor, DataGridRow row, AlarmDetails alarm)
		{
			int num = this.FindColumnIndex(AlarmColumnTypes.PriorityColor);
			if (num == -1)
			{
				return;
			}
			System.Windows.Controls.Grid grid = this.alarmGrid.Columns[num].GetCellContent(row) as System.Windows.Controls.Grid;
			if (grid == null)
			{
				return;
			}
			switch (alarm.AlarmType)
			{
			case AlarmState.Active:
				grid.Background = priorityColor.ActiveAlarmColor;
				return;
			case AlarmState.Acknowledged:
				grid.Background = priorityColor.AcknowledgedAlarmColor;
				return;
			case AlarmState.Inactive:
				grid.Background = priorityColor.InactiveAlarmColor;
				return;
			case AlarmState.AcknowledgedAndGone:
				grid.Background = priorityColor.AcknowledgedAndGoneAlarmColor;
				return;
			default:
				grid.Background = null;
				return;
			}
		}
		private void ResetPriorityColorCellBackground(DataGridRow row)
		{
			int num = this.FindColumnIndex(AlarmColumnTypes.PriorityColor);
			if (num == -1)
			{
				return;
			}
			System.Windows.Controls.Grid grid = this.alarmGrid.Columns[num].GetCellContent(row) as System.Windows.Controls.Grid;
			if (grid != null)
			{
				grid.Background = null;
			}
		}
		private void HMIAlarmViewer_DataContextChanged(object sender, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			HMIAlarmViewerViewModel wFAlarmViewerViewModel = e.OldValue as HMIAlarmViewerViewModel;
			HMIAlarmViewerViewModel wFAlarmViewerViewModel2 = e.NewValue as HMIAlarmViewerViewModel;
			if (wFAlarmViewerViewModel != null)
			{
				wFAlarmViewerViewModel.Columns.CollectionChanged -= new System.Collections.Specialized.NotifyCollectionChangedEventHandler(this.Columns_CollectionChanged);
			}
			if (wFAlarmViewerViewModel2 != null)
			{
				wFAlarmViewerViewModel2.Columns.CollectionChanged += new System.Collections.Specialized.NotifyCollectionChangedEventHandler(this.Columns_CollectionChanged);
			}
		}
		private void Columns_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
		{
			this.UpdateGridColumns();
		}
		private void UpdateGridColumns()
		{
			if (this.alarmGrid != null && this.ViewModel != null)
			{
				DataGridColumnsSetter dataGridColumnsSetter = new DataGridColumnsSetter(this.alarmGrid);
				dataGridColumnsSetter.SetColumns(this.ViewModel.Columns);
				return;
			}
		}
	}
}
