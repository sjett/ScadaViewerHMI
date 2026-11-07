// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Common.Converters;
namespace HMI.RuntimeControls.InputOutput
{
	[TemplatePart(Name = "OutputElement", Type = typeof(TextBlock))]
	public class HMIExtendedNumericDisplay1 : Control, IDisposable
	{
		private const string ElementOutput = "OutputElement";

		private TextBlock tbOutputElement;

		private IHMIConnector connector;

		private OnSignalChangedDelegate delegateSignalValueChanged;
		private OnSignalChangedDelegate delegateUnitSignalValueChanged;
		private OnSignalChangedDelegate delegateVisibilitySignalValueChanged;

		private double signalValue;
		private double unitSignalValue;
		private bool canShowControlFromVisibilitySignal;
		private bool isSecurityAvailable;
		private bool isAuthorized;

		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnObjectIDPropertyChanged)));
		public static readonly DependencyProperty PrecisionProperty = DependencyProperty.Register("Precision", typeof(int), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(2, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnPrecisionPropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnSignalNamePropertyChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnSignalPrefixPropertyChanged)));
		public static readonly DependencyProperty Unit1Property = DependencyProperty.Register("Unit1", typeof(string), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnUnit1PropertyChanged)));
		public static readonly DependencyProperty Unit2Property = DependencyProperty.Register("Unit2", typeof(string), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnUnit2PropertyChanged)));
		public static readonly DependencyProperty Unit1ScaleFactorProperty = DependencyProperty.Register("Unit1ScaleFactor", typeof(double), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(1.0, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnUnit1ScaleFactorPropertyChanged)));
		public static readonly DependencyProperty Unit2ScaleFactorProperty = DependencyProperty.Register("Unit2ScaleFactor", typeof(double), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(1.0, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnUnit2ScaleFactorPropertyChanged)));
		public static readonly DependencyProperty UnitSignalNameProperty = DependencyProperty.Register("UnitSignalName", typeof(string), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnUnitSignalNamePropertyChanged)));
		public static readonly DependencyProperty UnitSignalNameMaskProperty = DependencyProperty.Register("UnitSignalNameMask", typeof(int?), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnUnitSignalNameMaskPropertyChanged)));
		public static readonly DependencyProperty VisibilityMaskProperty = DependencyProperty.Register("VisibilityMask", typeof(int?), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(null, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnVisibilityMaskPropertyChanged)));
		public static readonly DependencyProperty VisibilitySignalNameProperty = DependencyProperty.Register("VisibilitySignalName", typeof(string), typeof(HMIExtendedNumericDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIExtendedNumericDisplay1.OnVisibilitySignalNamePropertyChanged)));
		
		private IHMIConnector Connector
		{
			get
			{
				return this.connector;
			}
		}
		private bool IsAuthorized
		{
			get
			{
				return this.isAuthorized;
			}
		}
		[Category("HMI")]
		public string ObjectID
		{
			get
			{
				return (string)base.GetValue(HMIExtendedNumericDisplay1.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public int Precision
		{
			get
			{
				return (int)base.GetValue(HMIExtendedNumericDisplay1.PrecisionProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.PrecisionProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIExtendedNumericDisplay1.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIExtendedNumericDisplay1.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		private string ServerName
		{
			get
			{
				return (string)base.GetValue(HMIExtendedNumericDisplay1.ServerNameProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.ServerNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMIExtendedNumericDisplay1.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMIExtendedNumericDisplay1.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.SignalPrefixProperty, value);
			}
		}
		[Category("HMI")]
		public string Unit1
		{
			get
			{
				return (string)base.GetValue(HMIExtendedNumericDisplay1.Unit1Property);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.Unit1Property, value);
			}
		}
		[Category("HMI")]
		public string Unit2
		{
			get
			{
				return (string)base.GetValue(HMIExtendedNumericDisplay1.Unit2Property);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.Unit2Property, value);
			}
		}
		[Category("HMI")]
		public double Unit1ScaleFactor
		{
			get
			{
				return (double)base.GetValue(HMIExtendedNumericDisplay1.Unit1ScaleFactorProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.Unit1ScaleFactorProperty, value);
			}
		}
		[Category("HMI")]
		public double Unit2ScaleFactor
		{
			get
			{
				return (double)base.GetValue(HMIExtendedNumericDisplay1.Unit2ScaleFactorProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.Unit2ScaleFactorProperty, value);
			}
		}
		[Category("HMI")]
		public string UnitSignalName
		{
			get
			{
				return (string)base.GetValue(HMIExtendedNumericDisplay1.UnitSignalNameProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.UnitSignalNameProperty, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? UnitSignalNameMask
		{
			get
			{
				return (int?)base.GetValue(HMIExtendedNumericDisplay1.UnitSignalNameMaskProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.UnitSignalNameMaskProperty, value);
			}
		}
		[Category("HMI"), TypeConverter(typeof(NullableInt32TypeConverter))]
		public int? VisibilityMask
		{
			get
			{
				return (int?)base.GetValue(HMIExtendedNumericDisplay1.VisibilityMaskProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.VisibilityMaskProperty, value);
			}
		}
		[Category("HMI")]
		public string VisibilitySignalName
		{
			get
			{
				return (string)base.GetValue(HMIExtendedNumericDisplay1.VisibilitySignalNameProperty);
			}
			set
			{
				base.SetValue(HMIExtendedNumericDisplay1.VisibilitySignalNameProperty, value);
			}
		}
		private static void OnObjectIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIExtendedNumericDisplay1? wFExtendedNumericDisplay = d as HMIExtendedNumericDisplay1;
			if (wFExtendedNumericDisplay != null && DesignerProperties.GetIsInDesignMode(wFExtendedNumericDisplay))
			{
				wFExtendedNumericDisplay.UpdateOutputInDesignMode();
			}
		}
		private static void OnPrecisionPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIExtendedNumericDisplay1? wFExtendedNumericDisplay = d as HMIExtendedNumericDisplay1;
			if (wFExtendedNumericDisplay != null && (int)e.NewValue < 0)
			{
				wFExtendedNumericDisplay.Precision = (int)e.OldValue;
			}
		}
		private static void OnSecurityAuthorizationNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityDenyAccessBehaviourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnServerNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIExtendedNumericDisplay1? wFExtendedNumericDisplay = d as HMIExtendedNumericDisplay1;
			if (wFExtendedNumericDisplay != null && DesignerProperties.GetIsInDesignMode(wFExtendedNumericDisplay))
			{
				wFExtendedNumericDisplay.UpdateOutputInDesignMode();
			}
		}
		private static void OnSignalPrefixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIExtendedNumericDisplay1? wFExtendedNumericDisplay = d as HMIExtendedNumericDisplay1;
			if (wFExtendedNumericDisplay != null && DesignerProperties.GetIsInDesignMode(wFExtendedNumericDisplay))
			{
				wFExtendedNumericDisplay.UpdateOutputInDesignMode();
			}
		}
		private static void OnUnit1PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnUnit2PropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnUnit1ScaleFactorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnUnit2ScaleFactorPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnUnitSignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnUnitSignalNameMaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnVisibilityMaskPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnVisibilitySignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMIExtendedNumericDisplay1()
		{
			base.DefaultStyleKey=(typeof(HMIExtendedNumericDisplay1));
			this.signalValue = double.NaN;
			this.unitSignalValue = double.NaN;
		}
		~HMIExtendedNumericDisplay1()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void OnSignalValueChangedHandler(string signalName, object signalValue)
		{
			this.signalValue = this.GetSignalValue(signalValue);
			this.UpdateOutput();
		}
		private void OnUnitSignalValueChangedHandler(string signalName, object signalValue)
		{
			this.unitSignalValue = this.GetSignalValue(signalValue);
			this.UpdateOutput();
		}
		private void OnVisibilitySignalValueChangedHandler(string signalName, object signalValue)
		{
			double visibilitySignalValue = this.GetSignalValue(signalValue);
			this.canShowControlFromVisibilitySignal =
				!double.IsNaN(visibilitySignalValue) &&
				visibilitySignalValue != 0.0 &&
				(!this.VisibilityMask.HasValue ||
				 (Convert.ToInt64(visibilitySignalValue) & (long)this.VisibilityMask.Value) != (long)this.VisibilityMask.Value);
			this.ShowControl(this.canShowControlFromVisibilitySignal);
		}
		private void connector_UserContextChanged(string userName)
		{
			if (this.isSecurityAvailable)
			{
				this.isAuthorized = this.connector.CheckProjectAuthorization(this.SecurityAuthorizationName);
				switch (this.SecurityDenyAccessBehaviour)
				{
				case DenyAccessBehaviour.Disable:
					this.EnableControl(this.isAuthorized);
					break;
				case DenyAccessBehaviour.Hide:
					this.ShowControl(this.isAuthorized);
					break;
				default:
					throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
				}
			}
			this.OnUserContextChanged(userName);
		}
		public override void OnApplyTemplate()
		{
			this.tbOutputElement = null;
			base.OnApplyTemplate();
			this.tbOutputElement = (base.GetTemplateChild("OutputElement") as TextBlock);
			base.IsTabStop=(false);
			bool isInDesignMode;
			if (isInDesignMode = DesignerProperties.GetIsInDesignMode(this))
			{
				this.UpdateOutputInDesignMode();
				return;
			}
			if (this.connector != null)
			{
				return;
			}
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();

            if (this.tbOutputElement != null)
			{
				this.tbOutputElement.Text=(string.Empty);
			}
			if (string.IsNullOrEmpty(this.SecurityAuthorizationName))
			{
				this.isAuthorized = true;
				this.isSecurityAvailable = false;
			}
			else
			{
				this.isAuthorized = false;
				this.isSecurityAvailable = true;
				switch (this.SecurityDenyAccessBehaviour)
				{
				case DenyAccessBehaviour.Disable:
					base.IsEnabled=(false);
					break;
				case DenyAccessBehaviour.Hide:
					base.Visibility= System.Windows.Visibility.Collapsed;
					break;
				default:
					throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
				}
			}
			this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
			if (!string.IsNullOrEmpty(this.SignalName))
			{
				this.SignalName = this.SignalPrefix + this.SignalName;
				this.SignalName = SignalNameBuilder.BuildSignalName(this.SignalName, this);
				this.delegateSignalValueChanged = new OnSignalChangedDelegate(this.OnSignalValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.SignalName, this.delegateSignalValueChanged);
			}
			if (!string.IsNullOrEmpty(this.UnitSignalName))
			{
				this.UnitSignalName = this.SignalPrefix + this.UnitSignalName;
				this.UnitSignalName = SignalNameBuilder.BuildSignalName(this.UnitSignalName, this);
				this.delegateUnitSignalValueChanged = new OnSignalChangedDelegate(this.OnUnitSignalValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.UnitSignalName, this.delegateUnitSignalValueChanged);
			}
			if (!string.IsNullOrEmpty(this.VisibilitySignalName))
			{
				this.VisibilitySignalName = this.SignalPrefix + this.VisibilitySignalName;
				this.VisibilitySignalName = SignalNameBuilder.BuildSignalName(this.VisibilitySignalName, this);
				this.delegateVisibilitySignalValueChanged = new OnSignalChangedDelegate(this.OnVisibilitySignalValueChangedHandler);
				this.RegisterSignal(this.ServerName, this.VisibilitySignalName, this.delegateVisibilitySignalValueChanged);
			}
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.connector != null)
				{
					this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
					if (this.delegateSignalValueChanged != null)
					{
						this.UnregisterSignal(this.ServerName, this.SignalName, this.delegateSignalValueChanged);
					}
					if (this.delegateUnitSignalValueChanged != null)
					{
						this.UnregisterSignal(this.ServerName, this.UnitSignalName, this.delegateUnitSignalValueChanged);
					}
					if (this.delegateVisibilitySignalValueChanged != null)
					{
						this.UnregisterSignal(this.ServerName, this.VisibilitySignalName, this.delegateVisibilitySignalValueChanged);
					}
					this.connector.Dispose();
				}
				this.StopStoryboards();
			}
		}
		private void OnUserContextChanged(string userName)
		{
		}
		private void EnableControl(bool enable)
		{
			if (!enable)
			{
				base.IsEnabled=(false);
				return;
			}
			if (this.isSecurityAvailable && !this.isAuthorized && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Disable)
			{
				base.IsEnabled=(false);
				return;
			}
			base.IsEnabled=(true);
		}
		private void ShowControl(bool show)
		{
			if (!show)
			{
				base.Visibility= System.Windows.Visibility.Collapsed;
				return;
			}
			if (this.isSecurityAvailable && !this.isAuthorized && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Hide)
			{
				base.Visibility= System.Windows.Visibility.Collapsed;
				return;
			}
			if (string.IsNullOrEmpty(this.VisibilitySignalName))
			{
				base.Visibility= System.Windows.Visibility.Visible;
				return;
			}
			if (this.canShowControlFromVisibilitySignal)
			{
				base.Visibility= System.Windows.Visibility.Visible;
				return;
			}
			base.Visibility= System.Windows.Visibility.Collapsed;
		}
		private double GetSignalValue(object value)
		{
			double result = double.NaN;
			try
			{
				result = double.Parse(value.ToString());
			}
			catch
			{
				result = double.NaN;
			}
			return result;
		}
		private string ConvertSignalValueToStringForUnit1()
		{
			string text = string.Empty;
			text += (this.signalValue * this.Unit1ScaleFactor).ToString("F" + this.Precision);
			if (!string.IsNullOrEmpty(this.Unit1))
			{
				text = text + " " + this.Unit1;
			}
			return text;
		}
		private string ConvertSignalValueToStringForUnit2()
		{
			string text = string.Empty;
			text += (this.signalValue * this.Unit2ScaleFactor).ToString("F" + this.Precision);
			if (!string.IsNullOrEmpty(this.Unit2))
			{
				text = text + " " + this.Unit2;
			}
			return text;
		}
		private void UpdateOutput()
		{
			if (this.tbOutputElement != null)
			{
				try
				{
					string text = string.Empty;
					if (double.IsNaN(this.signalValue) || double.IsNaN(this.unitSignalValue) || this.unitSignalValue == 0.0)
					{
						text = this.ConvertSignalValueToStringForUnit1();
					}
					else if (!this.UnitSignalNameMask.HasValue)
					{
						text = this.ConvertSignalValueToStringForUnit2();
					}
					else
					{
						long signalBitmask = Convert.ToInt64(this.signalValue);
						long unitSignalMask = (long)this.UnitSignalNameMask.Value;
						text = (signalBitmask & unitSignalMask) == unitSignalMask
							? this.ConvertSignalValueToStringForUnit1()
							: this.ConvertSignalValueToStringForUnit2();
					}
					this.tbOutputElement.Text=(text);
				}
				catch (Exception ex)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void UpdateOutputInDesignMode()
		{
			if (this.tbOutputElement != null)
			{
				string text = string.Empty;
				if (!string.IsNullOrEmpty(this.SignalName))
				{
					text = this.SignalName;
					text = this.SignalPrefix + text;
					text = SignalNameBuilder.BuildSignalName(text, this);
				}
				this.tbOutputElement.Text=(text);
			}
		}
		private void RegisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
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
		private void UnregisterSignal(string serverName, string signalName, OnSignalChangedDelegate signalChangedDelegate)
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
	}
}
