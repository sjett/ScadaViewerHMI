// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.Navigation
{
    /// <summary>

    /// </summary>
    [TemplatePart(Name = "NavigateToggleButtonElement", Type = typeof(ToggleButton))]
	public class HMINavigationToggle1 : Control, IDisposable
	{
		private ToggleButton? NavigateToggleButtonElement;
		private IHMIConnector connector;
		private bool isSecurityAvailable;
		private bool isAuthorized;
		private ObservableCollection<NavigationItem> navigations = new ObservableCollection<NavigationItem>();
		private OnLanguageChangedDelegate onSymbolicTextLanguageChangedDelegate;
		private bool isNavigationControlCreated;

		public static readonly DependencyProperty CheckedProperty = DependencyProperty.Register("Checked", typeof(bool), typeof(HMINavigationToggle1), new PropertyMetadata(false, new PropertyChangedCallback(HMINavigationToggle1.OnCheckedPropertyChanged)));
		public static readonly DependencyProperty MenuGroupProperty = DependencyProperty.Register("MenuGroup", typeof(string), typeof(HMINavigationToggle1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMINavigationToggle1.OnMenuGroupPropertyChanged)));
		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMINavigationToggle1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMINavigationToggle1.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMINavigationToggle1), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMINavigationToggle1.OnSecurityDenyAccessBehaviourPropertyChanged)));
		public static readonly DependencyProperty SymbolicTextProperty = DependencyProperty.Register("SymbolicText", typeof(string), typeof(HMINavigationToggle1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMINavigationToggle1.OnSymbolicTextPropertyChanged)));
		
		[Category("HMI")]
		public bool Checked
		{
			get
			{
				return (bool)base.GetValue(HMINavigationToggle1.CheckedProperty);
			}
			set
			{
				base.SetValue(HMINavigationToggle1.CheckedProperty, value);
			}
		}
		[Category("HMI")]
		public string MenuGroup
		{
			get
			{
				return (string)base.GetValue(HMINavigationToggle1.MenuGroupProperty);
			}
			set
			{
				base.SetValue(HMINavigationToggle1.MenuGroupProperty, value);
			}
		}
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMINavigationToggle1.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMINavigationToggle1.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMINavigationToggle1.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMINavigationToggle1.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		[Category("HMI")]
		public string SymbolicText
		{
			get
			{
				return (string)base.GetValue(HMINavigationToggle1.SymbolicTextProperty);
			}
			set
			{
				base.SetValue(HMINavigationToggle1.SymbolicTextProperty, value);
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
		private static void OnCheckedPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			HMINavigationToggle1? wFNavigationToggle = dp as HMINavigationToggle1;
			if (wFNavigationToggle != null && wFNavigationToggle.NavigateToggleButtonElement != null)
			{
				wFNavigationToggle.NavigateToggleButtonElement.IsChecked=(new bool?((bool)e.NewValue));
			}
		}
		private static void OnMenuGroupPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityAuthorizationNamePropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityDenyAccessBehaviourPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
		{
			HMINavigationToggle1? wFNavigationToggle = dp as HMINavigationToggle1;
			if (wFNavigationToggle != null && DesignerProperties.GetIsInDesignMode(wFNavigationToggle) && wFNavigationToggle.NavigateToggleButtonElement != null)
			{
				wFNavigationToggle.NavigateToggleButtonElement.Content=(wFNavigationToggle.SymbolicText);
			}
		}
		public HMINavigationToggle1()
		{
			base.DefaultStyleKey=(typeof(HMINavigationToggle1));
			this.isNavigationControlCreated = true;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void onSymbolicTextLanguageChanged(string[] symbolicNames, string[] translationTexts)
		{
			if (this.NavigateToggleButtonElement != null)
			{
				try
				{
					if (translationTexts[0] != null && !translationTexts[0].Equals("!null!", StringComparison.InvariantCultureIgnoreCase))
					{
						this.NavigateToggleButtonElement.Content=(translationTexts[0]);
					}
					else
					{
						this.NavigateToggleButtonElement.Content=(symbolicNames[0]);
					}
				}
				catch (Exception ex)
				{
					this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
				}
			}
		}
		private void HMINavigationToggle1_MouseEnter(object sender, MouseEventArgs e)
		{
			base.Cursor=(Cursors.Hand);
		}
		private void HMINavigationToggle1_MouseLeave(object sender, MouseEventArgs e)
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
		private void OnNavigateToggleButton_Checked(object sender, RoutedEventArgs e)
		{
			this.Checked = true;
			if (this.isNavigationControlCreated)
			{
				NavigationMenuGroup.UncheckOtherNavToggleInGroup(this);
				NavigationControlHelper.CreateNavigationControl(this, this.Navigations);
			}
			this.isNavigationControlCreated = true;
		}
		private void OnNavigateToggleButton_Click(object sender, RoutedEventArgs e)
		{
			if (this.Checked && this.NavigateToggleButtonElement != null && this.NavigateToggleButtonElement.IsChecked.HasValue && !this.NavigateToggleButtonElement.IsChecked.Value)
			{
				this.isNavigationControlCreated = false;
				this.NavigateToggleButtonElement.IsChecked=(new bool?(true));
			}
		}
		public override void OnApplyTemplate()
		{
			this.NavigateToggleButtonElement = null;
			base.OnApplyTemplate();
			this.NavigateToggleButtonElement = (base.GetTemplateChild("NavigateToggleButtonElement") as ToggleButton);

			bool isInDesignMode;
			if (isInDesignMode = DesignerProperties.GetIsInDesignMode(this))
			{
				if (this.NavigateToggleButtonElement != null)
				{
					this.NavigateToggleButtonElement.Content=(this.SymbolicText);
					this.NavigateToggleButtonElement.IsThreeState=(false);
					this.NavigateToggleButtonElement.IsChecked=(new bool?(this.Checked));
				}
				return;
			}

			if (this.connector != null)
			{
				return;
			}
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();

            if (this.NavigateToggleButtonElement != null)
			{
				this.NavigateToggleButtonElement.Content=(string.Empty);
				this.NavigateToggleButtonElement.IsThreeState=(false);
				this.NavigateToggleButtonElement.IsChecked=(new bool?(this.Checked));
				this.NavigateToggleButtonElement.Checked+=(new RoutedEventHandler(this.OnNavigateToggleButton_Checked));
				this.NavigateToggleButtonElement.Click+=(new RoutedEventHandler(this.OnNavigateToggleButton_Click));
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
                    base.Visibility = System.Windows.Visibility.Collapsed;
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

			NavigationMenuGroup.TryAdd(this);

			this.RegisterMouseHandlers();
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
                base.Visibility = System.Windows.Visibility.Collapsed;
				return;
			}
            base.Visibility = System.Windows.Visibility.Visible;
		}
		private void RegisterMouseHandlers()
		{
			base.MouseEnter+=(new MouseEventHandler(this.HMINavigationToggle1_MouseEnter));
			base.MouseLeave+=(new MouseEventHandler(this.HMINavigationToggle1_MouseLeave));
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

			NavigationMenuGroup.TryRemove(this);

			this.connector.Dispose();
		}
		private void DisposeMouseHandlers()
		{
			base.MouseEnter-=(new MouseEventHandler(this.HMINavigationToggle1_MouseEnter));
			base.MouseLeave-=(new MouseEventHandler(this.HMINavigationToggle1_MouseLeave));
		}
	}
}
