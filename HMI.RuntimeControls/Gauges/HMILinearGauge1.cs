// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.Gauges
{
	public abstract class HMILinearGauge1 : Control, IDisposable
	{
		private IHMIConnector connector;
		private bool authorizationEnabled;
		private bool isAuthorized;
		private OnSignalChangedDelegate signalChangedHandler;

		public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(HMILinearGauge1), new PropertyMetadata(100.0, new PropertyChangedCallback(HMILinearGauge1.OnMaximumChanged)));
		public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(HMILinearGauge1), new PropertyMetadata(0.0, new PropertyChangedCallback(HMILinearGauge1.OnMinimumChanged)));
		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMILinearGauge1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMILinearGauge1.OnObjectIdChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMILinearGauge1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMILinearGauge1.OnSecurityAuthorizationNameChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMILinearGauge1), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMILinearGauge1.OnSecurityDenyAccessBehaviourChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMILinearGauge1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMILinearGauge1.OnServerNameChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMILinearGauge1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMILinearGauge1.OnSignalNameChanged)));
		public static readonly DependencyProperty SignalPrefixProperty = DependencyProperty.Register("SignalPrefix", typeof(string), typeof(HMILinearGauge1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMILinearGauge1.OnSignalPrefixChanged)));
		
		[Category("HMI")]
		public double Maximum
		{
			get
			{
				return (double)base.GetValue(HMILinearGauge1.MaximumProperty);
			}
			set
			{
				base.SetValue(HMILinearGauge1.MaximumProperty, value);
			}
		}
		[Category("HMI")]
		public double Minimum
		{
			get
			{
				return (double)base.GetValue(HMILinearGauge1.MinimumProperty);
			}
			set
			{
				base.SetValue(HMILinearGauge1.MinimumProperty, value);
			}
		}
		[Category("HMI")]
		public string ObjectID
		{
			get
			{
				return (string)base.GetValue(HMILinearGauge1.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMILinearGauge1.ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMILinearGauge1.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMILinearGauge1.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMILinearGauge1.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMILinearGauge1.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMILinearGauge1.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMILinearGauge1.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalPrefix
		{
			get
			{
				return (string)base.GetValue(HMILinearGauge1.SignalPrefixProperty);
			}
			set
			{
				base.SetValue(HMILinearGauge1.SignalPrefixProperty, value);
			}
		}
		private static void OnMaximumChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnMinimumChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnObjectIdChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSecurityAuthorizationNameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSecurityDenyAccessBehaviourChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private string GetServerName()
		{
			return (string)base.GetValue(HMILinearGauge1.ServerNameProperty);
		}
		private static void OnServerNameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSignalNameChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		private static void OnSignalPrefixChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs change)
		{
		}
		public HMILinearGauge1()
		{
			base.DefaultStyleKey=(typeof(HMILinearGauge1));
		}
		~HMILinearGauge1()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void HandleUserContextChanged(string userContext)
		{
			if (this.authorizationEnabled)
			{
				this.isAuthorized = this.connector.CheckProjectAuthorization(this.SecurityAuthorizationName);
				switch (this.SecurityDenyAccessBehaviour)
				{
				case DenyAccessBehaviour.Disable:
					this.ApplyAuthorizationEnabledState(this.isAuthorized);
					break;
				case DenyAccessBehaviour.Hide:
					this.ApplyAuthorizationVisibility(this.isAuthorized);
					break;
				default:
					throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
				}
			}
			this.RefreshSignalBinding(userContext);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.connector != null)
				{
					this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.HandleUserContextChanged);
					if (this.signalChangedHandler != null)
					{
						this.UnregisterSignalChangedHandler(this.GetServerName(), this.SignalName, this.signalChangedHandler);
					}
					this.connector.Dispose();
				}
				this.StopStoryboards();
			}
		}
		private void RefreshSignalBinding(string userContext)
		{
		}
		private void ApplyAuthorizationEnabledState(bool isAuthorized)
		{
			if (!isAuthorized)
			{
				base.IsEnabled=(false);
				return;
			}
			if (this.authorizationEnabled && !this.isAuthorized && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Disable)
			{
				base.IsEnabled=(false);
				return;
			}
			base.IsEnabled=(true);
		}
		private void ApplyAuthorizationVisibility(bool isAuthorized)
		{
			if (!isAuthorized)
			{
				base.Visibility= System.Windows.Visibility.Collapsed;
				return;
			}
			if (this.authorizationEnabled && !this.isAuthorized && this.SecurityDenyAccessBehaviour == DenyAccessBehaviour.Hide)
			{
				base.Visibility= System.Windows.Visibility.Collapsed;
				return;
			}
			base.Visibility= System.Windows.Visibility.Visible;
		}
		private void UnregisterSignalChangedHandler(string serverName, string signalName, OnSignalChangedDelegate signalChangedHandler)
		{
			try
			{
				this.connector.UnregisterSignalChangedHandler(serverName, signalName, signalChangedHandler);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
	}
}
