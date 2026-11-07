// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;

namespace HMI.RuntimeControls.Navigation
{
    /// <summary>

    /// </summary>
    [TemplatePart(Name = "NavigateButtonElement", Type = typeof(Button))]
	public class HMINavigation : Control, IDisposable
	{
		private bool isSecurityAvailable;
		private bool isAuthorized;

		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMINavigation), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMINavigation.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMINavigation), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMINavigation.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty SymbolicTextProperty = DependencyProperty.Register("SymbolicText", typeof(string), typeof(HMINavigation), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMINavigation.OnSymbolicTextPropertyChanged)));
		
		private IHMIConnector connector;
		private Button? NavigateButtonElement;
		private ObservableCollection<NavigationItem> navigations = new ObservableCollection<NavigationItem>();
		private OnLanguageChangedDelegate onSymbolicTextLanguageChangedDelegate;

		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMINavigation.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMINavigation.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMINavigation.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMINavigation.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		public string SymbolicText
		{
			get
			{
				return (string)base.GetValue(HMINavigation.SymbolicTextProperty);
			}
			set
			{
				base.SetValue(HMINavigation.SymbolicTextProperty, value);
			}
		}
		[Category("HMI")]
		public ObservableCollection<NavigationItem> Navigations
		{
			get
			{
				return this.navigations;
			}
		}
		private static void OnSecurityAuthorizationNamePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityDenyAccessBehaviourPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			HMINavigation? wFNavigation = dp as HMINavigation;
			if (wFNavigation != null && DesignerProperties.GetIsInDesignMode(wFNavigation) && wFNavigation.NavigateButtonElement != null)
			{
				wFNavigation.NavigateButtonElement.Content=(wFNavigation.SymbolicText);
			}
		}
		public HMINavigation()
		{
			base.DefaultStyleKey=(typeof(HMINavigation));
		}
		public override void OnApplyTemplate()
		{
			this.NavigateButtonElement = null;

			base.OnApplyTemplate();

			this.NavigateButtonElement = (base.GetTemplateChild("NavigateButtonElement") as Button);

			bool isInDesignMode;
			if (isInDesignMode = DesignerProperties.GetIsInDesignMode(this))
			{
				if (this.NavigateButtonElement != null)
				{
					this.NavigateButtonElement.Content=(this.SymbolicText);
				}
				return;
			}

			if (this.connector != null)
			{
				return;
			}

			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();

            if (this.NavigateButtonElement != null)
			{
				this.NavigateButtonElement.Content=(string.Empty);
				this.NavigateButtonElement.Click+=(new RoutedEventHandler(this.OnNavigateButtonClick));
				this.NavigateButtonElement.Focus();
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

			foreach (NavigationItem current in this.navigations)
			{
				current.Source = StringPlaceholdersResolver.Resolve(current.Source, this);
				foreach (ParameterItem parameterItem in current.Parameters)
				{
					parameterItem.ParameterName = StringPlaceholdersResolver.Resolve(parameterItem.ParameterName, this);
					parameterItem.ParameterValue = StringPlaceholdersResolver.Resolve(parameterItem.ParameterValue, this);
				}
			}

			if (!string.IsNullOrEmpty(this.SymbolicText))
			{
				try
				{
					this.SymbolicText = StringPlaceholdersResolver.Resolve(this.SymbolicText, this);
					this.onSymbolicTextLanguageChangedDelegate = new OnLanguageChangedDelegate(this.onSymbolicTextLanguageChanged);
					this.connector.RegisterLanguageChangedHandler(new string[]
					{
						this.SymbolicText
					}, this.onSymbolicTextLanguageChangedDelegate);
				}
				catch (Exception ex)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}

			this.RegisteMouseHandlers();
		}
		private void onSymbolicTextLanguageChanged(string[] symbolicNames, string[] translationTexts)
		{
			try
			{
				if (translationTexts[0] != null && !translationTexts[0].Equals("!null!", StringComparison.InvariantCultureIgnoreCase))
				{
					this.NavigateButtonElement!.Content=(translationTexts[0]);
				}
				else
				{
					this.NavigateButtonElement!.Content=(symbolicNames[0]);
				}
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void HMINavigation_MouseEnter(object sender, MouseEventArgs e)
		{
			base.Cursor=(Cursors.Hand);
		}
		private void HMINavigation_MouseLeave(object sender, MouseEventArgs e)
		{
			base.Cursor=(Cursors.Arrow);
		}
		private void connector_UserContextChanged(string userName)
		{
			if (!this.isSecurityAvailable)
			{
				return;
			}
			this.isAuthorized = this.connector.CheckProjectAuthorization(this.SecurityAuthorizationName);
			switch (this.SecurityDenyAccessBehaviour)
			{
			case DenyAccessBehaviour.Disable:
				this.EnableControl(this.isAuthorized);
				return;
			case DenyAccessBehaviour.Hide:
				this.ShowControl(this.isAuthorized);
				return;
			default:
				throw new ArgumentOutOfRangeException("DenyAccessBehaviour");
			}
		}
		private void OnNavigateButtonClick(object sender, RoutedEventArgs e)
		{
			NavigationControlHelper.CreateNavigationControl(this, this.Navigations);
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
		private void RegisteMouseHandlers()
		{
			base.MouseEnter+=(new MouseEventHandler(this.HMINavigation_MouseEnter));
			base.MouseLeave+=(new MouseEventHandler(this.HMINavigation_MouseLeave));
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}

			this.DisposeConnector();
			this.DisposeMouseHandlers();
			this.StopStoryboards();
		}
		private void DisposeConnector()
		{
			if (this.connector == null)
			{
				return;
			}
			if (this.onSymbolicTextLanguageChangedDelegate != null)
			{
				this.connector.UnregisterLanguageChangedHandler(null, this.onSymbolicTextLanguageChangedDelegate);
			}
			this.connector.Dispose();
		}
		private void DisposeMouseHandlers()
		{
			base.MouseEnter-=(new MouseEventHandler(this.HMINavigation_MouseEnter));
			base.MouseLeave-=(new MouseEventHandler(this.HMINavigation_MouseLeave));
		}
	}
}
