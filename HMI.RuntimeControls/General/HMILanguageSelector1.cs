// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Core;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.General
{
	[TemplatePart(Name = "OutputElement", Type = typeof(ComboBox))]
	public class HMILanguageSelector1 : Control, IDisposable
	{
		private const string ElementOutput = "OutputElement";
		private const string DummySymbolicText = "SLDummySymbolicText";

		private ComboBox comboBoxElement;
		private IHMIConnector connector;
		private bool isInitialized;
		private bool isSecurityAvailable;
		private bool isAuthorized;
		private bool processSelectionChangedEvent;

		public static readonly DependencyProperty SecurityAuthorizationNameProperty = DependencyProperty.Register("SecurityAuthorizationName", typeof(string), typeof(HMILanguageSelector1), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMILanguageSelector1.OnSecurityAuthorizationNamePropertyChanged)));
		public static readonly DependencyProperty SecurityDenyAccessBehaviourProperty = DependencyProperty.Register("SecurityDenyAccessBehaviour", typeof(DenyAccessBehaviour), typeof(HMILanguageSelector1), new PropertyMetadata(DenyAccessBehaviour.Hide, new PropertyChangedCallback(HMILanguageSelector1.OnSecurityDenyAccessBehaviourPropertyChanged)));
		
		[Category("HMI")]
		public string SecurityAuthorizationName
		{
			get
			{
				return (string)base.GetValue(HMILanguageSelector1.SecurityAuthorizationNameProperty);
			}
			set
			{
				base.SetValue(HMILanguageSelector1.SecurityAuthorizationNameProperty, value);
			}
		}
		[Category("HMI")]
		public DenyAccessBehaviour SecurityDenyAccessBehaviour
		{
			get
			{
				return (DenyAccessBehaviour)base.GetValue(HMILanguageSelector1.SecurityDenyAccessBehaviourProperty);
			}
			set
			{
				base.SetValue(HMILanguageSelector1.SecurityDenyAccessBehaviourProperty, value);
			}
		}
		public ObservableCollection<HMILanguage> Languages
		{
			get;
			private set;
		}
		private static void OnSecurityAuthorizationNamePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSecurityDenyAccessBehaviourPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMILanguageSelector1()
		{
			base.DefaultStyleKey=(typeof(HMILanguageSelector1));
			this.Languages = new ObservableCollection<HMILanguage>();
			base.DataContext=(this);
			this.processSelectionChangedEvent = false;
		}
		~HMILanguageSelector1()
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
			if (!disposing)
			{
				return;
			}
			this.DisposeConnector();
			this.DisposeCombobox();
			this.StopStoryboards();
		}
		private void DisposeCombobox()
		{
			if (this.comboBoxElement == null)
			{
				return;
			}
			this.comboBoxElement.SelectionChanged-=(new SelectionChangedEventHandler(this.ComboBoxElementSelectionChanged));
		}
		private void DisposeConnector()
		{
			if (this.connector == null)
			{
				return;
			}
			this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
			this.connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler));
			this.connector.Dispose();
		}
		private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicTexts, string[] translationTexts)
		{
			this.processSelectionChangedEvent = false;
			if (this.isInitialized)
			{
				this.SetSelectedItemFromLanguageID();
			}
			else
			{
				this.InitializeControl();
			}
			this.processSelectionChangedEvent = true;
		}
		private void connector_UserContextChanged(string userName)
		{
			if (!this.isSecurityAvailable)
			{
				return;
			}
			this.isAuthorized = this.connector.CheckProjectAuthorization(this.SecurityAuthorizationName);
			this.HandleSecurityChanged();
		}
		private void ComboBoxElementSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (this.comboBoxElement != null && this.processSelectionChangedEvent)
			{
				int languageId = Convert.ToInt32(this.comboBoxElement.SelectedValue);
				this.connector.SetLanguageId(languageId);
				return;
			}
		}
		public override void OnApplyTemplate()
		{
			this.comboBoxElement = null;
			base.OnApplyTemplate();

			if (DesignerPropertiesExt.IsInDesignTool)
			{
				return;
			}
			base.IsEnabled=(false);
			this.InitializeComboBox();
			this.InitializeComboBySecurity();
			this.InitializeConnector();
		}
		private void InitializeConnector()
		{
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
			try
			{
				this.connector.RegisterLanguageChangedHandler(new string[]
				{
					"SLDummySymbolicText"
				}, new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler));
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void InitializeComboBySecurity()
		{
			if (string.IsNullOrEmpty(this.SecurityAuthorizationName))
			{
				this.isAuthorized = true;
				this.isSecurityAvailable = false;
				return;
			}
			this.isAuthorized = false;
			this.isSecurityAvailable = true;
			this.HandleSecurityChanged();
		}
		private void InitializeComboBox()
		{
			this.comboBoxElement = (base.GetTemplateChild("OutputElement") as ComboBox);
			if (this.comboBoxElement != null)
			{
				this.comboBoxElement.SelectionChanged+=(new SelectionChangedEventHandler(this.ComboBoxElementSelectionChanged));
			}
		}
		private void HandleSecurityChanged()
		{
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
		private void InitializeControl()
		{
			this.PopulateLanguages();
			this.isInitialized = true;
			this.EnableControl(this.isInitialized);
		}
		private void PopulateLanguages()
		{
			this.Languages.Clear();
			HMILanguage[] array = this.connector.ReadLanguages();
			if (array == null)
			{
				return;
			}
			foreach (var current in array.Where(x => x.IsActive))
			{
				this.Languages.Add(current);
			}
			this.SetSelectedItemFromLanguageID();
		}
		private void EnableControl(bool enable)
		{
			base.IsEnabled=(enable && this.isInitialized && (!this.isSecurityAvailable || this.isAuthorized || this.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Disable));
		}
		private void ShowControl(bool show)
		{
			Visibility visibility = System.Windows.Visibility.Collapsed;
            if (show)
			{
				if (!this.isSecurityAvailable || this.isAuthorized || this.SecurityDenyAccessBehaviour != DenyAccessBehaviour.Hide)
				{
					visibility =  System.Windows.Visibility.Visible;
				}
			}
			base.Visibility=(visibility);
		}
		private void SetSelectedItemFromLanguageID()
		{
			int languageId = this.connector.GetLanguageId();
			this.comboBoxElement.SelectedValue=(languageId);
		}
	}
}
