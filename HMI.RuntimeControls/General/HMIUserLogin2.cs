// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using HMI.Runtime.Core;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.General
{
    /// <summary>




    /// </summary>
    public class HMIUserLogin2 : Control, IDisposable
	{
		public enum UserPrefixType
		{
			None,
			HostName,
			DomainName
		}
		private string userName = string.Empty;
		private string windowsUser;
		private string clientHost;
		private string domainName;
		private DispatcherTimer timer;
		private bool ignoreUserContextChanged;
		private IHMIConnector connector;

		public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register("Password", typeof(string), typeof(HMIUserLogin2), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIUserLogin2.OnPasswordPropertyChanged)));
		public static readonly DependencyProperty UserPrefixProperty = DependencyProperty.Register("UserPrefix", typeof(HMIUserLogin2.UserPrefixType), typeof(HMIUserLogin2), new PropertyMetadata(HMIUserLogin2.UserPrefixType.None, new PropertyChangedCallback(HMIUserLogin2.OnUserPrefixPropertyChanged)));
		
		public bool IsInDesignMode
		{
			get
			{
				return DesignerProperties.GetIsInDesignMode(this);
			}
		}
		[Category("HMI")]
		public string Password
		{
			get
			{
				return (string)base.GetValue(HMIUserLogin2.PasswordProperty);
			}
			set
			{
				base.SetValue(HMIUserLogin2.PasswordProperty, value);
			}
		}
		[Category("HMI")]
		public HMIUserLogin2.UserPrefixType UserPrefix
		{
			get
			{
				return (HMIUserLogin2.UserPrefixType)base.GetValue(HMIUserLogin2.UserPrefixProperty);
			}
			set
			{
				base.SetValue(HMIUserLogin2.UserPrefixProperty, value);
			}
		}
		public HMIUserLogin2()
		{
			base.DefaultStyleKey=(typeof(HMIUserLogin2));
			this.timer = new DispatcherTimer();
			this.timer.Interval=(TimeSpan.FromSeconds(30.0));
			this.timer.Tick+=(new EventHandler(this.timer_Tick));
		}
		~HMIUserLogin2()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.timer != null)
				{
					this.timer.Stop();
					this.timer.Tick-=(new EventHandler(this.timer_Tick));
				}
				if (this.connector != null)
				{
					this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
					this.connector.Dispose();
				}
				this.StopStoryboards();
			}
		}
		private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnUserPrefixPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private void connector_UserContextChanged(string userName)
		{
			if (this.ignoreUserContextChanged)
			{
				return;
			}
			if (string.CompareOrdinal(this.userName, userName) != 0)
			{
				this.userName = userName;
				this.CheckAuthentication();
			}
		}
		private void timer_Tick(object sender, EventArgs e)
		{
			this.timer.Stop();
			base.Dispatcher.BeginInvoke(delegate
			{
				this.CheckAuthentication();
			});
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (!this.IsInDesignMode)
			{
				this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			}
			if (this.connector != null)
			{
				try
				{
					this.userName = this.connector.GetUserName();
					if (this.userName == null)
					{
						this.userName = string.Empty;
					}
					this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
				}
				catch (Exception ex)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void OnLoginResult(Exception error, int[] results)
		{
			if (error == null && results != null && results.Length == 1 && results[0] == 0)
			{
				if (this.connector != null)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Hint, string.Format("{0} Login success.", base.GetType().Name), new object[0]);
				}
			}
			else
			{
				if (this.connector != null)
				{
					if (error != null)
					{
						this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} Login failed: {1}", base.GetType().Name, error.Message), new object[0]);
					}
					else
					{
						this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} Login failed, error {1}", base.GetType().Name, results[0]), new object[0]);
					}
				}
				this.StartTimer();
			}
			this.ignoreUserContextChanged = false;
		}
		private void CheckAuthentication()
		{
			if (string.IsNullOrEmpty(this.windowsUser))
			{
				if (this.connector != null)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} Could not get the windows user name. Automatic login aborted.", base.GetType().Name), new object[0]);
				}
				return;
			}
			string text = this.windowsUser;
			switch (this.UserPrefix)
			{
			case HMIUserLogin2.UserPrefixType.HostName:
				text = string.Format("{0}\\{1}", this.clientHost, this.windowsUser);
				break;
			case HMIUserLogin2.UserPrefixType.DomainName:
				text = string.Format("{0}\\{1}", this.domainName, this.windowsUser);
				break;
			}
			if (this.connector != null && string.CompareOrdinal(text, this.userName) != 0)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Hint, string.Format("{0} Try to login as \"{1}\"", base.GetType().Name, text), new object[0]);
				try
				{
					this.ignoreUserContextChanged = true;
					this.connector.LoginUser(text, this.Password, false, new OnResultDelegate(this.OnLoginResult));
				}
				catch (Exception ex)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void StartTimer()
		{
			this.timer.Start();
		}
	}
}
