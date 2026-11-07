// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;

namespace HMI.RuntimeControls.Gauges
{
    /// <summary>

    /// </summary>
    [TemplatePart(Name = "HandleElement", Type = typeof(Rectangle))]
	public class HMIGauge2 : Control, IDisposable
	{
		private IHMIConnector connector;
		private Rectangle? handleElement;
		private double anglePercentValue;
		private double angleValue;
		private bool isSecurityAvailable;
		private bool isAuthorized;
		private OnSignalChangedDelegate signalChangedHandler;

		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIGauge2), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIGauge2.OnObjectIdChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIGauge2), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIGauge2.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIGauge2), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMIGauge2.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIGauge2), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIGauge2.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIGauge2), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIGauge2.OnSignalNamePropertyChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIGauge2), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIGauge2.OnSignalPrefixPropertyChanged)));
		
		[Category("HMI")]
		public string ObjectID
		{
			get
			{
				return (string)base.GetValue(HMIGauge2.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMIGauge2.ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIGauge2.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIGauge2.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIGauge2.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIGauge2.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMIGauge2.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMIGauge2.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMIGauge2.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMIGauge2.SignalPrefixProperty, value);
			}
		}
		private static void OnObjectIdChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSecurityAuthorizationNamePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSecurityDenyAccessBehaviourPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private string GetServerName()
		{
			return (string)base.GetValue(HMIGauge2.ServerNameProperty);
		}
		private static void OnServerNamePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSignalNamePropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSignalPrefixPropertyChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		public HMIGauge2()
		{
			base.DefaultStyleKey=(typeof(HMIGauge2));
			this.anglePercentValue = 0.0;
		}
		~HMIGauge2()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void OnSignalValueChanged(string signalName, object signalValue)
		{
			this.anglePercentValue = this.ConvertSignalValueToPercent(signalValue);
			if (!double.IsNaN(this.anglePercentValue))
			{
				this.angleValue = this.anglePercentValue / 100.0 * 90.0;
			}
			else
			{
				this.angleValue = 0.0;
			}
			this.RotateHandlerElement();
		}
		private void HandleAuthorizationContextChanged(string userName)
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
			this.handleElement = null;
			base.OnApplyTemplate();
			base.IsTabStop=(false);
			this.handleElement = (base.GetTemplateChild("HandleElement") as Rectangle);
			bool isInDesignMode;
			if (!(isInDesignMode = DesignerProperties.GetIsInDesignMode(this)))
			{
				if (this.connector != null)
				{
					return;
				}
				this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
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
				this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.HandleAuthorizationContextChanged);
				if (!string.IsNullOrEmpty(this.SignalName))
				{
					this.SignalName = this.SignalPrefix + this.SignalName;
					this.SignalName = SignalNameBuilder.BuildSignalName(this.SignalName, this);
					this.signalChangedHandler = new OnSignalChangedDelegate(this.OnSignalValueChanged);
					this.RegisterSignal(this.GetServerName(), this.SignalName, this.signalChangedHandler);
				}
			}
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.connector != null)
				{
					this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.HandleAuthorizationContextChanged);
					if (this.signalChangedHandler != null)
					{
						this.UnregisterSignal(this.GetServerName(), this.SignalName, this.signalChangedHandler);
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
		private double ConvertSignalValueToPercent(object signalValue)
		{
			double percentage = double.NaN;
			try
			{
				percentage = double.Parse(signalValue.ToString());
				if (percentage < 0.0)
				{
					percentage = 0.0;
				}
				else
				{
					if (percentage > 100.0)
					{
						percentage = 100.0;
					}
				}
			}
			catch
			{
				percentage = 0.0;
			}
			return percentage;
		}
		private void RotateHandlerElement()
		{
			if (this.handleElement != null)
			{
				RotateTransform rotateTransform = new RotateTransform();
				rotateTransform.Angle=(this.angleValue);
				this.handleElement.RenderTransform=(rotateTransform);
			}
		}
		private void RegisterSignal(string serverName, string signalName, OnSignalChangedDelegate onSignalChangedDelegate)
		{
			try
			{
				this.connector.RegisterSignalChangedHandler(serverName, signalName, onSignalChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void UnregisterSignal(string serverName, string signalName, OnSignalChangedDelegate onSignalChangedDelegate)
		{
			try
			{
				this.connector.UnregisterSignalChangedHandler(serverName, signalName, onSignalChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
	}
}
