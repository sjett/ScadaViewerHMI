// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.InputOutput
{
	[TemplatePart(Name = "OutputElement", Type = typeof(TextBlock))]
	public class HMIDateTimeDisplay : Control, IDisposable
	{
		private const string ElementOutput = "OutputElement";

		private TextBlock tbOutputElement;
		private IHMIConnector connector;
		private OnSignalChangedDelegate delegateSignalValueChanged;
		private double signalValue;
		private bool isValidSignalValue;
		private bool isSecurityAvailable;
		private bool isAuthorized;

		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIDateTimeDisplay), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDateTimeDisplay.OnObjectIDPropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIDateTimeDisplay), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDateTimeDisplay.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIDateTimeDisplay), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMIDateTimeDisplay.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIDateTimeDisplay), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDateTimeDisplay.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIDateTimeDisplay), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDateTimeDisplay.OnSignalNamePropertyChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIDateTimeDisplay), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDateTimeDisplay.OnSignalPrefixPropertyChanged)));
		
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
				return (string)base.GetValue(HMIDateTimeDisplay.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMIDateTimeDisplay.ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIDateTimeDisplay.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIDateTimeDisplay.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIDateTimeDisplay.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIDateTimeDisplay.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		private string ServerName
		{
			get
			{
				return (string)base.GetValue(HMIDateTimeDisplay.ServerNameProperty);
			}
			set
			{
				base.SetValue(HMIDateTimeDisplay.ServerNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMIDateTimeDisplay.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMIDateTimeDisplay.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMIDateTimeDisplay.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMIDateTimeDisplay.SignalPrefixProperty, value);
			}
		}
		private static void OnObjectIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIDateTimeDisplay? wFDateTimeDisplay = d as HMIDateTimeDisplay;
			if (wFDateTimeDisplay != null && DesignerProperties.GetIsInDesignMode(wFDateTimeDisplay))
			{
				wFDateTimeDisplay.UpdateOutputInDesignMode();
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
			HMIDateTimeDisplay? wFDateTimeDisplay = d as HMIDateTimeDisplay;
			if (wFDateTimeDisplay != null && DesignerProperties.GetIsInDesignMode(wFDateTimeDisplay))
			{
				wFDateTimeDisplay.UpdateOutputInDesignMode();
			}
		}
		private static void OnSignalPrefixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIDateTimeDisplay? wFDateTimeDisplay = d as HMIDateTimeDisplay;
			if (wFDateTimeDisplay != null && DesignerProperties.GetIsInDesignMode(wFDateTimeDisplay))
			{
				wFDateTimeDisplay.UpdateOutputInDesignMode();
			}
		}
		public HMIDateTimeDisplay()
		{
			base.DefaultStyleKey=(typeof(HMIDateTimeDisplay));
			this.signalValue = 0.0;
		}
		~HMIDateTimeDisplay()
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
			try
			{
				this.signalValue = this.GetSignalValue(signalValue);
				this.isValidSignalValue = true;
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				this.isValidSignalValue = false;
			}
			this.UpdateOutput();
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

			this.tbOutputElement = (base.GetTemplateChild(ElementOutput) as TextBlock);
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
			base.Visibility= System.Windows.Visibility.Visible;
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
		private void UpdateOutput()
		{
			if (this.tbOutputElement != null)
			{
				string text = string.Empty;
				if (this.isValidSignalValue)
				{
					try
					{
						text = DateTime.FromOADate(this.signalValue).ToLocalTime().ToString(DateTimeFormatInfo.CurrentInfo);
					}
					catch (Exception ex)
					{
						this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
					}
				}
				this.tbOutputElement.Text = text;
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
