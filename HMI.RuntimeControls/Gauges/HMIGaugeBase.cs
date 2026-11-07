// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared;
namespace HMI.RuntimeControls.Gauges
{
	public abstract class HMIGaugeBase : Control, IDisposable
	{
		private string decimalSeparator;
		private bool hasProjectAuthorization;
		private bool authorizationRequired;
		private double currentValue;
		private IHMIConnector connector;
        private TextBlock? unitLabel;
        protected TextBlock? levelValue;
        private Storyboard? startAnimation;
        private OnSignalChangedDelegate signalValueChangedHandler;

        public static readonly DependencyProperty UnitProperty = DependencyProperty.Register("Unit", typeof(string), typeof(HMIGaugeBase), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIGaugeBase.UpdateUnitLabel)));
		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIGaugeBase), new PropertyMetadata(string.Empty, null));
		public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register("Precision", typeof(int), typeof(HMIGaugeBase), new PropertyMetadata(2, new PropertyChangedCallback(HMIGaugeBase.OnPrecisionChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIGaugeBase), new PropertyMetadata(string.Empty, null));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIGaugeBase), new PropertyMetadata(string.Empty, null));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIGaugeBase), new PropertyMetadata(string.Empty, null));
		public static readonly DependencyProperty MaximumValueProperty = DependencyProperty.Register("MaximumValue", typeof(double), typeof(HMIGaugeBase), new PropertyMetadata(100.0, new PropertyChangedCallback(HMIGaugeBase.OnMaximumValueChanged)));
		public static readonly DependencyProperty MinimumValueProperty = DependencyProperty.Register("MinimumValue", typeof(double), typeof(HMIGaugeBase), new PropertyMetadata(0.0, new PropertyChangedCallback(HMIGaugeBase.OnMinimumValueChanged)));
		public static readonly DependencyProperty UnitBackgroundProperty = DependencyProperty.Register("UnitBackground", typeof(Brush), typeof(HMIGaugeBase), new PropertyMetadata(new SolidColorBrush(Colors.Gray), new PropertyChangedCallback(HMIGaugeBase.OnUnitBackgroundChanged)));
		public static readonly DependencyProperty ValueBackgroundProperty = DependencyProperty.Register("ValueBackground", typeof(Brush), typeof(HMIGaugeBase), new PropertyMetadata(new SolidColorBrush(Colors.Gray), new PropertyChangedCallback(HMIGaugeBase.OnValueBackgroundChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIGaugeBase), new PropertyMetadata(string.Empty, null));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIGaugeBase), new PropertyMetadata(DenyAccessBehaviour.Hide, null));
		
		[Category("HMI")]
		public string Unit
		{
			get
			{
				return (string)base.GetValue(HMIGaugeBase.UnitProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.UnitProperty, value);
			}
		}
		[Category("HMI")]
		public string ObjectID
		{
			get
			{
				return (string)base.GetValue(HMIGaugeBase.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public int Precision
		{
			get
			{
				return (int)base.GetValue(HMIGaugeBase.PrecisionProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.PrecisionProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMIGaugeBase.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMIGaugeBase.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.SignalPrefixProperty, value);
			}
		}
		[Category("HMI")]
		public double MaximumValue
		{
			get
			{
				return (double)base.GetValue(HMIGaugeBase.MaximumValueProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.MaximumValueProperty, value);
			}
		}
		[Category("HMI")]
		public double MinimumValue
		{
			get
			{
				return (double)base.GetValue(HMIGaugeBase.MinimumValueProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.MinimumValueProperty, value);
			}
		}
		[Category("HMI")]
		public Brush UnitBackground
		{
			get
			{
				return (Brush)base.GetValue(HMIGaugeBase.UnitBackgroundProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.UnitBackgroundProperty, value);
			}
		}
		[Category("HMI")]
		public Brush ValueBackground
		{
			get
			{
				return (Brush)base.GetValue(HMIGaugeBase.ValueBackgroundProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.ValueBackgroundProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIGaugeBase.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIGaugeBase.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIGaugeBase.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		public HMIGaugeBase()
		{
			base.DefaultStyleKey=(typeof(HMIGaugeBase));
			this.decimalSeparator = HMIGaugeBase.GetDecimalSeparator();
		}
		internal double GetCurrentValue()
		{
			return this.currentValue;
		}
		internal void SetCurrentValue(double value)
		{
			if (this.currentValue == value)
			{
				return;
			}
			this.currentValue = value;
		}
		private static void UpdateUnitLabel(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIGaugeBase gauge = dependencyObject as HMIGaugeBase;
			if (gauge != null && gauge.unitLabel != null)
			{
				gauge.unitLabel.Text=(change.NewValue.ToString());
			}
		}
		private static void OnPrecisionChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIGaugeBase gauge = dependencyObject as HMIGaugeBase;
			if (gauge != null)
			{
				int? precision = HMIGaugeBase.ClampInteger((int)change.NewValue, 0, 10);
				if (precision.HasValue)
				{
					gauge.Precision = precision.Value;
					return;
				}
				if (gauge.levelValue != null)
				{
					gauge.levelValue.Text=(HMIGaugeBase.CreatePrecisionPlaceholder((int)change.NewValue));
				}
			}
		}
		private string GetServerName()
		{
			return (string)base.GetValue(HMIGaugeBase.ServerNameProperty);
		}
		private static void OnMaximumValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIGaugeBase gauge = dependencyObject as HMIGaugeBase;
			if (gauge != null)
			{
				gauge.OnGaugeRangeChanged();
			}
		}
		private static void OnMinimumValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIGaugeBase gauge = dependencyObject as HMIGaugeBase;
			if (gauge != null)
			{
				gauge.OnGaugeRangeChanged();
			}
		}
		private static void OnUnitBackgroundChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIGaugeBase gauge = dependencyObject as HMIGaugeBase;
			if (gauge != null && gauge.unitLabel != null)
			{
				gauge.unitLabel.Foreground=((Brush)change.NewValue);
			}
		}
		private static void OnValueBackgroundChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
			HMIGaugeBase gauge = dependencyObject as HMIGaugeBase;
			if (gauge != null && gauge.levelValue != null)
			{
				gauge.levelValue.Foreground=((Brush)change.NewValue);
			}
		}
		internal static int? ClampInteger(int value, int minimum, int maximum)
		{
			if (value < minimum)
			{
				return new int?(minimum);
			}
			if (value > maximum)
			{
				return new int?(maximum);
			}
			return default(int?);
		}
		internal static double? ClampDouble(double value, double minimum, double maximum)
		{
			if (value < minimum)
			{
				return new double?(minimum);
			}
			if (value > maximum)
			{
				return new double?(maximum);
			}
			return default(double?);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.InitializeGaugeConnector();
			this.GetGaugeTemplateParts();
			this.RegisterConfiguredSignal();
			this.UpdateAuthorizationState();
			this.StartAnimation();
			this.RefreshGaugeAppearance();
		}
		internal static string CreatePrecisionPlaceholder(int decimalPlaces)
		{
			string text = "---";
			if (decimalPlaces > 0)
			{
				text += HMIGaugeBase.GetDecimalSeparator();
			}
			for (int i = 0; i < decimalPlaces; i++)
			{
				text += "-";
			}
			return text;
		}
		private static string GetDecimalSeparator()
		{
			return CultureInfo.CurrentUICulture.NumberFormat.CurrencyDecimalSeparator;
		}
        protected virtual void GetGaugeTemplateParts()
		{
			this.unitLabel = (TextBlock)base.GetTemplateChild("UnitLabel");
			this.levelValue = (TextBlock)base.GetTemplateChild("LevelValue");
			this.startAnimation = (Storyboard)base.GetTemplateChild("StartAnimation");
		}
        protected virtual void RefreshGaugeAppearance()
		{
			this.RestoreUnitBackground();
			this.RestoreValueBackground();
		}
		private void RestoreUnitBackground()
		{
			Brush unitBackground = this.UnitBackground;
			this.UnitBackground = new LinearGradientBrush();
			this.UnitBackground = unitBackground;
		}
		private void RestoreValueBackground()
		{
			Brush valueBackground = this.ValueBackground;
			this.ValueBackground = new LinearGradientBrush();
			this.ValueBackground = valueBackground;
		}
		internal void StartAnimation()
		{
			if (this.startAnimation != null && !DesignerProperties.GetIsInDesignMode(this))
			{
				this.startAnimation.Begin();
			}
		}
		internal virtual void InitializeGaugeConnector()
		{
			bool isInDesignTool = DesignerPropertiesExt.IsInDesignTool;
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			if (isInDesignTool)
			{
				return;
			}
			this.RegisterLanguage();
		}
		private void RegisterLanguage()
		{
			if (string.IsNullOrEmpty(this.Unit))
			{
				return;
			}
			this.connector.RegisterLanguageChangedHandler(new string[]
			{
				this.Unit
			}, new OnLanguageChangedDelegate(this.OnUnitLanguageChanged));
		}
		internal virtual void InitializeGaugeValueDisplay()
		{
			if (this.unitLabel != null)
			{
				this.unitLabel.Text=(this.Unit);
			}
			if (this.levelValue != null)
			{
				this.levelValue.Text=(HMIGaugeBase.CreatePrecisionPlaceholder(this.Precision));
			}
		}
		internal virtual void UpdateGaugeValueDisplay(object signalValue)
		{
			if (this.levelValue != null)
			{
				this.levelValue.Text=(this.FormatValueToPrecision(signalValue.ToString()));
			}
		}
		internal string FormatValueToPrecision(string formattedValue)
		{
			if (formattedValue.Contains(this.decimalSeparator))
			{
				int truncateIndex;
				if (this.Precision == 0)
				{
					truncateIndex = formattedValue.LastIndexOf(this.decimalSeparator) + this.Precision;
				}
				else
				{
					truncateIndex = formattedValue.LastIndexOf(this.decimalSeparator) + this.Precision + 1;
				}
				if (truncateIndex < formattedValue.Length)
				{
					formattedValue = formattedValue.Remove(truncateIndex);
				}
			}
			return formattedValue;
		}
		protected virtual void OnGaugeRangeChanged()
		{
		}
		private void RegisterConfiguredSignal()
		{
			this.SignalName = this.SignalName.Trim();
			if (!string.IsNullOrEmpty(this.SignalName))
			{
				this.signalValueChangedHandler = new OnSignalChangedDelegate(this.OnSignalValueChangedHandler);
				this.RegisterSignal(string.Empty, this.SignalName, this.signalValueChangedHandler);
			}
		}
		protected virtual void OnSignalValueChangedHandler(string signalName, object signalValue)
		{
			if (!base.IsEnabled)
			{
				signalValue = 0.0;
				this.SetCurrentValue(0.0);
				return;
			}
			this.SetCurrentValue(this.ConvertSignalValue(signalValue));
		}
		protected void RegisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
				this.connector.RegisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		protected void UnregisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
		{
			try
			{
				this.connector.UnregisterSignalChangedHandler(serverName, signalName, signalChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void OnUnitLanguageChanged(string[] previousValues, string[] translatedValues)
		{
			if (translatedValues.Length <= 0)
			{
				return;
			}
			this.Unit = translatedValues[0];
		}
		private void UpdateAuthorizationState()
		{
			if (!DesignerProperties.GetIsInDesignMode(this))
			{
				if (string.IsNullOrEmpty(this.SecurityAuthorizationName))
				{
					this.hasProjectAuthorization = true;
					this.authorizationRequired = false;
					return;
				}
				this.hasProjectAuthorization = false;
				this.authorizationRequired = true;
				switch (this.SecurityDenyAccessBehaviour)
				{
				case DenyAccessBehaviour.Disable:
					this.UpdateEnabledState(this.hasProjectAuthorization);
					break;
				case DenyAccessBehaviour.Hide:
					base.Visibility= System.Windows.Visibility.Collapsed;
					break;
				default:
					throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
				}
				this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.OnUserContextChanged);
			}
		}
		private void UpdateVisibilityBasedOnAuthorization(bool isAuthorized)
		{
			if (!isAuthorized)
			{
				base.Visibility= System.Windows.Visibility.Collapsed;
				return;
			}
			if (this.ShouldHideGauge())
			{
				base.Visibility= System.Windows.Visibility.Collapsed;
				return;
			}
			base.Visibility= System.Windows.Visibility.Visible;
		}
		private void UpdateEnabledState(bool isAuthorized)
		{
			if (!isAuthorized)
			{
				this.DisableGauge();
				return;
			}
			if (this.ShouldDisableGauge())
			{
				this.DisableGauge();
				return;
			}
			this.EnableGauge();
		}
		private void EnableGauge()
		{
			base.IsEnabled=(true);
			VisualStateManager.GoToState(this, "Enabled", false);
		}
		private void DisableGauge()
		{
			base.IsEnabled=(false);
			VisualStateManager.GoToState(this, "Disabled", false);
		}
		private bool ShouldDisableGauge()
		{
			return this.authorizationRequired && !this.hasProjectAuthorization && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Disable;
		}
		private bool ShouldHideGauge()
		{
			return this.authorizationRequired && !this.hasProjectAuthorization && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Hide;
		}
		private void OnUserContextChanged(string userName)
		{
			if (!this.authorizationRequired)
			{
				return;
			}
			this.hasProjectAuthorization = this.connector.CheckProjectAuthorization(this.SecurityAuthorizationName);
			switch (this.SecurityDenyAccessBehaviour)
			{
			case DenyAccessBehaviour.Disable:
				this.UpdateEnabledState(this.hasProjectAuthorization);
				return;
			case DenyAccessBehaviour.Hide:
				this.UpdateVisibilityBasedOnAuthorization(this.hasProjectAuthorization);
				return;
			default:
				throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
			}
		}
		protected virtual double ConvertSignalValue(object signalValue)
		{
			double result = double.NaN;
			try
			{
				result = double.Parse(signalValue.ToString());
			}
			catch
			{
				result = double.NaN;
			}
			return result;
		}
		public virtual void Dispose()
		{
			this.UnregisterGauge(true);
			GC.SuppressFinalize(this);
		}
        protected void UnregisterGauge(bool isDisposing)
		{
			if (!isDisposing)
			{
				return;
			}
			if (this.connector == null)
			{
				return;
			}
			this.CleanupSignalSubscriptions();
		}
		private void CleanupSignalSubscriptions()
		{
			this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.OnUserContextChanged);
			this.connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnUnitLanguageChanged));
			if (this.signalValueChangedHandler != null)
			{
				this.UnregisterSignal(this.GetServerName(), this.SignalName, this.signalValueChangedHandler);
			}
			this.connector.Dispose();
		}
	}
}
