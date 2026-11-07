// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Dialogs;

namespace HMI.RuntimeControls.General.UserLoginDialogs
{
	public class HMIUserLogin1DialogContent : Control, IDisposable
	{
		private const string ElementUserNameTextBox = "UserNameTextBoxElement";
		private const string ElementIsDomainUserLogin = "uxIsDomainUserLogin";
		private const string ElementChangePasswordButton = "ChangePasswordButtonElement";
		private const string ElementLoginButton = "LoginButtonElement";
		private const string ElementLogoutButton = "LogoutButtonElement";
		private const string ElementUserPasswordPasswordBox = "UserPasswordPasswordBoxElement";
		private const string ElementLoginMessageBox = "LoginMessageBoxElement";
		private const string ElementUserNameTextBlock = "UserNameTextBlockElement";
		private const string ElementUserPasswordTextBlock = "UserPasswordTextBlockElement";

		private const string SNDialogCaption = "HMISN_Ctrl_HMIUserLogin_DialogCaption";
		private const string SNUserName = "HMISN_Ctrl_HMIUserLogin_UserName";
		private const string SNUserPassword = "HMISN_Ctrl_HMIUserLogin_Password";
		private const string SNLoginButton = "HMISN_Ctrl_HMIUserLogin_LoginButton";
		private const string SNLogoutButton = "HMISN_Ctrl_HMIUserLogin_LogoffButton";
		private const string SNEmptyUserName = "HMISN_Ctrl_HMIUserLogin_EmptyUserName";
		private const string SNIsDomainUser = "HMISN_Ctrl_HMIUserLogin_IsDomainUser";
		private const string SNChangePasswordButton = "HMISN_Ctrl_HMIUserLogin_ChangePasswordButton";

		private const string DefaultTextDialogCaption = "HMI 登录";
		private const string DefaultTextUserName = "用户名:";
		private const string DefaultTextUserPassword = "密码:";
		private const string DefaultTextLoginButton = "登录";
		private const string DefaultTextLogoutButton = "退出";
		private const string DefaultTextChangePasswordButton = "修改密码...";
		private const string DefaultTextIsDomainUser = "域用户";
		private const string DefaultTextEmptyUserName = "用户名不能为空";

		private readonly IList<DialogResource> dialogResources;
		private IHMIConnector connector;

		public event Action<string, string, bool> LoginTriggered;
		public event Action LogoutTriggered;
		public event Action ChangePasswordTriggered;
		public event Action CloseWindow;

		public bool CanConnectWithDomainUser
		{
			get;
			set;
		}
		public bool CanChangePassword
		{
			get;
			set;
		}
		public HMIUserLogin1DialogContent()
		{
			List<DialogResource> list = new List<DialogResource>();
			list.Add(new DialogResource(ElementLoginMessageBox, SNDialogCaption, DefaultTextDialogCaption));
			list.Add(new DialogResource(ElementUserNameTextBlock, SNUserName, DefaultTextUserName));
			list.Add(new DialogResource(ElementUserPasswordTextBlock, SNUserPassword, DefaultTextUserPassword));
			list.Add(new DialogResource(ElementIsDomainUserLogin, SNIsDomainUser, DefaultTextIsDomainUser));
			list.Add(new DialogResource(ElementLoginButton, SNLoginButton, DefaultTextLoginButton));
			list.Add(new DialogResource(ElementLogoutButton, SNLogoutButton, DefaultTextLogoutButton));
			list.Add(new DialogResource(ElementChangePasswordButton, SNChangePasswordButton, DefaultTextChangePasswordButton));
			list.Add(new DialogResource(null, SNEmptyUserName, DefaultTextEmptyUserName));
			this.dialogResources = list;
			
			base.DefaultStyleKey=(base.GetType());
		}
		public HMIUserLogin1DialogContent(bool canConnectWithDomainUser, bool canChangePassword) : this()
		{
			this.CanConnectWithDomainUser = canConnectWithDomainUser;
			this.CanChangePassword = canChangePassword;
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			this.RegisterToConnectorEvents();
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.RegisterToUIEvents();
			this.TranslateDialogTexts();
			this.SetUpDialog();
		}
		private void connector_UserContextChanged(string userName)
		{
			this.SetChangePasswordVisibility();
		}
		private void OnLanguageChangedHandler(string[] symbolicnames, string[] translationtexts)
		{
			for (int i = 0; i < symbolicnames.Length; i++)
			{
				string symbolicname = symbolicnames[i];
				string translation = translationtexts[i];
				DialogResource? dialogResource = Enumerable.FirstOrDefault<DialogResource>(this.dialogResources, (DialogResource x) => x.SymbolicText == symbolicname);
				if (dialogResource != null)
				{
					dialogResource.Translation = translation;
				}
			}
			this.TranslateDialogTexts();
		}
		private void RegisterToConnectorEvents()
		{
			string[] symbolicNames = Enumerable.ToArray<string>(Enumerable.Select<DialogResource, string>(this.dialogResources, (DialogResource x) => x.SymbolicText));
			this.connector.RegisterLanguageChangedHandler(symbolicNames, new OnLanguageChangedDelegate(this.OnLanguageChangedHandler));
			this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
		}
		private string GetTranslationFor(string symbolicText)
		{
			DialogResource? dialogResource = Enumerable.FirstOrDefault<DialogResource>(this.dialogResources, (DialogResource x) => x.SymbolicText == symbolicText);
			if (dialogResource != null)
			{
				return dialogResource.Translation;
			}
			return string.Empty;
		}
		private void LoginDialogHandleKeyDown(object sender, KeyEventArgs e)
		{
			Key key = e.Key;
			if (key == Key.Enter)
			{
				this.AttemptLogin();
				e.Handled=(true);
				return;
			}
			if (key != Key.Escape)
			{
				e.Handled=(false);
				return;
			}
			this.CloseLoginDialog();
			e.Handled=(true);
		}
		private void SetUpDialog()
		{
			TextBox? textBox = base.GetTemplateChild(ElementUserNameTextBox) as TextBox;
			CheckBox? checkBox = base.GetTemplateChild(ElementIsDomainUserLogin) as CheckBox;
			if (textBox != null)
			{
				textBox.Focus();
				textBox.SelectAll();
			}
			if (checkBox != null && !this.CanConnectWithDomainUser)
			{
				checkBox.IsChecked=(new bool?(false));
				checkBox.Visibility= System.Windows.Visibility.Collapsed;
			}
			this.SetChangePasswordVisibility();
		}
		private void SetChangePasswordVisibility()
		{
			Button? button = base.GetTemplateChild(ElementChangePasswordButton) as Button;
			if (button != null)
			{
				button.Visibility=((string.IsNullOrEmpty(this.connector.GetUserName()) || this.connector.GetIsDomainUser()) ? Visibility.Collapsed : Visibility.Visible);
				if (!this.CanChangePassword)
				{
					button.Visibility= System.Windows.Visibility.Collapsed;
				}
			}
		}
		private void RegisterToUIEvents()
		{
			Button? button = base.GetTemplateChild(ElementLoginButton) as Button;
			Button? button2 = base.GetTemplateChild(ElementLogoutButton) as Button;
			Button? button3 = base.GetTemplateChild(ElementChangePasswordButton) as Button;
			TextBox? textBox = base.GetTemplateChild(ElementUserNameTextBox) as TextBox;
			PasswordBox? passwordBox = base.GetTemplateChild(ElementUserPasswordPasswordBox) as PasswordBox;
			if (button != null)
			{
				button.Click+=(delegate(object sender, RoutedEventArgs e)
				{
					this.AttemptLogin();
				});
				button.KeyDown+=(new KeyEventHandler(this.LoginDialogHandleKeyDown));
			}
			if (button2 != null)
			{
				button2.Click+=(delegate(object sender, RoutedEventArgs e)
				{
					this.TriggerLogout();
				});
				button2.KeyDown+=(new KeyEventHandler(this.LoginDialogHandleKeyDown));
			}
			if (button3 != null)
			{
				button3.Click+=(delegate(object sender, RoutedEventArgs e)
				{
					this.TriggerChangePassword();
				});
				button3.KeyDown+=(new KeyEventHandler(this.LoginDialogHandleKeyDown));
			}
			if (textBox != null)
			{
				textBox.KeyDown+=(new KeyEventHandler(this.LoginDialogHandleKeyDown));
			}
			if (passwordBox != null)
			{
				passwordBox.KeyDown+=(new KeyEventHandler(this.LoginDialogHandleKeyDown));
			}
		}
		private void TranslateDialogTexts()
		{
			foreach (var current in this.dialogResources)
			{
				DependencyObject? control = string.IsNullOrEmpty(current.Name) ? null : base.GetTemplateChild(current.Name);
				this.SetTranslatedText(current.Translation, control);
			}
		}
		private void SetTranslatedText(string translation, DependencyObject? control)
		{
			if (control == null)
			{
				return;
			}
			if (control is TextBlock)
			{
				(control as TextBlock)!.Text=(translation);
				return;
			}
			if (control is CheckBox)
			{
				(control as CheckBox)!.Content=(translation);
				return;
			}
			if (control is Button)
			{
				(control as Button)!.Content=(translation);
			}
		}
		private void CloseLoginDialog()
		{
			if (this.CloseWindow != null)
			{
				this.CloseWindow.Invoke();
			}
			this.Dispose();
		}
		private void ShowMessageBox(string text)
		{
			MessageBoxManager messageBoxManager = new MessageBoxManager(null);
			messageBoxManager.Show(text, "HMI", MessageBoxType.Information, "确定", "取消");
		}
		private void AttemptLogin()
		{
			TextBox? textBox = base.GetTemplateChild(ElementUserNameTextBox) as TextBox;
			PasswordBox? passwordBox = base.GetTemplateChild(ElementUserPasswordPasswordBox) as PasswordBox;
			CheckBox? checkBox = base.GetTemplateChild(ElementIsDomainUserLogin) as CheckBox;
			if (textBox == null || passwordBox == null || checkBox == null)
			{
				this.CloseLoginDialog();
				return;
			}
			if (string.IsNullOrEmpty(textBox.Text))
			{
				this.ShowMessageBox(this.GetTranslationFor(SNEmptyUserName));
				return;
			}
			this.TriggerLogin(textBox.Text, passwordBox.Password, checkBox.IsChecked.HasValue && checkBox.IsChecked.Value);
		}
		private void TriggerLogin(string userName, string password, bool isDomainUser)
		{
			if (this.LoginTriggered != null)
			{
				this.LoginTriggered.Invoke(userName, password, isDomainUser);
			}
		}
		private void TriggerLogout()
		{
			if (this.LogoutTriggered != null)
			{
				this.LogoutTriggered.Invoke();
			}
		}
		private void TriggerChangePassword()
		{
			if (this.ChangePasswordTriggered != null)
			{
				this.ChangePasswordTriggered.Invoke();
			}
		}
		public void Dispose()
		{
			this.Dispose(true);
		}
		private void Dispose(bool dispose)
		{
			if (!dispose)
			{
				return;
			}
			this.UnregisterFromEvents();
		}
		private void UnregisterFromEvents()
		{
			this.UnregisterAndDisposeConnector();
			this.UnregisterFromUIEvents();
		}
		private void UnregisterAndDisposeConnector()
		{
			if (this.connector != null)
			{
				this.connector.UnregisterLanguageChangedHandler(null, new OnLanguageChangedDelegate(this.OnLanguageChangedHandler));
				this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
				this.connector.Dispose();
				this.connector = null;
			}
		}
		private void UnregisterFromUIEvents()
		{
			Button? button = base.GetTemplateChild(ElementLoginButton) as Button;
			Button? button2 = base.GetTemplateChild(ElementLogoutButton) as Button;
			Button? button3 = base.GetTemplateChild(ElementChangePasswordButton) as Button;
			TextBox? textBox = base.GetTemplateChild(ElementUserNameTextBox) as TextBox;
			PasswordBox? passwordBox = base.GetTemplateChild(ElementUserPasswordPasswordBox) as PasswordBox;
			if (button != null)
			{
				button.KeyDown-=(new KeyEventHandler(this.LoginDialogHandleKeyDown));
			}
			if (button2 != null)
			{
				button2.KeyDown-=(new KeyEventHandler(this.LoginDialogHandleKeyDown));
			}
			if (button3 != null)
			{
				button3.KeyDown-=(new KeyEventHandler(this.LoginDialogHandleKeyDown));
			}
			if (textBox != null)
			{
				textBox.KeyDown-=(new KeyEventHandler(this.LoginDialogHandleKeyDown));
			}
			if (passwordBox != null)
			{
				passwordBox.KeyDown-=(new KeyEventHandler(this.LoginDialogHandleKeyDown));
			}
		}
	}
}
