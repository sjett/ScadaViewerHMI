// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public class HMIAlarmOnline : CommonControl
	{
		private DataGrid uxAlarmGrid;
		private static readonly System.DateTime HMIAlarmMinDateTime;
		private HMIAlarmOnlineViewModel ViewModel
		{
			get
			{
				return base.DataContext as HMIAlarmOnlineViewModel;
			}
		}
		static HMIAlarmOnline()
		{
			HMIAlarmOnline.HMIAlarmMinDateTime = new System.DateTime(1900, 1, 1, 0, 0, 0);
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAlarmOnline), new System.Windows.FrameworkPropertyMetadata(typeof(HMIAlarmOnline)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.uxAlarmGrid = (base.GetTemplateChild("uxAlarmGrid") as DataGrid);
			if (this.uxAlarmGrid != null)
			{
				this.SubscribeToEvents();
			}
		}
		private void SubscribeToEvents()
		{
			this.uxAlarmGrid.LoadingRow += new System.EventHandler<DataGridRowEventArgs>(this.uxAlarmGrid_LoadingRow);
			if (this.ViewModel != null)
			{
				this.ViewModel.ColumnWidthChanged += new System.EventHandler<ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, DataGridLength>>>(this.ViewModel_ColumnWidthChanged);
				this.ViewModel.ColumnOrderChanged += new System.EventHandler<ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, int>>>(this.ViewModel_ColumnOrderChanged);
				this.ViewModel.ColorChanged += new System.EventHandler<ValueChangedEventArgs<object>>(this.ViewModel_ColorChanged);
				this.ViewModel.InitializePropertyStates();
			}
		}
		private void ViewModel_ColorChanged(object sender, ValueChangedEventArgs<object> e)
		{
			this.UpdateRowColors();
		}
		private void uxAlarmGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			Alarm alarm = e.Row.DataContext as Alarm;
			if (alarm != null)
			{
				this.SetAlarmRowColors(e.Row, alarm);
			}
		}
		private void ViewModel_ColumnOrderChanged(object sender, ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, int>> e)
		{
			if (this.uxAlarmGrid == null)
			{
				return;
			}
			System.Collections.Generic.Dictionary<string, int> newValue = e.NewValue;
			foreach (DataGridColumn current in this.uxAlarmGrid.Columns)
			{
				int num = newValue[current.Header.ToString()] - 1;
				current.SetValue(DataGridColumn.DisplayIndexProperty, num);
			}
		}
		private void ViewModel_ColumnWidthChanged(object sender, ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, DataGridLength>> e)
		{
			if (this.uxAlarmGrid == null)
			{
				return;
			}
			System.Collections.Generic.Dictionary<string, DataGridLength> newValue = e.NewValue;
			foreach (DataGridColumn current in this.uxAlarmGrid.Columns)
			{
				current.SetValue(DataGridColumn.WidthProperty, newValue[current.Header.ToString()]);
			}
		}
		private void SetAlarmRowColors(DataGridRow row, Alarm alarm)
		{
			switch (alarm.AlarmType)
			{
			case AlarmState.Acknowledged:
				row.Background = this.ViewModel.AcknowledgedAlarmBackground;
				row.Foreground = this.ViewModel.AcknowledgedAlarmForeground;
				return;
			case AlarmState.Inactive:
				row.Background = this.ViewModel.InactiveAlarmBackground;
				row.Foreground = this.ViewModel.InactiveAlarmForeground;
				return;
			default:
				row.Background = this.ViewModel.ActiveAlarmBackground;
				row.Foreground = this.ViewModel.ActiveAlarmForeground;
				return;
			}
		}
		private void UpdateRowColors()
		{
			if (this.uxAlarmGrid == null)
			{
				return;
			}
			foreach (object current in (System.Collections.IEnumerable)this.uxAlarmGrid.Items)
			{
				DataGridRow dataGridRow = this.uxAlarmGrid.ItemContainerGenerator.ContainerFromItem(current) as DataGridRow;
				if (dataGridRow != null)
				{
					Alarm alarm = dataGridRow.DataContext as Alarm;
					this.SetAlarmRowColors(dataGridRow, alarm);
				}
			}
		}
	}
}
