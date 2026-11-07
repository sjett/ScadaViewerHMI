// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.General
{
	public class HMITimeline1 : Control, IDisposable
	{
		private IHMIConnector connector;
		private Storyboard? storyboard;
		private bool isSecurityAvailable;
		private bool isAuthorized;
		private OnSignalChangedDelegate delegateSignalValueChanged;

		public static readonly DependencyProperty ObjectIDProperty = DependencyProperty.Register("ObjectID", typeof(string), typeof(HMITimeline1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITimeline1.OnObjectIDPropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMITimeline1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITimeline1.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMITimeline1), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMITimeline1.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty ServerNameProperty = DependencyProperty.Register("ServerName", typeof(string), typeof(HMITimeline1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITimeline1.OnServerNamePropertyChanged)));
		public static readonly DependencyProperty SignalNameProperty = DependencyProperty.Register("SignalName", typeof(string), typeof(HMITimeline1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITimeline1.OnSignalNamePropertyChanged)));
		public static readonly DependencyProperty SignalValueStartStoryboardProperty = DependencyProperty.Register("SignalValueStartStoryboard", typeof(double), typeof(HMITimeline1), new PropertyMetadata(0.0, new PropertyChangedCallback(HMITimeline1.OnSignalValueStartStoryboardPropertyChanged)));
		public static readonly DependencyProperty StoryboardNameProperty = DependencyProperty.Register("StoryboardName", typeof(string), typeof(HMITimeline1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMITimeline1.OnStoryboardNamePropertyChanged)));
		
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
				return (string)base.GetValue(HMITimeline1.ObjectIDProperty);
			}
			set
			{
				base.SetValue(HMITimeline1.ObjectIDProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMITimeline1.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMITimeline1.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMITimeline1.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMITimeline1.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		private string ServerName
		{
			get
			{
				return (string)base.GetValue(HMITimeline1.ServerNameProperty);
			}
			set
			{
				base.SetValue(HMITimeline1.ServerNameProperty, value);
			}
		}
		[Category("HMI")]
		public string SignalName
		{
			get
			{
				return (string)base.GetValue(HMITimeline1.SignalNameProperty);
			}
			set
			{
				base.SetValue(HMITimeline1.SignalNameProperty, value);
			}
		}
		[Category("HMI")]
		public double SignalValueStartStoryboard
		{
			get
			{
				return (double)base.GetValue(HMITimeline1.SignalValueStartStoryboardProperty);
			}
			set
			{
				base.SetValue(HMITimeline1.SignalValueStartStoryboardProperty, value);
			}
		}
		[Category("HMI")]
		public string StoryboardName
		{
			get
			{
				return (string)base.GetValue(HMITimeline1.StoryboardNameProperty);
			}
			set
			{
				base.SetValue(HMITimeline1.StoryboardNameProperty, value);
			}
		}
		private static void OnObjectIDPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
		private static void OnSignalNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSignalValueStartStoryboardPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnStoryboardNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMITimeline1()
		{
			base.DefaultStyleKey=(typeof(HMITimeline1));
		}
		~HMITimeline1()
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
			if (this.storyboard == null)
			{
				return;
			}
			if (this.GetSignalValue(signalValue).Equals(this.SignalValueStartStoryboard))
			{
				this.storyboard.Begin();
				return;
			}
			this.storyboard.Stop();
		}
        /// <summary>

        /// </summary>

        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
				if (!string.IsNullOrEmpty(this.StoryboardName))
				{
					this.storyboard = HMITimeline1.FindStoryboard(Application.Current.MainWindow, this.StoryboardName);
					if (this.storyboard == null)
					{
						this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, "The storyboard '" + this.StoryboardName + "' is not defined"), new object[0]);
					}
				}
				if (!string.IsNullOrEmpty(this.SignalName))
				{
					this.SignalName = SignalNameBuilder.BuildSignalName(this.SignalName, this);
					this.delegateSignalValueChanged = new OnSignalChangedDelegate(this.OnSignalValueChangedHandler);
					this.RegisterSignal(this.ServerName, this.SignalName, this.delegateSignalValueChanged);
				}
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
		private static Storyboard? FindStoryboard(DependencyObject source, string storyboardName)
		{
			Storyboard? storyboard = source as Storyboard;
			if (storyboard == null)
			{
				FrameworkElement? frameworkElement = source as FrameworkElement;
				storyboard = (frameworkElement?.Resources[storyboardName] as Storyboard);
				if (storyboard == null)
				{
					int childrenCount = VisualTreeHelper.GetChildrenCount(source);
					if (childrenCount == 0)
					{
						return null;
					}
					for (int i = 0; i < childrenCount; i++)
					{
						storyboard = HMITimeline1.FindStoryboard(VisualTreeHelper.GetChild(source, i), storyboardName);
						if (storyboard != null)
						{
							break;
						}
					}
				}
			}
			return storyboard;
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
