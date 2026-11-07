// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using Telerik.Windows.Controls;
using HMI.Common.Instrumentation.Events;

namespace HMI.Editor.Common
{
	public class ZoomSelector : Control
	{
		private ButtonBase uxDecreaseZoom;
		private ButtonBase uxIncreaseZoom;
		private RadSlider uxZoomSlider;
		public static readonly DependencyProperty TickValuesProperty = DependencyProperty.Register("TickValues", typeof(ZoomTickValue[]), typeof(ZoomSelector), new PropertyMetadata(new ZoomTickValue[0], new PropertyChangedCallback(OnTickValuesChanged)));
		public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(double), typeof(ZoomSelector), null);
		public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(double), typeof(ZoomSelector), null);
		public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(double), typeof(ZoomSelector), new PropertyMetadata(new PropertyChangedCallback(OnValueChanged)));
		public static readonly DependencyProperty TickValueProperty = DependencyProperty.Register("TickValue", typeof(double), typeof(ZoomSelector), new PropertyMetadata(new PropertyChangedCallback(OnTickChanged)));
		public event EventHandler<DoubleValueChangedEventArgs>? ValueChanged;
		public ZoomTickValue[] TickValues
		{
			get
			{
				return (ZoomTickValue[])GetValue(TickValuesProperty);
			}
			set
			{
                SetValue(TickValuesProperty, value);
			}
		}
		public double MinValue
		{
			get
			{
				return (double)GetValue(MinValueProperty);
			}
			set
			{
                SetValue(MinValueProperty, value);
			}
		}
		public double MaxValue
		{
			get
			{
				return (double)GetValue(MaxValueProperty);
			}
			set
			{
                SetValue(MaxValueProperty, value);
			}
		}
		public double Value
		{
			get
			{
				return (double)GetValue(ValueProperty);
			}
			set
			{
                SetValue(ValueProperty, value);
			}
		}
		public double TickValue
		{
			get
			{
				return (double)GetValue(TickValueProperty);
			}
			set
			{
                SetValue(TickValueProperty, value);
			}
		}
		private static void OnTickValuesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ZoomSelector zoomSelector = (ZoomSelector)d;
			double valueFromTick = zoomSelector.GetValueFromTick(zoomSelector.TickValue);
			zoomSelector.Value = valueFromTick;
		}
		private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ZoomSelector zoomSelector = (ZoomSelector)d;
			double num = (double)e.NewValue;
			double tickFromValue = zoomSelector.GetTickFromValue(num);
			zoomSelector.TickValue = tickFromValue;
			zoomSelector.FireValueChanged(num);
		}
		private static void OnTickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			ZoomSelector zoomSelector = (ZoomSelector)d;
			double tick = (double)e.NewValue;
			double valueFromTick = zoomSelector.GetValueFromTick(tick);
			zoomSelector.Value = valueFromTick;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
            uxZoomSlider = (RadSlider)GetTemplateChild("uxZoomSlider");
            uxDecreaseZoom = (ButtonBase)GetTemplateChild("uxDecreaseZoom");
			if (uxDecreaseZoom != null)
			{
                uxDecreaseZoom.Click += new RoutedEventHandler(uxDecreaseZoom_Click);
			}
            uxIncreaseZoom = (ButtonBase)GetTemplateChild("uxIncreaseZoom");
			if (uxIncreaseZoom != null)
			{
                uxIncreaseZoom.Click += new RoutedEventHandler(uxIncreaseZoom_Click);
			}
		}
		private void uxIncreaseZoom_Click(object sender, RoutedEventArgs e)
		{
            uxZoomSlider.Value += uxZoomSlider.SmallChange;
		}
		private void uxDecreaseZoom_Click(object sender, RoutedEventArgs e)
		{
            uxZoomSlider.Value -= uxZoomSlider.SmallChange;
		}
		public double GetTickFromValue(double value)
		{
			ZoomTickValue leftTickFromValue = GetLeftTickFromValue(value);
			ZoomTickValue rightTickFromValue = GetRightTickFromValue(value);
			if (rightTickFromValue.Value == leftTickFromValue.Value)
			{
				return leftTickFromValue.Tick;
			}
			double num = (value - leftTickFromValue.Value) / (rightTickFromValue.Value - leftTickFromValue.Value);
			return leftTickFromValue.Tick + (rightTickFromValue.Tick - leftTickFromValue.Tick) * num;
		}
		public double GetValueFromTick(double tick)
		{
			ZoomTickValue leftValueFromTick = GetLeftValueFromTick(tick);
			ZoomTickValue rightValueFromTick = GetRightValueFromTick(tick);
			if (rightValueFromTick.Tick == leftValueFromTick.Tick)
			{
				return leftValueFromTick.Value;
			}
			double num = (tick - leftValueFromTick.Tick) / (rightValueFromTick.Tick - leftValueFromTick.Tick);
			return leftValueFromTick.Value + (rightValueFromTick.Value - leftValueFromTick.Value) * num;
		}
		private IEnumerable<ZoomTickValue> GetTickValues()
		{
			return TickValues;
		}
		private ZoomTickValue GetRightTickFromValue(double selectorValue)
		{
			return (
				from t in GetTickValues()
				where t.Value >= selectorValue
				orderby t.Tick
				select t).FirstOrDefault() ?? new ZoomTickValue(1.0, MaxValue);
		}
		private ZoomTickValue GetLeftTickFromValue(double selectorValue)
		{
			return (
				from t in GetTickValues()
				where t.Value <= selectorValue
				orderby t.Tick descending
				select t).FirstOrDefault() ?? new ZoomTickValue(0.0, MinValue);
		}
		private ZoomTickValue GetRightValueFromTick(double tick)
		{
			return (
				from t in GetTickValues()
				where t.Tick >= tick
				orderby t.Tick
				select t).FirstOrDefault() ?? new ZoomTickValue(1.0, MaxValue);
		}
		private ZoomTickValue GetLeftValueFromTick(double tick)
		{
			return (
				from t in GetTickValues()
				where t.Tick <= tick
				orderby t.Tick descending
				select t).FirstOrDefault() ?? new ZoomTickValue(0.0, MinValue);
		}
		public void FireValueChanged(double selectorValue)
		{
            ValueChanged?.Raise(this, new DoubleValueChangedEventArgs
			{
				Value = selectorValue
			});
		}
	}
}
