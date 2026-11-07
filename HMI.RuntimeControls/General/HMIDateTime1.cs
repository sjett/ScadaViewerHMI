// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.General
{
	[TemplatePart(Name = "OutputElement", Type = typeof(TextBlock))]
	public class HMIDateTime1 : Control, IDisposable
	{
		public enum DateTimeSource
		{
			Client,
			Server,
			ServerToClient
		}
		private const string ElementOutput = "OutputElement";

		private const string HMISInternalAliveTimeStamp = "HMISInternal_AliveTimeStamp";
		private const string HMISInternalLocalTimeStamp = "HMISInternal_LocalTimeStamp";

		private const string SNConnectionLost = "HMISN_Ctrl_HMIDateTime1_ConnectionLost";

		private TextBlock? tbOutputElement;
		private IHMIConnector connector;
		private bool isSecurityAvailable;
		private bool isAuthorized;

		private Dictionary<string, SymbolicTextTranslation> dictTranslations;
		private DispatcherTimer dispatcherTimerClient;
		private DispatcherTimer dispatcherTimerConnection;
		private OnSignalChangedDelegate delegateServerTimeSignalValueChanged;
		private OnSignalChangedDelegate delegateServerUTCTimeSignalValueChanged;
		private OnLanguageChangedDelegate delegateSymbolicTextTranslationsChanged;

		private DateTime dateTimeBackUpCurrent;
		private DateTime dateTimeBackUpOld;

		public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(string), typeof(HMIDateTime1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDateTime1.OnFormatPropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIDateTime1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDateTime1.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIDateTime1), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMIDateTime1.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMIDateTime1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIDateTime1.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty SourceProperty = DependencyProperty.Register("Source", typeof(HMIDateTime1.DateTimeSource), typeof(HMIDateTime1), new PropertyMetadata(HMIDateTime1.DateTimeSource.Client, new PropertyChangedCallback(HMIDateTime1.OnSourcePropertyChanged)));
		
		private IHMIConnector Connector
		{
			get
			{
				return this.connector;
			}
		}

		[Category("HMI")]
		public string Format
		{
			get
			{
				return (string)base.GetValue(HMIDateTime1.FormatProperty);
			}
			set
			{
				base.SetValue(HMIDateTime1.FormatProperty, value);
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
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIDateTime1.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIDateTime1.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIDateTime1.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIDateTime1.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		public string ServerName
		{
			get
			{
				return (string)base.GetValue(HMIDateTime1.ServerNameProperty);
			}
			set
			{
				base.SetValue(HMIDateTime1.ServerNameProperty, value);
			}
		}
		[Category("HMI")]
		public HMIDateTime1.DateTimeSource Source
		{
			get
			{
				return (HMIDateTime1.DateTimeSource)base.GetValue(HMIDateTime1.SourceProperty);
			}
			set
			{
				base.SetValue(HMIDateTime1.SourceProperty, value);
			}
		}
		private static void OnFormatPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
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
		private static void OnSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMIDateTime1()
		{
			base.DefaultStyleKey=(typeof(HMIDateTime1));
			this.dateTimeBackUpCurrent = (this.dateTimeBackUpOld = DateTime.Now);
			this.dictTranslations = new Dictionary<string, SymbolicTextTranslation>(StringComparer.InvariantCultureIgnoreCase);
		}
		~HMIDateTime1()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void OnServerTimeSignalValueChangedHandler(string signalName, object signalValue)
		{
			try
			{
				DateTime output = DateTime.Parse(signalValue.ToString());
				this.dateTimeBackUpCurrent = output;
				this.SetOutput(output);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void OnServerUTCTimeSignalValueChangedHandler(string signalName, object signalValue)
		{
			try
			{
				DateTime output = DateTime.Parse(signalValue.ToString()).ToLocalTime();
				this.dateTimeBackUpCurrent = output;
				this.SetOutput(output);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
		{
			try
			{
				for (int i = 0; i < symbolicTexts.Length; i++)
				{
					SymbolicTextTranslation symbolicTextTranslation = this.dictTranslations[symbolicTexts[i]];
                    if (translationTexts[i] != null && !translationTexts[i].Equals("!null!", StringComparison.InvariantCultureIgnoreCase))
					{
						symbolicTextTranslation.Translation = translationTexts[i];
					}
					else
					{
						symbolicTextTranslation.Translation = symbolicTextTranslation.SymbolicText;
					}
				}
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
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
		private void dispatcherTimerClient_Tick(object sender, EventArgs e)
		{
			this.SetOutput(DateTime.Now);
		}
		private void dispatcherTimerConnection_Tick(object sender, EventArgs e)
		{
			if (this.dateTimeBackUpCurrent.Equals(this.dateTimeBackUpOld))
			{
				SymbolicTextTranslation symbolicTextTranslation = this.dictTranslations["HMISN_Ctrl_HMIDateTime1_ConnectionLost"];
				if (symbolicTextTranslation != null)
				{
					this.SetOutput(symbolicTextTranslation.Translation);
				}
			}
			this.dateTimeBackUpOld = this.dateTimeBackUpCurrent;
		}
		public override void OnApplyTemplate()
		{
			this.tbOutputElement = null;
			base.OnApplyTemplate();

			base.IsTabStop = (false);
			this.tbOutputElement = (base.GetTemplateChild("OutputElement") as TextBlock);

			bool isInDesignMode;
			if (isInDesignMode = DesignerProperties.GetIsInDesignMode(this))
			{
				return;
			}
			if (this.connector != null)
			{
				return;
			}
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			if (this.tbOutputElement != null)
			{
				this.tbOutputElement.Text = (string.Empty);
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
						base.IsEnabled = (false);
						break;
					case DenyAccessBehaviour.Hide:
						base.Visibility = System.Windows.Visibility.Collapsed;
						break;
					default:
						throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
				}
			}
			if (this.Source == HMIDateTime1.DateTimeSource.Server || this.Source == HMIDateTime1.DateTimeSource.ServerToClient)
			{
				SymbolicTextTranslation symbolicTextTranslation = new SymbolicTextTranslation("HMISN_Ctrl_HMIDateTime1_ConnectionLost");
				this.dictTranslations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
				try
				{
					string[] array = new string[this.dictTranslations.Count];
					this.dictTranslations.Keys.CopyTo(array, 0);
					this.delegateSymbolicTextTranslationsChanged = new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler);
					this.connector.RegisterLanguageChangedHandler(array, this.delegateSymbolicTextTranslationsChanged);
				}
				catch (Exception ex)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
			this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
			switch (this.Source)
			{
				case HMIDateTime1.DateTimeSource.Client:
					this.dispatcherTimerClient = new DispatcherTimer();
					this.dispatcherTimerClient.Interval = (TimeSpan.FromSeconds(1.0));
					this.dispatcherTimerClient.Tick += (new EventHandler(this.dispatcherTimerClient_Tick));
					this.dispatcherTimerClient.Start();
					break;
				case HMIDateTime1.DateTimeSource.Server:
					this.delegateServerTimeSignalValueChanged = new OnSignalChangedDelegate(this.OnServerTimeSignalValueChangedHandler);
					this.RegisterSignal(this.ServerName, "HMISInternal_LocalTimeStamp", this.delegateServerTimeSignalValueChanged);
					break;
				case HMIDateTime1.DateTimeSource.ServerToClient:
					this.delegateServerUTCTimeSignalValueChanged = new OnSignalChangedDelegate(this.OnServerUTCTimeSignalValueChangedHandler);
					this.RegisterSignal(this.ServerName, "HMISInternal_AliveTimeStamp", this.delegateServerUTCTimeSignalValueChanged);
					break;
				default:
					throw new ArgumentOutOfRangeException("DateTimeSource");
			}
			if (this.Source == HMIDateTime1.DateTimeSource.Server || this.Source == HMIDateTime1.DateTimeSource.ServerToClient)
			{
				this.dispatcherTimerConnection = new DispatcherTimer();
				this.dispatcherTimerConnection.Interval = (TimeSpan.FromSeconds(5.0));
				this.dispatcherTimerConnection.Tick += (new EventHandler(this.dispatcherTimerConnection_Tick));
				this.dispatcherTimerConnection.Start();
			}
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.dispatcherTimerClient != null && this.dispatcherTimerClient.IsEnabled)
				{
					this.dispatcherTimerClient.Stop();
				}
				if (this.dispatcherTimerConnection != null && this.dispatcherTimerConnection.IsEnabled)
				{
					this.dispatcherTimerConnection.Stop();
				}
				if (this.connector != null)
				{
					this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
					if (this.delegateServerTimeSignalValueChanged != null)
					{
						this.UnregisterSignal(this.ServerName, "HMISInternal_LocalTimeStamp", this.delegateServerTimeSignalValueChanged);
					}
					if (this.delegateServerUTCTimeSignalValueChanged != null)
					{
						this.UnregisterSignal(this.ServerName, "HMISInternal_AliveTimeStamp", this.delegateServerUTCTimeSignalValueChanged);
					}
					if (this.delegateSymbolicTextTranslationsChanged != null)
					{
						this.connector.UnregisterLanguageChangedHandler(null, this.delegateSymbolicTextTranslationsChanged);
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
		private void SetOutput(DateTime dateTime)
		{
			if (this.tbOutputElement == null)
			{
				return;
			}
			if (string.IsNullOrEmpty(this.Format))
			{
				this.tbOutputElement.Text=(dateTime.ToString(DateTimeFormatInfo.CurrentInfo));
				return;
			}
			this.tbOutputElement.Text=(dateTime.ToString(this.Format, DateTimeFormatInfo.CurrentInfo));
		}
		private void SetOutput(string output)
		{
			if (this.tbOutputElement == null)
			{
				return;
			}
			this.tbOutputElement.Text=(output);
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
