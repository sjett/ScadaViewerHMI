// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.General
{
	[TemplatePart(Name = "OutputElement", Type = typeof(TextBlock))]
	public class HMIUserDisplay1 : Control, IDisposable
	{
		private const string ElementOutput = "OutputElement";

		private TextBlock tbOutputElement;
		private IHMIConnector connector;
		private bool isSecurityAvailable;
		private bool isAuthorized;

		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMIUserDisplay1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIUserDisplay1.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMIUserDisplay1), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMIUserDisplay1.OnSecurityDenyAccessBehaviourPropertyChanged)));
		
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
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMIUserDisplay1.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMIUserDisplay1.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMIUserDisplay1.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMIUserDisplay1.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		private static void OnSecurityAuthorizationNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityDenyAccessBehaviourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMIUserDisplay1()
		{
			base.DefaultStyleKey=(typeof(HMIUserDisplay1));
		}
		~HMIUserDisplay1()
		{
			this.Dispose(false);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		private void connector_UserContextChanged(string userName)
		{
			if (this.tbOutputElement != null)
			{
				this.tbOutputElement.Text=(userName);
			}
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
				if (this.tbOutputElement != null)
				{
					this.tbOutputElement.Text=(string.Empty);
					return;
				}
			}
			else
			{
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
			}
		}
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (this.connector != null)
				{
					this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
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
	}
}
