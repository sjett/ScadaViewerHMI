// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.General
{
    /// <summary>



    /// </summary>
    [
        TemplateVisualState(GroupName = "CommonStates", Name = "Working"), 
		TemplateVisualState(GroupName = "CommonStates", Name = "Error")]
	public class HMIWatchdog1 : Control, IDisposable
	{
		private const string VisualStateGroupCommonStates = "CommonStates";
		private const string VisualStateWorking = "Working";
		private const string VisualStateError = "Error";

		private IHMIConnector connector;
		private DispatcherTimer dispatcherTimer;
		private string signalValue;
		private string signalValueBackup;
		private bool isSecurityAvailable;
		private bool isAuthorized;
		private OnSignalChangedDelegate delegateSignalValueChanged;

		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMIWatchdog1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIWatchdog1.OnObjectIDPropertyChanged)));
		public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register("Period", typeof(int), typeof(HMIWatchdog1), new PropertyMetadata(5, new PropertyChangedCallback(HMIWatchdog1.OnPeriodPropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIWatchdog1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIWatchdog1.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIWatchdog1), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMIWatchdog1.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIWatchdog1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIWatchdog1.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMIWatchdog1), new PropertyMetadata("HMISInternal_AliveTimeStamp", new PropertyChangedCallback(HMIWatchdog1.OnSignalNamePropertyChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMIWatchdog1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIWatchdog1.OnSignalPrefixPropertyChanged)));
		
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
				return (string)base.GetValue(HMIWatchdog1.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMIWatchdog1.ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public int Period
		{
			get
			{
				return (int)base.GetValue(HMIWatchdog1.PeriodProperty);
			}
			set
			{
				base.SetValue(HMIWatchdog1.PeriodProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIWatchdog1.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIWatchdog1.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIWatchdog1.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIWatchdog1.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		private string ServerName
		{
			get
			{
				return (string)base.GetValue(HMIWatchdog1.ServerNameProperty);
			}
			set
			{
				base.SetValue(HMIWatchdog1.ServerNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMIWatchdog1.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMIWatchdog1.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMIWatchdog1.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMIWatchdog1.SignalPrefixProperty, value);
			}
		}
		private static void OnObjectIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnPeriodPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIWatchdog1 wFWatchdog = d as HMIWatchdog1;
			if (wFWatchdog != null)
			{
				int num = (int)e.NewValue;
				if (num <= 0)
				{
					wFWatchdog.Period = (int)e.OldValue;
				}
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
		}
		private static void OnSignalPrefixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMIWatchdog1()
		{
			base.DefaultStyleKey=(typeof(HMIWatchdog1));
			this.signalValue = string.Empty;
			this.signalValueBackup = string.Empty;
		}
		~HMIWatchdog1()
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
			if (signalValue == null)
			{
				this.signalValue = string.Empty;
				return;
			}
			this.signalValue = signalValue.ToString();
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
		private void dispatcherTimer_Tick(object sender, EventArgs e)
		{
			if (this.signalValue.Equals(this.signalValueBackup))
			{
				base.Dispatcher.BeginInvoke(delegate
				{
					VisualStateManager.GoToState(this, "Error", true);
				});
			}
			else
			{
				base.Dispatcher.BeginInvoke(delegate
				{
					VisualStateManager.GoToState(this, "Working", true);
				});
			}
			this.signalValueBackup = this.signalValue;
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
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
				this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
				if (!string.IsNullOrEmpty(this.SignalName))
				{
					this.SignalName = this.SignalPrefix + this.SignalName;
					this.SignalName = SignalNameBuilder.BuildSignalName(this.SignalName, this);
					this.delegateSignalValueChanged = new OnSignalChangedDelegate(this.OnSignalValueChangedHandler);
					this.RegisterSignal(this.ServerName, this.SignalName, this.delegateSignalValueChanged);
				}
				this.dispatcherTimer = new DispatcherTimer();
				this.dispatcherTimer.Interval=(TimeSpan.FromSeconds((double)this.Period));
				this.dispatcherTimer.Tick+=(new EventHandler(this.dispatcherTimer_Tick));
				this.dispatcherTimer.Start();
			}
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.dispatcherTimer != null && this.dispatcherTimer.IsEnabled)
				{
					this.dispatcherTimer.Stop();
				}
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
