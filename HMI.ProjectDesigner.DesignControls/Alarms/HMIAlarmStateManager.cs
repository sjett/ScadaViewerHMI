// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.Views;
using HMI.ProjectDesigner.DesignControls.Alarms.AlarmStateManager;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using System.Windows.Controls;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public class HMIAlarmStateManager : CommonControl
	{
		private DataGrid uxAlarmsDataGrid;
		private static readonly System.DateTime HMIAlarmMinDateTime;
		private HMIAlarmStateManagerViewModel ViewModel
		{
			get
			{
				return base.DataContext as HMIAlarmStateManagerViewModel;
			}
		}
		static HMIAlarmStateManager()
		{
			HMIAlarmStateManager.HMIAlarmMinDateTime = new System.DateTime(1900, 1, 1, 0, 0, 0);
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAlarmStateManager), new System.Windows.FrameworkPropertyMetadata(typeof(HMIAlarmStateManager)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.uxAlarmsDataGrid = (base.GetTemplateChild("uxAlarmsDataGrid") as DataGrid);
			this.SubsribeToEvents();
		}
		private void SubsribeToEvents()
		{
			this.uxAlarmsDataGrid.LoadingRow += new System.EventHandler<DataGridRowEventArgs>(HMIAlarmStateManager.uxAlarmsDataGrid_LoadingRow);
			if (this.ViewModel != null)
			{
				this.ViewModel.ColumnVisibilityChanged += new System.EventHandler<ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, System.Windows.Visibility>>>(this.ViewModel_ColumnVisibilityChanged);
				this.ViewModel.ColumnWidthChanged += new System.EventHandler<ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, DataGridLength>>>(this.ViewModel_ColumnWidthChanged);
				this.ViewModel.ColumnOrderChanged += new System.EventHandler<ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, int>>>(this.ViewModel_ColumnOrderChanged);
				this.ViewModel.InitializePropertyStates();
			}
		}
		private static void uxAlarmsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
		{
			HMIAlarm wFAlarm = e.Row.DataContext as HMIAlarm;
			if (wFAlarm != null)
			{
				if (wFAlarm.Status == 0)
				{
					if (wFAlarm.ReactivationDate <= HMIAlarmStateManager.HMIAlarmMinDateTime)
					{
						e.Row.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Red);
						return;
					}
					e.Row.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Yellow);
					return;
				}
				else
				{
					if (wFAlarm.Status == 1)
					{
						e.Row.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Green);
					}
				}
			}
		}
		private void ViewModel_ColumnOrderChanged(object sender, ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, int>> e)
		{
			if (this.uxAlarmsDataGrid == null)
			{
				return;
			}
			System.Collections.Generic.Dictionary<string, int> newValue = e.NewValue;
			foreach (DataGridColumn current in this.uxAlarmsDataGrid.Columns)
			{
				int num = newValue[current.Header.ToString()] - 1;
				current.SetValue(DataGridColumn.DisplayIndexProperty, num);
			}
		}
		private void ViewModel_ColumnWidthChanged(object sender, ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, DataGridLength>> e)
		{
			if (this.uxAlarmsDataGrid == null)
			{
				return;
			}
			System.Collections.Generic.Dictionary<string, DataGridLength> newValue = e.NewValue;
			foreach (DataGridColumn current in this.uxAlarmsDataGrid.Columns)
			{
				current.SetValue(DataGridColumn.WidthProperty, newValue[current.Header.ToString()]);
			}
		}
		private void ViewModel_ColumnVisibilityChanged(object sender, ValueChangedEventArgs<System.Collections.Generic.Dictionary<string, System.Windows.Visibility>> e)
		{
			if (this.uxAlarmsDataGrid == null)
			{
				return;
			}
			System.Collections.Generic.Dictionary<string, System.Windows.Visibility> newValue = e.NewValue;
			foreach (DataGridColumn current in this.uxAlarmsDataGrid.Columns)
			{
				current.SetValue(DataGridColumn.VisibilityProperty, newValue[current.Header.ToString()]);
			}
		}
	}
}
