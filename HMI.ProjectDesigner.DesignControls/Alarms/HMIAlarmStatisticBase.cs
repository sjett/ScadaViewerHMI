// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using HMI.ProjectDesigner.DesignControls.Alarms.ViewModels;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public abstract class HMIAlarmStatisticBase : CommonControl
	{
		protected const string ElementLayoutRoot = "uxLayoutRoot";
		protected const string ElementRadChart = "RadChartElement";
		private HMIAlarmStatisticBaseViewModel ViewModel
		{
			get
			{
				return base.DataContext as HMIAlarmStatisticBaseViewModel;
			}
		}
		protected RadChart RadChart
		{
			get;
			set;
		}
		protected System.Windows.Controls.Grid LayoutRoot
		{
			get;
			set;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.LayoutRoot = null;
			this.RadChart = null;
			base.OnApplyTemplate();
			this.LayoutRoot = (base.GetTemplateChild("uxLayoutRoot") as System.Windows.Controls.Grid);
			this.RadChart = (base.GetTemplateChild("RadChartElement") as RadChart);
			if (this.ViewModel != null)
			{
				this.UpdateData();
				this.SetChartTheme();
				this.ViewModel.ChartThemeChanged += new System.EventHandler<ValueChangedEventArgs<TelerikTheme>>(this.HMIAlarmStatisticBase_ChartThemeChanged);
			}
		}
		private void HMIAlarmStatisticBase_ChartThemeChanged(object sender, ValueChangedEventArgs<TelerikTheme> e)
		{
			this.RecreateChart();
			this.UpdateData();
			this.SetChartTheme();
		}
		protected virtual void RecreateChart()
		{
			if (this.LayoutRoot == null)
			{
				return;
			}
			if (this.RadChart != null)
			{
				this.LayoutRoot.Children.Remove(this.RadChart);
			}
			this.RadChart = new RadChart
			{
				Name = "RadChartElement"
			};
			this.LayoutRoot.Children.Insert(0, this.RadChart);
		}
		protected abstract void UpdateData();
		protected virtual void SetChartTheme()
		{
			if (this.RadChart == null)
			{
				return;
			}
			switch (this.ViewModel.ChartTheme)
			{
			case TelerikTheme.OfficeBlack:
				StyleManager.SetTheme(this.RadChart, new Office_BlackTheme());
				return;
			case TelerikTheme.OfficeBlue:
				StyleManager.SetTheme(this.RadChart, new Office_BlueTheme());
				return;
			case TelerikTheme.OfficeSilver:
				StyleManager.SetTheme(this.RadChart, new Office_SilverTheme());
				return;
			case TelerikTheme.Summer:
				StyleManager.SetTheme(this.RadChart, new SummerTheme());
				return;
			case TelerikTheme.Vista:
				StyleManager.SetTheme(this.RadChart, new VistaTheme());
				return;
			case TelerikTheme.Windows7:
				StyleManager.SetTheme(this.RadChart, new Windows7Theme());
				return;
			default:
				throw new System.ArgumentOutOfRangeException("TelerikTheme");
			}
		}
	}
}
