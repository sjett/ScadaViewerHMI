// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls.Charting;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public abstract class HMIHighCountAlarmStatisticBase : HMIAlarmStatisticBase
	{
		private const string DefaultTitle = "Top {0} alarms";
		private const string BarSeriesItemLabelStyle_OfficeBlackTheme = "BarSeriesItemLabelStyle_OfficeBlackTheme";
		private const string BarSeriesItemLabelStyle_OfficeBlueTheme = "BarSeriesItemLabelStyle_OfficeBlueTheme";
		private const string BarSeriesItemLabelStyle_OfficeSilverTheme = "BarSeriesItemLabelStyle_OfficeSilverTheme";
		private const string BarSeriesItemLabelStyle_SummerTheme = "BarSeriesItemLabelStyle_SummerTheme";
		private const string BarSeriesItemLabelStyle_VistaTheme = "BarSeriesItemLabelStyle_VistaTheme";
		private const string BarSeriesItemLabelStyle_Windows7Theme = "BarSeriesItemLabelStyle_Windows7Theme";
		protected System.Collections.ObjectModel.ObservableCollection<object> listAlarms = new System.Collections.ObjectModel.ObservableCollection<object>();
		public HMIHighCountAlarmStatisticBaseViewModel ViewModel
		{
			get
			{
				return base.DataContext as HMIHighCountAlarmStatisticBaseViewModel;
			}
		}
		public virtual string YTitle
		{
			get;
			private set;
		}
		public virtual double PieLabelOffset
		{
			get;
			private set;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.UpdateMaximumAlarmsProperty();
			if (this.ViewModel == null)
			{
				return;
			}
			this.ViewModel.ChartThemeChanged += new System.EventHandler<ValueChangedEventArgs<TelerikTheme>>(this.HMIHighCountAlarmStatisticBase_ChartThemeChanged);
			this.ViewModel.DisplayTypeChanged += new System.EventHandler<ValueChangedEventArgs<DisplayType>>(this.HMIHighCountAlarmStatisticBase_DisplayTypeChanged);
			this.ViewModel.MaximumAlarmsChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMIHighCountAlarmStatisticBase_MaximumAlarmsChanged);
		}
		private void HMIHighCountAlarmStatisticBase_MaximumAlarmsChanged(object sender, ValueChangedEventArgs<int> e)
		{
			this.UpdateMaximumAlarmsProperty();
		}
		private void HMIHighCountAlarmStatisticBase_DisplayTypeChanged(object sender, ValueChangedEventArgs<DisplayType> e)
		{
			this.UpdateData();
		}
		private void HMIHighCountAlarmStatisticBase_ChartThemeChanged(object sender, ValueChangedEventArgs<TelerikTheme> e)
		{
			this.UpdateMaximumAlarmsProperty();
		}
		protected virtual void SetChartSeriesItemLabelStyle(ISeriesDefinition seriesDefinition)
		{
			if (base.LayoutRoot == null)
			{
				return;
			}
			switch (this.ViewModel.ChartTheme)
			{
			case TelerikTheme.OfficeBlack:
				seriesDefinition.SeriesItemLabelStyle=(base.LayoutRoot.Resources["BarSeriesItemLabelStyle_OfficeBlackTheme"] as System.Windows.Style);
				return;
			case TelerikTheme.OfficeBlue:
				seriesDefinition.SeriesItemLabelStyle=(base.LayoutRoot.Resources["BarSeriesItemLabelStyle_OfficeBlueTheme"] as System.Windows.Style);
				return;
			case TelerikTheme.OfficeSilver:
				seriesDefinition.SeriesItemLabelStyle=(base.LayoutRoot.Resources["BarSeriesItemLabelStyle_OfficeSilverTheme"] as System.Windows.Style);
				return;
			case TelerikTheme.Summer:
				seriesDefinition.SeriesItemLabelStyle=(base.LayoutRoot.Resources["BarSeriesItemLabelStyle_SummerTheme"] as System.Windows.Style);
				return;
			case TelerikTheme.Vista:
				seriesDefinition.SeriesItemLabelStyle=(base.LayoutRoot.Resources["BarSeriesItemLabelStyle_VistaTheme"] as System.Windows.Style);
				return;
			case TelerikTheme.Windows7:
				seriesDefinition.SeriesItemLabelStyle=(base.LayoutRoot.Resources["BarSeriesItemLabelStyle_Windows7Theme"] as System.Windows.Style);
				return;
			default:
				throw new System.ArgumentOutOfRangeException("TelerikTheme");
			}
		}
		protected override void UpdateData()
		{
			if (base.RadChart == null)
			{
				return;
			}
			switch (this.ViewModel.DisplayType)
			{
			case DisplayType.Column:
				this.CreateColumnSeriesInDesignMode();
				return;
			case DisplayType.Pie:
				this.CreatePieSeriesInDesignMode();
				return;
			default:
				throw new System.ArgumentOutOfRangeException("HMIAlarmFrequencyDisplayType");
			}
		}
		protected virtual void CreateColumnSeriesInDesignMode()
		{
			this.FillListAlarms();
			base.RadChart.SeriesMappings.Clear();
			BarSeriesDefinition barSeriesDefinition = this.CreateBarSeriesDefinition();
			SeriesMapping seriesMapping = this.CreateSeriesMapping(barSeriesDefinition);
			this.SetupColumnRadChartArea(seriesMapping);
		}
		protected virtual void SetupColumnRadChartArea(SeriesMapping seriesMapping)
		{
			base.RadChart.DefaultView.ChartArea.AxisX.DefaultLabelFormat=(" ");
			base.RadChart.DefaultView.ChartArea.AxisY.DefaultLabelFormat=("F0");
			base.RadChart.DefaultView.ChartArea.AxisY.IsZeroBased=(true);
			base.RadChart.DefaultView.ChartArea.AxisY.Title=(this.YTitle);
			base.RadChart.DefaultView.ChartArea.IsNoDataMessageEnabled=(false);
			base.RadChart.ItemsSource=(this.listAlarms);
			base.RadChart.SeriesMappings.Add(seriesMapping);
		}
		protected SeriesMapping CreateSeriesMapping(BarSeriesDefinition barSeriesDefinition)
		{
			SeriesMapping seriesMapping = new SeriesMapping();
			seriesMapping.ItemMappings.Add(new ItemMapping("Value",DataPointMember.YValue));
			seriesMapping.ItemMappings.Add(new ItemMapping("Value",DataPointMember.Label));
			seriesMapping.ItemMappings.Add(new ItemMapping("Name",DataPointMember.LegendLabel));
			seriesMapping.ItemMappings.Add(new ItemMapping("Name",DataPointMember.Tooltip));
			seriesMapping.SeriesDefinition=(barSeriesDefinition);
			return seriesMapping;
		}
		protected virtual BarSeriesDefinition CreateBarSeriesDefinition()
		{
			BarSeriesDefinition barSeriesDefinition = new BarSeriesDefinition();
			barSeriesDefinition.ShowItemToolTips=(true);
            barSeriesDefinition.LegendDisplayMode = LegendDisplayMode.DataPointLabel;
			this.SetChartSeriesItemLabelStyle(barSeriesDefinition);
			return barSeriesDefinition;
		}
		protected virtual void FillListAlarms()
		{
			this.listAlarms.Clear();
			this.listAlarms.Add(new AlarmValue("报警 1", 100));
			this.listAlarms.Add(new AlarmValue("报警 2", 80));
			this.listAlarms.Add(new AlarmValue("报警 3", 60));
			this.listAlarms.Add(new AlarmValue("报警 4", 40));
			this.listAlarms.Add(new AlarmValue("报警 5", 20));
		}
		protected virtual void CreatePieSeriesInDesignMode()
		{
			this.FillListAlarms();
			base.RadChart.SeriesMappings.Clear();
			PieSeriesDefinition pieSeriesDefinition = this.CreatePieSeriesDefinition();
			SeriesMapping seriesMapping = this.CreatePieSeriesMapping(pieSeriesDefinition);
			this.SetupPieRadChartArea(seriesMapping);
		}
		protected virtual void SetupPieRadChartArea(SeriesMapping seriesMapping)
		{
			base.RadChart.DefaultView.ChartArea.IsNoDataMessageEnabled=(false);
			base.RadChart.ItemsSource=(this.listAlarms);
			base.RadChart.SeriesMappings.Add(seriesMapping);
		}
		protected SeriesMapping CreatePieSeriesMapping(PieSeriesDefinition pieSeriesDefinition)
		{
			SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.ItemMappings.Add(new ItemMapping("Value", DataPointMember.YValue));
            seriesMapping.ItemMappings.Add(new ItemMapping("Value", DataPointMember.Label));
			seriesMapping.ItemMappings.Add(new ItemMapping("Name",DataPointMember.LegendLabel));
            seriesMapping.ItemMappings.Add(new ItemMapping("Name", DataPointMember.Tooltip));
			seriesMapping.SeriesDefinition=(pieSeriesDefinition);
			return seriesMapping;
		}
		protected virtual PieSeriesDefinition CreatePieSeriesDefinition()
		{
			PieSeriesDefinition pieSeriesDefinition = new PieSeriesDefinition();
			pieSeriesDefinition.ShowItemToolTips=(true);
			if (this.PieLabelOffset != 0.0)
			{
				pieSeriesDefinition.LabelSettings.LabelOffset=(this.PieLabelOffset);
			}
			return pieSeriesDefinition;
		}
		protected virtual void UpdateMaximumAlarmsProperty()
		{
			if (base.RadChart == null)
			{
				return;
			}
			base.RadChart.DefaultView.ChartTitle.Content = string.Format("前 {0} 个报警", (this.ViewModel == null) ? 1 : this.ViewModel.MaximumAlarms);
		}
	}
}
