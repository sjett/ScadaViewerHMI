// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Gauge;
using HMI.ProjectDesigner.DesignControls.InputOutput.ViewModels;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Common;
using System.Windows.Media;
namespace HMI.ProjectDesigner.DesignControls.InputOutput
{
	public class HMINumericDisplay2 : HMIStatesControlBase
	{
		private const string ElementNumericScale = "uxNumericScaleElement";
		private const string ElementNumericIndicator = "uxNumericIndicatorElement";
		private NumericScale numericScale;
		private NumericIndicator numericIndicator;
		public static readonly System.Windows.DependencyProperty DigitsProperty;
		public static readonly System.Windows.DependencyProperty DisplayTypeProperty;
		public static readonly System.Windows.DependencyProperty PrecisionProperty;
		public event System.EventHandler<ValueChangedEventArgs<int>> DigitsChanged;
		public event System.EventHandler<ValueChangedEventArgs<HMINumericDisplay2DisplayType>> DisplayTypeChanged;
		public event System.EventHandler<ValueChangedEventArgs<int>> PrecisionChanged;
		private HMINumericDisplay2ViewModel ViewModel
		{
			get
			{
				return base.DataContext as HMINumericDisplay2ViewModel;
			}
		}
		public int Digits
		{
			get
			{
				return (int)base.GetValue(HMINumericDisplay2.DigitsProperty);
			}
			set
			{
				base.SetValue(HMINumericDisplay2.DigitsProperty, value);
			}
		}
		public HMINumericDisplay2DisplayType DisplayType
		{
			get
			{
				return (HMINumericDisplay2DisplayType)base.GetValue(HMINumericDisplay2.DisplayTypeProperty);
			}
			set
			{
				base.SetValue(HMINumericDisplay2.DisplayTypeProperty, value);
			}
		}
		public int Precision
		{
			get
			{
				return (int)base.GetValue(HMINumericDisplay2.PrecisionProperty);
			}
			set
			{
				base.SetValue(HMINumericDisplay2.PrecisionProperty, value);
			}
		}
		static HMINumericDisplay2()
		{
			HMINumericDisplay2.DigitsProperty = System.Windows.DependencyProperty.Register("Digits", typeof(int), typeof(HMINumericDisplay2), new System.Windows.PropertyMetadata(6, new System.Windows.PropertyChangedCallback(HMINumericDisplay2.OnDigitsChanged)));
			HMINumericDisplay2.DisplayTypeProperty = System.Windows.DependencyProperty.Register("DisplayType", typeof(HMINumericDisplay2DisplayType), typeof(HMINumericDisplay2), new System.Windows.PropertyMetadata(HMINumericDisplay2DisplayType.HexagonalSevenSegments, new System.Windows.PropertyChangedCallback(HMINumericDisplay2.OnDisplayTypeChanged)));
			HMINumericDisplay2.PrecisionProperty = System.Windows.DependencyProperty.Register("Precision", typeof(int), typeof(HMINumericDisplay2), new System.Windows.PropertyMetadata(2, new System.Windows.PropertyChangedCallback(HMINumericDisplay2.OnPrecisionChanged)));
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMINumericDisplay2), new System.Windows.FrameworkPropertyMetadata(typeof(HMINumericDisplay2)));
			DependencyPropertyMetadata.OverridePropertyCallback<HMINumericDisplay2>(System.Windows.Controls.Control.ForegroundProperty, new System.Windows.PropertyChangedCallback(HMINumericDisplay2.BaseControl_PropertyChanged));
			DependencyPropertyMetadata.OverridePropertyCallback<HMINumericDisplay2>(System.Windows.Controls.Control.BackgroundProperty, new System.Windows.PropertyChangedCallback(HMINumericDisplay2.BaseControl_PropertyChanged));
			DependencyPropertyMetadata.OverridePropertyCallback<HMINumericDisplay2>(System.Windows.Controls.Control.FontFamilyProperty, new System.Windows.PropertyChangedCallback(HMINumericDisplay2.BaseControl_PropertyChanged));
			DependencyPropertyMetadata.OverridePropertyCallback<HMINumericDisplay2>(System.Windows.Controls.Control.FontSizeProperty, new System.Windows.PropertyChangedCallback(HMINumericDisplay2.BaseControl_PropertyChanged));
			DependencyPropertyMetadata.OverridePropertyCallback<HMINumericDisplay2>(System.Windows.Controls.Control.FontStretchProperty, new System.Windows.PropertyChangedCallback(HMINumericDisplay2.BaseControl_PropertyChanged));
			DependencyPropertyMetadata.OverridePropertyCallback<HMINumericDisplay2>(System.Windows.Controls.Control.FontStyleProperty, new System.Windows.PropertyChangedCallback(HMINumericDisplay2.BaseControl_PropertyChanged));
			DependencyPropertyMetadata.OverridePropertyCallback<HMINumericDisplay2>(System.Windows.Controls.Control.FontWeightProperty, new System.Windows.PropertyChangedCallback(HMINumericDisplay2.BaseControl_PropertyChanged));
		}
		private static void BaseControl_PropertyChanged(System.Windows.DependencyObject d, System.Windows.DependencyPropertyChangedEventArgs e)
		{
			HMINumericDisplay2 wFNumericDisplay = d as HMINumericDisplay2;
			if (wFNumericDisplay.numericIndicator == null)
			{
				return;
			}
			wFNumericDisplay.CreateDigits();
		}
		private static void OnDigitsChanged(System.Windows.DependencyObject sender, System.Windows.DependencyPropertyChangedEventArgs args)
		{
			HMINumericDisplay2 wFNumericDisplay = (HMINumericDisplay2)sender;
			System.EventHandler<ValueChangedEventArgs<int>> digitsChanged = wFNumericDisplay.DigitsChanged;
			if (digitsChanged != null)
			{
				digitsChanged(sender, new ValueChangedEventArgs<int>((int)args.OldValue, (int)args.NewValue));
			}
		}
		private static void OnDisplayTypeChanged(System.Windows.DependencyObject sender, System.Windows.DependencyPropertyChangedEventArgs args)
		{
			HMINumericDisplay2 wFNumericDisplay = (HMINumericDisplay2)sender;
			System.EventHandler<ValueChangedEventArgs<HMINumericDisplay2DisplayType>> displayTypeChanged = wFNumericDisplay.DisplayTypeChanged;
			if (displayTypeChanged != null)
			{
				displayTypeChanged(sender, new ValueChangedEventArgs<HMINumericDisplay2DisplayType>((HMINumericDisplay2DisplayType)args.OldValue, (HMINumericDisplay2DisplayType)args.NewValue));
			}
		}
		private static void OnPrecisionChanged(System.Windows.DependencyObject sender, System.Windows.DependencyPropertyChangedEventArgs args)
		{
			HMINumericDisplay2 wFNumericDisplay = (HMINumericDisplay2)sender;
			System.EventHandler<ValueChangedEventArgs<int>> precisionChanged = wFNumericDisplay.PrecisionChanged;
			if (precisionChanged != null)
			{
				precisionChanged(sender, new ValueChangedEventArgs<int>((int)args.OldValue, (int)args.NewValue));
			}
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.numericScale = (base.GetTemplateChild("uxNumericScaleElement") as NumericScale);
			this.numericIndicator = (base.GetTemplateChild("uxNumericIndicatorElement") as NumericIndicator);
			if (this.numericScale != null)
			{
				this.numericScale.Max=double.MaxValue;
				this.numericScale.Min=double.MinValue;
			}
			this.CreateDigits();
			this.DigitsChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMINumericDisplay2_DigitsChanged);
			this.DisplayTypeChanged += new System.EventHandler<ValueChangedEventArgs<HMINumericDisplay2DisplayType>>(this.HMINumericDisplay2_DisplayTypeChanged);
			this.PrecisionChanged += new System.EventHandler<ValueChangedEventArgs<int>>(this.HMINumericDisplay2_PrecisionChanged);
			if (this.ViewModel != null)
			{
				this.ViewModel.DisplayTypeChanged += new System.EventHandler<ValueChangedEventArgs<HMINumericDisplay2DisplayType>>(this.ViewModel_DisplayTypeChanged);
			}
		}
		private void ViewModel_DisplayTypeChanged(object sender, ValueChangedEventArgs<HMINumericDisplay2DisplayType> e)
		{
			this.CreateDigits();
		}
		private void HMINumericDisplay2_PrecisionChanged(object sender, ValueChangedEventArgs<int> e)
		{
			this.CreateDigits();
		}
		private void HMINumericDisplay2_DisplayTypeChanged(object sender, ValueChangedEventArgs<HMINumericDisplay2DisplayType> e)
		{
			this.CreateDigits();
		}
		private void HMINumericDisplay2_DigitsChanged(object sender, ValueChangedEventArgs<int> e)
		{
			this.CreateDigits();
		}
		private void CreateNewNumericIndicator()
		{
			NumericIndicator numericIndicator = new NumericIndicator();

            System.Windows.Data.Binding binding = new System.Windows.Data.Binding("Background");
            binding.Source = this;
            numericIndicator.SetBinding(System.Windows.Controls.Control.BackgroundProperty, binding);
            //numericIndicator.Background = null;

			numericIndicator.BorderThickness = new System.Windows.Thickness(0.0);

			//numericIndicator.Foreground = new SolidColorBrush(Color.FromArgb(255,0,0,0));
            binding = new System.Windows.Data.Binding("Foreground");
            binding.Source = this;
            numericIndicator.SetBinding(System.Windows.Controls.Control.ForegroundProperty, binding);

            ScaleObject.SetRelativeX(numericIndicator, 0.0);
			ScaleObject.SetRelativeY(numericIndicator, 0.0);
			ScaleObject.SetRelativeWidth(numericIndicator, new GaugeMeasure(1.0, GridUnitType.Star));
			ScaleObject.SetRelativeHeight(numericIndicator, new GaugeMeasure(1.0, GridUnitType.Star));

			numericIndicator.FontFamily = base.FontFamily;
			numericIndicator.Format = (string.Format("{{0:F{0}}}", this.Precision));
			numericIndicator.Value = (this.GetSampleValue(this.Digits, this.Precision));
			this.numericIndicator = numericIndicator;

        }
		private void AddNumberPositionBindings(NumberPosition numberPosition)
		{
			System.Windows.Data.Binding binding = new System.Windows.Data.Binding("Background");
			binding.Source = this;
			numberPosition.SetBinding(System.Windows.Controls.Control.BackgroundProperty, binding);
			binding = new System.Windows.Data.Binding("Foreground");
			binding.Source = this;
			numberPosition.SetBinding(System.Windows.Controls.Control.ForegroundProperty, binding);
			binding = new System.Windows.Data.Binding("FontFamily");
			binding.Source = this;
			numberPosition.SetBinding(System.Windows.Controls.Control.FontFamilyProperty, binding);
			binding = new System.Windows.Data.Binding("FontSize");
			binding.Source = this;
			numberPosition.SetBinding(System.Windows.Controls.Control.FontSizeProperty, binding);
			binding = new System.Windows.Data.Binding("FontStretch");
			binding.Source = this;
			numberPosition.SetBinding(System.Windows.Controls.Control.FontStretchProperty, binding);
			binding = new System.Windows.Data.Binding("FontStyle");
			binding.Source = this;
			numberPosition.SetBinding(System.Windows.Controls.Control.FontStyleProperty, binding);
			binding = new System.Windows.Data.Binding("FontWeight");
			binding.Source = this;
			numberPosition.SetBinding(System.Windows.Controls.Control.FontWeightProperty, binding);
		}
		private NumberPosition CreateNumberPosition()
		{
			NumberPosition numberPosition= GetNumberPosition();

			this.AddNumberPositionBindings(numberPosition);
			return numberPosition;
		}
		private void AddDigitNumberPositions()
		{
			for (int i = 0; i < this.Digits; i++)
			{
				NumberPosition newItem = this.CreateNumberPosition();
				this.numericIndicator.Positions.Add(newItem);
			}
		}
		private void CreateDigits()
		{
			if (this.numericIndicator == null)
			{
				return;
			}
			this.numericScale.Indicators.Clear();
			this.CreateNewNumericIndicator();
			this.AddDigitNumberPositions();
			this.numericScale.Indicators.Add(this.numericIndicator);
            //this.numericIndicator.Refresh();

        }
		private double GetSampleValue(int digits, int precision)
		{
			double result = double.NaN;
			string text = string.Empty;
			if (precision == 0)
			{
				text = new string('8', digits);
			}
			else
			{
				text = new string('8', digits - precision - 1);
				text += ".";
				text += new string('8', precision);
			}
			double.TryParse(text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out result);
			return result;
		}
		private NumberPosition GetNumberPosition()
		{
			switch (this.DisplayType)
			{
			case HMINumericDisplay2DisplayType.FontFamily:
				return new FontNumberPosition();
			case HMINumericDisplay2DisplayType.HexagonalSevenSegments:
				return new HexagonalNumberPosition();
			case HMINumericDisplay2DisplayType.SevenSegments:
				return new SevenSegsNumberPosition();
			//case HMINumericDisplay2DisplayType.Graphic:
			//	return new GraphicNumberPosition();
			default:
				throw new System.ArgumentOutOfRangeException("HMINumericDisplay2DisplayType");
			}
		}
	}
}
