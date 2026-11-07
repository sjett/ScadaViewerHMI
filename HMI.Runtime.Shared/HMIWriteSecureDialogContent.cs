// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HMI.Runtime.Core;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Dialogs;
using HMI.Runtime.Shared.KeyboardControl;
using DialogResult = HMI.Runtime.Shared.Dialogs.DialogResult;

namespace HMI.Runtime.Shared
{
	[
		TemplatePart(Name = "UserPasswordPasswordBoxElement", Type = typeof(PasswordBox)), 
		TemplatePart(Name = "OKButtonElement", Type = typeof(Button)), 
		TemplatePart(Name = "UserPasswordTextBlockElement", Type = typeof(TextBlock)), 
		TemplatePart(Name = "CancelButtonElement", Type = typeof(Button))]
	public class HMIWriteSecureDialogContent : Control, IDisposable
	{
		private const string ElementUserPasswordTextBlock = "UserPasswordTextBlockElement";
		private const string ElementUserPasswordPasswordBox = "UserPasswordPasswordBoxElement";
		private const string ElementOKButton = "OKButtonElement";
		private const string ElementCancelButton = "CancelButtonElement";

		private const string SymbolicNameUserPassword = "HMISN_Ctrl_HMIUserLogin_Password";
		private const string SymbolicNameOK = "HMISN_Ctrl_HMIUserLogin_OKButton";
		private const string SymbolicNameCancel = "HMISN_Ctrl_HMIUserLogin_CancelButton";

		private const string DefaultTextUserPassword = "密码:";
		private const string DefaultTextOK = "确定";
		private const string DefaultTextCancel = "取消";

		private IHMIConnector connector;
		private OnLanguageChangedDelegate symbolicTextTranslationsChangedDelegate;
		private Dictionary<string, SymbolicTextTranslation> translations = new Dictionary<string, SymbolicTextTranslation>();
		private TextBlock passwordLabel;
		private PasswordBox passwordBox;
		private Button okButton;
		private Button cancelButton;
		public event EventHandler<DialogClosedEventArgs> Closed;
		public string Password
		{
			get
			{
				if (this.passwordBox == null)
				{
					return string.Empty;
				}
				return this.passwordBox.Password;
			}
			set
			{
				if (this.passwordBox != null)
				{
					this.passwordBox.Password=(value);
				}
			}
		}
		private bool IsInDesignMode
		{
			get
			{
				return DesignerProperties.GetIsInDesignMode(this);
			}
		}
		public HMIWriteSecureDialogContent()
		{
			base.DefaultStyleKey=(typeof(HMIWriteSecureDialogContent));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.InitializeControls();
			if (!this.IsInDesignMode && this.connector == null)
			{
				this.InitializeCommunication();
				this.InitializeTranslations();
				this.RegisterLanguageChangedHandler();
			}
		}
		private void InitializeCommunication()
		{
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			this.symbolicTextTranslationsChangedDelegate = new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler);
		}
		private void InitializeControls()
		{
			this.passwordLabel = (base.GetTemplateChild("UserPasswordTextBlockElement") as TextBlock);
			this.passwordBox = (base.GetTemplateChild("UserPasswordPasswordBoxElement") as PasswordBox);
			if (this.passwordBox != null)
			{
				this.passwordBox.KeyDown +=(new KeyEventHandler(this.OnKeyDown));
			}
			this.okButton = (base.GetTemplateChild("OKButtonElement") as Button);
			if (this.okButton != null)
			{
				this.okButton.Click +=(new RoutedEventHandler(this.okButton_Click));
				this.okButton.KeyDown +=(new KeyEventHandler(this.OnKeyDown));
			}
			this.cancelButton = (base.GetTemplateChild("CancelButtonElement") as Button);
			if (this.cancelButton != null)
			{
				this.cancelButton.Click +=(new RoutedEventHandler(this.cancelButton_Click));
				this.cancelButton.KeyDown +=(new KeyEventHandler(this.OnKeyDown));
			}
		}
		private void RegisterLanguageChangedHandler()
		{
			try
			{
				string[] symbolicNames = new string[this.translations.Count];
				this.translations.Keys.CopyTo(symbolicNames, 0);
				this.connector.RegisterLanguageChangedHandler(symbolicNames, this.symbolicTextTranslationsChangedDelegate);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicNames, string[] translationTexts)
		{
			for (int i = 0; i < symbolicNames.Length; i++)
			{
				SymbolicTextTranslation symbolicTextTranslation = this.translations[symbolicNames[i]];
				symbolicTextTranslation.Translation = translationTexts[i];
			}
			this.UpdateControlTexts();
		}
		private void UpdateControlTexts()
		{
			this.SetLabelText(this.passwordLabel, this.translations["HMISN_Ctrl_HMIUserLogin_Password"], "密码:");
            this.SetButtonText(this.okButton, this.translations["HMISN_Ctrl_HMIUserLogin_OKButton"], "确定");
            this.SetButtonText(this.cancelButton, this.translations["HMISN_Ctrl_HMIUserLogin_CancelButton"], "取消");
		}
		private void SetButtonText(Button button, SymbolicTextTranslation stt, string defaultText)
		{
			if (button != null)
			{
				button.Content = (this.IsValidText(stt.Translation) ? stt.Translation : defaultText);
			}
		}
		private void SetLabelText(TextBlock label, SymbolicTextTranslation stt, string defaultText)
		{
			if (label != null)
			{
				label.Text = (this.IsValidText(stt.Translation) ? stt.Translation : defaultText);
			}
		}
		private bool IsValidText(string text)
		{
			return text != null && !text.Equals("!null!", StringComparison.InvariantCultureIgnoreCase);
		}
		private void InitializeTranslations()
		{
			this.AddTranslation("HMISN_Ctrl_HMIUserLogin_Password");
			this.AddTranslation("HMISN_Ctrl_HMIUserLogin_OKButton");
			this.AddTranslation("HMISN_Ctrl_HMIUserLogin_CancelButton");
		}
		private void AddTranslation(string symbolicName)
		{
			SymbolicTextTranslation symbolicTextTranslation = new SymbolicTextTranslation(symbolicName);
			this.translations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
		}
		private void Dispose(bool dispose)
		{
			if (dispose)
			{
				if (this.passwordBox != null)
				{
					this.passwordBox.KeyDown -=(new KeyEventHandler(this.OnKeyDown));
					KeyboardService.DetachKeyboardFrom(this.passwordBox);
					Grid grid = base.GetTemplateChild("BoxesGridElement") as Grid;
					if (grid != null && grid.Children.Contains(this.passwordBox))
					{
						grid.Children.Remove(this.passwordBox);
					}
				}
				if (this.okButton != null)
				{
					this.okButton.Click-=(new RoutedEventHandler(this.okButton_Click));
					this.okButton.KeyDown -=(new KeyEventHandler(this.OnKeyDown));
				}
				if (this.cancelButton != null)
				{
					this.cancelButton.Click-=(new RoutedEventHandler(this.cancelButton_Click));
					this.cancelButton.KeyDown -=(new KeyEventHandler(this.OnKeyDown));
				}
				StackPanel stackPanel = base.GetTemplateChild("ButtonsPanelElement") as StackPanel;
				if (stackPanel != null)
				{
					stackPanel.Children.Clear();
				}
				this.DisposeHMIConnector();
				this.StopStoryboards();
			}
		}
		private void DisposeHMIConnector()
		{
			if (this.connector != null)
			{
				this.UnregisterLanguageChangedHandler();
				this.connector.Dispose();
				this.connector = null;
			}
		}
		private void UnregisterLanguageChangedHandler()
		{
			if (this.symbolicTextTranslationsChangedDelegate != null)
			{
				this.connector.UnregisterLanguageChangedHandler(null, this.symbolicTextTranslationsChangedDelegate);
			}
		}
		private void RaiseCloseEvent(DialogResult result)
		{
			if (this.Closed != null)
			{
				this.Closed.Invoke(this, new DialogClosedEventArgs(result));
			}
		}
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			Key key = e.Key;
            if (key != Key.Enter)
			{
                if (key == Key.Escape)
				{
					this.RaiseCloseEvent(DialogResult.Cancel);
					e.Handled=(true);
					return;
				}
				e.Handled=(false);
			}
			else
			{
				if (!(sender is Button))
				{
					this.RaiseCloseEvent(DialogResult.OK);
					e.Handled=(true);
					return;
				}
			}
		}
		private void cancelButton_Click(object sender, RoutedEventArgs e)
		{
			this.RaiseCloseEvent(DialogResult.Cancel);
		}
		private void okButton_Click(object sender, RoutedEventArgs e)
		{
			this.RaiseCloseEvent(DialogResult.OK);
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
