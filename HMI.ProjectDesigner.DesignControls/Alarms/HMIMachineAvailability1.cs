// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using System.Windows;
using Telerik.Windows.Controls.Charting;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public class HMIMachineAvailability1 : HMIAlarmStatisticBase
	{
		private System.Collections.ObjectModel.ObservableCollection<MachineAvailability> listMachineAvailability = new System.Collections.ObjectModel.ObservableCollection<MachineAvailability>();
		static HMIMachineAvailability1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIMachineAvailability1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIMachineAvailability1)));
		}
		public HMIMachineAvailability1()
		{
			this.FillMachineAvailabilityList();
		}
		private void CreateBarSeriesInDesignMode()
		{
			base.RadChart.SeriesMappings.Clear();
			HorizontalBarSeriesDefinition horizontalBarSeriesDefinition = new HorizontalBarSeriesDefinition();
            horizontalBarSeriesDefinition.LegendDisplayMode = LegendDisplayMode.DataPointLabel;
			SeriesMapping seriesMapping = new SeriesMapping();
			seriesMapping.ItemMappings.Add(new ItemMapping("State",DataPointMember.XCategory));
			seriesMapping.ItemMappings.Add(new ItemMapping("Duration",DataPointMember.YValue));
			seriesMapping.ItemMappings.Add(new ItemMapping("Duration",DataPointMember.Label));
			seriesMapping.ItemMappings.Add(new ItemMapping("State",DataPointMember.LegendLabel));
			seriesMapping.ItemMappings.Add(new ItemMapping("State",DataPointMember.Tooltip));
			seriesMapping.SeriesDefinition=(horizontalBarSeriesDefinition);
			base.RadChart.DefaultView.ChartArea.AxisY.Title=("时间(秒)");
			base.RadChart.DefaultView.ChartArea.AxisY.DefaultLabelFormat=("F0");
			base.RadChart.DefaultView.ChartArea.AxisY.IsZeroBased=(true);
			base.RadChart.DefaultView.ChartArea.IsNoDataMessageEnabled=(false);
			base.RadChart.ItemsSource=(this.listMachineAvailability);
			base.RadChart.SeriesMappings.Add(seriesMapping);
			base.RadChart.DefaultView.ChartTitle.Content = "设备运行情况";
		}
		protected override void UpdateData()
		{
			if (base.RadChart == null)
			{
				return;
			}
			this.CreateBarSeriesInDesignMode();
		}
		protected override void RecreateChart()
		{
			base.RecreateChart();
			base.RadChart.DefaultView.ChartTitle.Content = "设备运行情况";
		}
		private void FillMachineAvailabilityList()
		{
			System.Random random = new System.Random();
			this.listMachineAvailability.Add(new MachineAvailability(0.0, (long)random.Next(0, 1000), "停机"));
			this.listMachineAvailability.Add(new MachineAvailability(1.0, (long)random.Next(0, 1000), "手动"));
			this.listMachineAvailability.Add(new MachineAvailability(2.0, (long)random.Next(0, 1000), "自动"));
			this.listMachineAvailability.Add(new MachineAvailability(3.0, (long)random.Next(0, 1000), "异常"));
			this.listMachineAvailability.Add(new MachineAvailability(4.0, (long)random.Next(0, 1000), "设置模式"));
			this.listMachineAvailability.Add(new MachineAvailability(5.0, (long)random.Next(0, 1000), "待机"));
		}
	}
}
