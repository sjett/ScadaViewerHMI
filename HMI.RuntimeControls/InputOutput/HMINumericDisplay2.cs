// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Telerik.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using Telerik.Windows.Controls.Gauge;
namespace HMI.RuntimeControls.InputOutput
{
	public class HMINumericDisplay2 : HMIStaticStatesControlBase
	{
		private const string ElementNumericScale = "NumericScaleElement";
		private const string ElementNumericIndicator = "NumericIndicatorElement";
		private const string ElementUnit = "UnitElement";

		private bool isInitialized;
		private double signalValue;

		private NumericScale numericScale;
		private NumericIndicator numericIndicator;
		private TextBlock tbUnitElement;

		private static readonly DependencyProperty ForegroundInternalProperty = DependencyProperty.Register("ForegroundInternal", typeof(Brush), typeof(HMINumericDisplay2), new PropertyMetadata(new SolidColorBrush(Colors.Black), new PropertyChangedCallback(HMINumericDisplay2.OnForegroundInternalPropertyChanged)));
		public static readonly DependencyProperty DigitsProperty = DependencyProperty.Register("Digits", typeof(int), typeof(HMINumericDisplay2), new PropertyMetadata(6, new PropertyChangedCallback(HMINumericDisplay2.OnDigitsPropertyChanged)));
		public static readonly DependencyProperty DisplayTypeProperty = DependencyProperty.Register("DisplayType", typeof(HMINumericDisplay2DisplayType), typeof(HMINumericDisplay2), new PropertyMetadata(HMINumericDisplay2DisplayType.FontFamily, new PropertyChangedCallback(HMINumericDisplay2.OnDisplayTypePropertyChanged)));
		public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register("Precision", typeof(int), typeof(HMINumericDisplay2), new PropertyMetadata(2, new PropertyChangedCallback(HMINumericDisplay2.OnPrecisionPropertyChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMINumericDisplay2), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMINumericDisplay2.OnSignalNamePropertyChanged)));
		public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(string), typeof(HMINumericDisplay2), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMINumericDisplay2.OnUnitPropertyChanged)));
		
		[Category("HMI")]
		private Brush ForegroundInternal
		{
			get
			{
				return (Brush)base.GetValue(HMINumericDisplay2.ForegroundInternalProperty);
			}
			set
			{
				base.SetValue(HMINumericDisplay2.ForegroundInternalProperty, value);
			}
		}
		[Category("HMI")]
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
		[Category("HMI")]
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
		[Category("HMI")]
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
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMINumericDisplay2.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMINumericDisplay2.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public string Unit
		{
			get
			{
				return (string)base.GetValue(HMINumericDisplay2.UnitProperty);
			}
			set
			{
				base.SetValue(HMINumericDisplay2.UnitProperty, value);
			}
		}
		private static void OnForegroundInternalPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMINumericDisplay2? wFNumericDisplay = d as HMINumericDisplay2;
			if (wFNumericDisplay != null && wFNumericDisplay.numericIndicator != null)
			{
				//wFNumericDisplay.numericIndicator.Refresh();
				return;
			}
		}
		private static void OnDigitsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMINumericDisplay2? wFNumericDisplay = d as HMINumericDisplay2;
			if (wFNumericDisplay == null)
			{
				return;
			}
			int digits = (int)e.OldValue;
			int num = (int)e.NewValue;
			if (num < 1)
			{
				wFNumericDisplay.Digits = digits;
				return;
			}
			wFNumericDisplay.CreateDigits();
		}
		private static void OnDisplayTypePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMINumericDisplay2? wFNumericDisplay = d as HMINumericDisplay2;
			if (wFNumericDisplay == null)
			{
				return;
			}
			wFNumericDisplay.CreateDigits();
		}
		private static void OnPrecisionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMINumericDisplay2? wFNumericDisplay = d as HMINumericDisplay2;
			if (wFNumericDisplay == null)
			{
				return;
			}
			int precision = (int)e.OldValue;
			int num = (int)e.NewValue;
			if (num < 0)
			{
				wFNumericDisplay.Precision = precision;
				return;
			}
			wFNumericDisplay.CreateDigits();
		}
		private static void OnSignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnUnitPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMINumericDisplay2()
		{
			base.DefaultStyleKey=(typeof(HMINumericDisplay2));
			this.signalValue = double.NaN;
		}
		private void OnSignalValueChangedHandler(string signalName, object signalValue)
		{
			this.signalValue = HMINumericSignal.ConvertSignalValueToDouble(signalValue);
			if (this.numericIndicator == null)
			{
				return;
			}
			this.numericIndicator.Value=(this.signalValue);
		}
		private void OnUnitTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
		{
			if (translationTexts.Length <= 0)
			{
				return;
			}
			this.UpdateUnitElementText(translationTexts[0]);
		}
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				this.UnregisterConnector();
			}
			base.Dispose(disposing);
		}
		private void UnregisterConnector()
		{
			if (base.Connector == null)
			{
				return;
			}
			this.UnregisterSignal(string.Empty, this.SignalName, new OnSignalChangedDelegate(this.OnSignalValueChangedHandler));
			base.Connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnUnitTranslationsChangedHandler));
		}
		public override void OnApplyTemplate()
		{
			this.numericScale = null;
			this.numericIndicator = null;
			this.tbUnitElement = null;
			base.OnApplyTemplate();
			this.numericScale = (base.GetTemplateChild("NumericScaleElement") as NumericScale);
			this.numericIndicator = (base.GetTemplateChild("NumericIndicatorElement") as NumericIndicator);
			this.tbUnitElement = (base.GetTemplateChild("UnitElement") as TextBlock);
			if (this.numericScale != null)
			{
				this.numericScale.Max=(1.7976931348623157E+308);
				this.numericScale.Min=(-1.7976931348623157E+308);
			}
			this.CreateDigits();
			Binding binding = new Binding("Foreground");
			binding.Source=(this);
			Binding binding2 = binding;
			base.SetBinding(HMINumericDisplay2.ForegroundInternalProperty, binding2);
			if (!base.IsInDesignMode && !this.isInitialized)
			{
				this.isInitialized = true;
				this.InitializeSignalName();
				this.InitializeUnit();
				return;
			}
		}
		private void InitializeSignalName()
		{
			if (string.IsNullOrEmpty(this.SignalName))
			{
				return;
			}
			this.SignalName = base.SignalPrefix + this.SignalName;
			this.SignalName = StringPlaceholdersResolver.Resolve(this.SignalName, this);
			this.RegisterSignal(string.Empty, this.SignalName, new OnSignalChangedDelegate(this.OnSignalValueChangedHandler));
		}
		private void InitializeUnit()
		{
			if (string.IsNullOrEmpty(this.Unit))
			{
				return;
			}
			this.Unit = StringPlaceholdersResolver.Resolve(this.Unit, this);
			base.Connector.RegisterLanguageChangedHandler(new string[]
			{
				this.Unit
			}, new OnLanguageChangedDelegate(this.OnUnitTranslationsChangedHandler));
		}
		private void UpdateUnitElementText(string unitTranslation)
		{
			if (this.tbUnitElement == null)
			{
				return;
			}
			try
			{
				this.tbUnitElement.Text=(unitTranslation);
			}
			catch (Exception ex)
			{
				base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void CreateDigits()
		{
			if (this.numericIndicator == null)
			{
				return;
			}
			this.numericScale.Indicators.Clear();
			this.numericIndicator = null;
			NumericIndicator numericIndicator = new NumericIndicator();
			numericIndicator.Background=(null);
			numericIndicator.BorderThickness=(new Thickness(0.0));
			numericIndicator.Foreground=(null);

            ScaleObject.SetRelativeX(numericIndicator, 0.0);
            ScaleObject.SetRelativeY(numericIndicator, 0.0);
            ScaleObject.SetRelativeWidth(numericIndicator, new GaugeMeasure(1.0, GridUnitType.Star));
            ScaleObject.SetRelativeHeight(numericIndicator, new GaugeMeasure(1.0, GridUnitType.Star));

            numericIndicator.Format=(string.Format("{{0:F{0}}}", this.Precision));
			numericIndicator.Value=(0.0);

			this.numericIndicator = numericIndicator;
			this.numericIndicator.Value=(base.IsInDesignMode ? 0.0 : this.signalValue);

			for (int i = 0; i < this.Digits; i++)
			{
				NumberPosition numberPosition = this.GetNumberPosition();
				numberPosition.CornerRadius=(new CornerRadius(0.0));

				Binding binding = new Binding("Background");
				binding.Source=(this);
				Binding binding2 = binding;
				numberPosition.SetBinding(Control.BackgroundProperty, binding2);
				Binding binding3 = new Binding("Foreground");
				binding3.Source=(this);
				binding2 = binding3;
				numberPosition.SetBinding(Control.ForegroundProperty, binding2);
				Binding binding4 = new Binding("FontFamily");
				binding4.Source=(this);
				binding2 = binding4;
				numberPosition.SetBinding(Control.FontFamilyProperty, binding2);
				Binding binding5 = new Binding("FontSize");
				binding5.Source=(this);
				binding2 = binding5;
				numberPosition.SetBinding(Control.FontSizeProperty, binding2);
				Binding binding6 = new Binding("FontStretch");
				binding6.Source=(this);
				binding2 = binding6;
				numberPosition.SetBinding(Control.FontStretchProperty, binding2);
				Binding binding7 = new Binding("FontStyle");
				binding7.Source=(this);
				binding2 = binding7;
				numberPosition.SetBinding(Control.FontStyleProperty, binding2);
				Binding binding8 = new Binding("FontWeight");
				binding8.Source=(this);
				binding2 = binding8;
				numberPosition.SetBinding(Control.FontWeightProperty, binding2);

				this.numericIndicator.Positions.Add(numberPosition);
			}

			this.numericScale.Indicators.Add(this.numericIndicator);
		}
		private double GetSignalValueInDesignMode(int digits, int precision)
		{
			double result = double.NaN;
			string text = string.Empty;
			if (precision == 0)
			{
				text += new string('8', digits);
			}
			else
			{
				text += new string('8', digits - precision - 1);
				text += ".";
				text += new string('8', precision);
			}
			double.TryParse(text, NumberStyles.Any, CultureInfo.InvariantCulture, out result);
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
				default:
					throw new ArgumentOutOfRangeException("HMINumericDisplay2DisplayType");
			}
		}
		protected void RegisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
				base.Connector.RegisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
				base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		protected void UnregisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
				base.Connector.UnregisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
				base.Connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
	}
}
