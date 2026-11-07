// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using HMI.Runtime.Core;

namespace HMI.RuntimeControls.Gauges
{
    /// <summary>

    /// </summary>
    public class HMIBargraph4 : HMIBargraphBase
	{
		private Shape? emptyFill;
		private double thresholdValue2Old;
		private double thresholdValue2New;
		private double thresholdValue1New;
		private double thresholdValue1Old;

		public static readonly DependencyProperty ThresholdValue1Property = DependencyProperty.Register("ThresholdValue1", typeof(double), typeof(HMIBargraph4), new PropertyMetadata(33.0, new PropertyChangedCallback(HMIBargraph4.OnThresholdValue1PropertyChanged)));
		public static readonly DependencyProperty ThresholdValue2Property = DependencyProperty.Register("ThresholdValue2", typeof(double), typeof(HMIBargraph4), new PropertyMetadata(66.0, new PropertyChangedCallback(HMIBargraph4.OnThresholdValue2PropertyChanged)));
		
		[Category("HMI")]
		public double ThresholdValue1
		{
			get
			{
				return (double)base.GetValue(HMIBargraph4.ThresholdValue1Property);
			}
			set
			{
				base.SetValue(HMIBargraph4.ThresholdValue1Property, value);
				this.FillBarLinearGradient();
			}
		}
		[Category("HMI")]
		public double ThresholdValue2
		{
			get
			{
				return (double)base.GetValue(HMIBargraph4.ThresholdValue2Property);
			}
			set
			{
				base.SetValue(HMIBargraph4.ThresholdValue2Property, value);
				this.FillBarLinearGradient();
			}
		}
		protected override void UpdateRangeMax(double oldValue, double newValue)
		{
			if (base.IsTemplateApplied)
			{
				if (oldValue == newValue)
				{
					return;
				}
				if (this.ThresholdValue2 >= newValue)
				{
					base.RangeMax = oldValue;
					return;
				}
			}
			base.UpdateRangeMax(oldValue, newValue);
			this.FillBarLinearGradient();
		}
		protected override void UpdateRangeMin(double oldValue, double newValue)
		{
			if (base.IsTemplateApplied)
			{
				if (oldValue == newValue)
				{
					return;
				}
				if (this.ThresholdValue1 <= newValue)
				{
					base.RangeMin = oldValue;
					return;
				}
			}
			base.UpdateRangeMin(oldValue, newValue);
			this.FillBarLinearGradient();
		}
		private static void OnThresholdValue1PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			double newValue = (double)change.NewValue;
			double oldValue = (double)change.OldValue;
			HMIBargraph4? bargraph = dependencyObject as HMIBargraph4;
			if (bargraph != null && newValue != oldValue)
			{
				bargraph.UpdateThresholdValue1Range(oldValue, newValue);
			}
		}
		private void UpdateThresholdValue1Range(double oldValue, double newValue)
		{
			if (base.IsTemplateApplied && (newValue <= base.RangeMin || newValue >= this.ThresholdValue2))
			{
				this.ThresholdValue1 = oldValue;
			}
			this.thresholdValue1Old = oldValue;
			this.thresholdValue1New = newValue;
		}
		private static void OnThresholdValue2PropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			double newValue = (double)change.NewValue;
			double oldValue = (double)change.OldValue;
			HMIBargraph4? bargraph = dependencyObject as HMIBargraph4;
			if (bargraph != null && newValue != oldValue)
			{
				bargraph.UpdateThresholdValue2Range(oldValue, newValue);
			}
		}
		private void UpdateThresholdValue2Range(double oldValue, double newValue)
		{
			if (base.IsTemplateApplied && (newValue <= this.ThresholdValue1 || newValue >= base.RangeMax))
			{
				this.ThresholdValue2 = oldValue;
			}
			this.thresholdValue2Old = oldValue;
			this.thresholdValue2New = newValue;
		}
		public HMIBargraph4()
		{
			base.DefaultStyleKey=(typeof(HMIBargraph4));
		}
		public override void OnApplyTemplate()
		{
			this.emptyFill = null;
			this.emptyFill = (base.GetTemplateChild("EmptyFill") as Shape);
			base.OnApplyTemplate();

			base.Dispatcher.BeginInvoke(new Action(this.FillBar));

			this.UpdateThresholdValue1Range(this.thresholdValue1Old, this.thresholdValue1New);
			this.UpdateThresholdValue2Range(this.thresholdValue2Old, this.thresholdValue2New);
			this.FillBarLinearGradient();
		}
		private void FillBarLinearGradient()
		{
			if (base.BarElement != null)
			{
				try
				{
					double threshold1Offset = (this.ThresholdValue1 - base.RangeMin) / (base.RangeMax - base.RangeMin);
					double threshold2Offset = (this.ThresholdValue2 - base.RangeMin) / (base.RangeMax - base.RangeMin);

					LinearGradientBrush? linearGradientBrush = base.BarElement.Fill as LinearGradientBrush;
					linearGradientBrush!.GradientStops[2].Offset=(threshold1Offset);
					linearGradientBrush.GradientStops[1].Offset=(threshold1Offset - 0.005);

					linearGradientBrush.GradientStops[4].Offset=(threshold2Offset);
					linearGradientBrush.GradientStops[3].Offset=(threshold2Offset - 0.005);
				}
				catch (Exception ex)
				{
					base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		protected override void FillBar()
		{
			if (base.BarElement != null && this.emptyFill != null)
			{
				try
				{
					double actualHeight = base.BarElement.ActualHeight;
					this.emptyFill.Height=(actualHeight - base.SignalValueProcentage * actualHeight);
				}
				catch (Exception ex)
				{
					base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
	}
}
