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
	public class HMIUserLoginPasswordChangeContent : Control, IDisposable
	{
		private const string ElementCPUserTextBlock = "CPUserTextBlockElement";
		private const string ElementCPUserNameTextBlock = "CPUserNameTextBlockElement";
		private const string ElementCPCurrentPasswordTextBlock = "CPCurrentPasswordTextBlockElement";
		private const string ElementCPCurrentPasswordPasswordBox = "CPCurrentPasswordPasswordBoxElement";
		private const string ElementCPNewPasswordTextBlock = "CPNewPasswordTextBlockElement";
		private const string ElementCPNewPasswordPasswordBox = "CPNewPasswordPasswordBoxElement";
		private const string ElementCPConfirmPasswordTextBlock = "CPConfirmPasswordTextBlockElement";
		private const string ElementCPConfirmPasswordPasswordBox = "CPConfirmPasswordPasswordBoxElement";
		private const string ElementCPOKButton = "CPOKButtonElement";
		private const string ElementCPCancelButton = "CPCancelButtonElement";

		private const string SNUserName = "HMISN_Ctrl_HMIUserLogin_UserName";
		private const string SNCPCurrentPassword = "HMISN_Ctrl_HMIUserLogin_CurrentPassword";
		private const string SNCPNewPassword = "HMISN_Ctrl_HMIUserLogin_NewPassword";
		private const string SNCPConfirmPassword = "HMISN_Ctrl_HMIUserLogin_ConfirmPassword";
		private const string SNCPOK = "HMISN_Ctrl_HMIUserLogin_OKButton";
		private const string SNCPCancel = "HMISN_Ctrl_HMIUserLogin_CancelButton";
		private const string SNPasswordsDontMatch = "HMISN_Ctrl_HMIUserLogin_PasswordsDontMatch";
		private const string SNDialogCaption = "HMISN_Ctrl_HMIUserLogin_DialogCaption";

		private const string DefaultTextDialogCaption = "HMI 修改密码";
		private const string DefaultTextUserName = "用户名:";
		private const string DefaultTextCPCurrentPassword = "当前密码:";
		private const string DefaultTextCPNewPassword = "新密码:";
		private const string DefaultTextCPConfirmPassword = "确认新密码:";
		private const string DefaultTextCPOK = "确定";
		private const string DefaultTextCPCancel = "取消";
		private const string DefaultTextPasswordsDontMatch = "两次输入的密码不一致。";

		private IHMIConnector connector;
		private readonly IList<DialogResource> dialogResources;

		public event Action<string, string> ChangePasswordTriggered;
		public event Action CloseWindow;

		protected string UserTryingToLoggin
		{
			get;
			set;
		}
		protected bool IsPasswordExpired
		{
			get;
			set;
		}
		public HMIUserLoginPasswordChangeContent()
		{
			List<DialogResource> list = new List<DialogResource>();
			list.Add(new DialogResource(ElementCPUserTextBlock, SNUserName, DefaultTextUserName));
			list.Add(new DialogResource(ElementCPCurrentPasswordTextBlock, SNCPCurrentPassword, DefaultTextCPCurrentPassword));
			list.Add(new DialogResource(ElementCPNewPasswordTextBlock, SNCPNewPassword, DefaultTextCPNewPassword));
			list.Add(new DialogResource(ElementCPConfirmPasswordTextBlock, SNCPConfirmPassword, DefaultTextCPConfirmPassword));
			list.Add(new DialogResource(ElementCPOKButton, SNCPOK, DefaultTextCPOK));
			list.Add(new DialogResource(ElementCPCancelButton, SNCPCancel, DefaultTextCPCancel));
			list.Add(new DialogResource(null, SNDialogCaption, DefaultTextDialogCaption));
			list.Add(new DialogResource(null, SNPasswordsDontMatch, DefaultTextPasswordsDontMatch));
			this.dialogResources = list;
			//base..ctor();
			base.DefaultStyleKey=(base.GetType());
		}
		public HMIUserLoginPasswordChangeContent(bool isPasswordExpired, string userTryingToLoggin) : this()
		{
			this.IsPasswordExpired = isPasswordExpired;
			this.UserTryingToLoggin = userTryingToLoggin;
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			this.RegisterToConnectorEvents();
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
		private void ChangePasswordDialogHandleKeyDown(object sender, KeyEventArgs e)
		{
			Key key = e.Key;
            if (key == Key.Enter)
			{
				this.ChangePassword();
				e.Handled=(true);
				return;
			}
            if (key != Key.Escape)
			{
				e.Handled=(false);
				return;
			}
			this.CloseDialog();
			e.Handled=(true);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.RegisterToUIEvents();
			this.TranslateDialogTexts();
			this.SetUpDialog();
		}
		private void SetUpDialog()
		{
			TextBlock? textBlock = base.GetTemplateChild(ElementCPUserNameTextBlock) as TextBlock;
			PasswordBox? passwordBox = base.GetTemplateChild(ElementCPCurrentPasswordPasswordBox) as PasswordBox;
			if (passwordBox != null)
			{
				passwordBox.Focus();
				passwordBox.SelectAll();
			}
			if (textBlock != null)
			{
				textBlock.Text=(this.IsPasswordExpired ? this.UserTryingToLoggin : this.connector.GetUserName());
			}
		}
		private void RegisterToConnectorEvents()
		{
			string[] symbolicNames = Enumerable.ToArray<string>(Enumerable.Select<DialogResource, string>(this.dialogResources, (DialogResource x) => x.SymbolicText));
			this.connector.RegisterLanguageChangedHandler(symbolicNames, new OnLanguageChangedDelegate(this.OnLanguageChangedHandler));
		}
		private void RegisterToUIEvents()
		{
			Button? button = base.GetTemplateChild(ElementCPOKButton) as Button;
			Button? button2 = base.GetTemplateChild(ElementCPCancelButton) as Button;
			PasswordBox? passwordBox = base.GetTemplateChild(ElementCPCurrentPasswordPasswordBox) as PasswordBox;
			PasswordBox? passwordBox2 = base.GetTemplateChild(ElementCPNewPasswordPasswordBox) as PasswordBox;
			PasswordBox? passwordBox3 = base.GetTemplateChild(ElementCPConfirmPasswordPasswordBox) as PasswordBox;
			if (button != null)
			{
				button.Click+=(delegate(object sender, RoutedEventArgs e)
				{
					this.ChangePassword();
				});
				button.KeyDown+=(new KeyEventHandler(this.ChangePasswordDialogHandleKeyDown));
			}
			if (button2 != null)
			{
				button2.Click+=(delegate(object sender, RoutedEventArgs e)
				{
					this.CloseDialog();
				});
				button2.KeyDown+=(new KeyEventHandler(this.ChangePasswordDialogHandleKeyDown));
			}
			if (passwordBox != null)
			{
				passwordBox.KeyDown+=(new KeyEventHandler(this.ChangePasswordDialogHandleKeyDown));
			}
			if (passwordBox2 != null)
			{
				passwordBox2.KeyDown+=(new KeyEventHandler(this.ChangePasswordDialogHandleKeyDown));
			}
			if (passwordBox3 != null)
			{
				passwordBox3.KeyDown+=(new KeyEventHandler(this.ChangePasswordDialogHandleKeyDown));
			}
		}
		private void CloseDialog()
		{
			if (this.CloseWindow != null)
			{
				this.CloseWindow.Invoke();
			}
			this.Dispose();
		}
		private void ChangePassword()
		{
			PasswordBox? passwordBox = base.GetTemplateChild(ElementCPCurrentPasswordPasswordBox) as PasswordBox;
			PasswordBox? passwordBox2 = base.GetTemplateChild(ElementCPNewPasswordPasswordBox) as PasswordBox;
			PasswordBox? passwordBox3 = base.GetTemplateChild(ElementCPConfirmPasswordPasswordBox) as PasswordBox;
			if (passwordBox == null || passwordBox2 == null || passwordBox3 == null || string.IsNullOrEmpty(this.connector.GetUserName()))
			{
				return;
			}
			string password = passwordBox.Password;
			string password2 = passwordBox2.Password;
			string password3 = passwordBox3.Password;
			if (string.CompareOrdinal(password2, password3) != 0)
			{
				passwordBox2.Password=(string.Empty);
				passwordBox3.Password=(string.Empty);
				passwordBox2.Focus();
				this.ShowMessageBox(this.GetTranslationFor(SNPasswordsDontMatch));
				return;
			}
			this.TriggerChangePassword(password, password2);
		}
		private void TranslateDialogTexts()
		{
			using (IEnumerator<DialogResource> enumerator = this.dialogResources.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DialogResource current = enumerator.Current;
					DependencyObject? control = string.IsNullOrEmpty(current.Name) ? null : base.GetTemplateChild(current.Name);
					this.SetTranslatedText(current.Translation, control);
				}
			}
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
		private void ShowMessageBox(string text)
		{
			MessageBoxManager messageBoxManager = new MessageBoxManager(null);
			messageBoxManager.Show(text, "HMI", MessageBoxType.Information, DefaultTextCPOK, DefaultTextCPCancel);
		}
		private void TriggerChangePassword(string currentPassword, string newPassword)
		{
			if (this.ChangePasswordTriggered != null)
			{
				this.ChangePasswordTriggered.Invoke(currentPassword, newPassword);
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
				this.connector.Dispose();
				this.connector = null;
			}
		}
		private void UnregisterFromUIEvents()
		{
			Button? button = base.GetTemplateChild(ElementCPOKButton) as Button;
			Button? button2 = base.GetTemplateChild(ElementCPCancelButton) as Button;
			PasswordBox? passwordBox = base.GetTemplateChild(ElementCPCurrentPasswordPasswordBox) as PasswordBox;
			PasswordBox? passwordBox2 = base.GetTemplateChild(ElementCPNewPasswordPasswordBox) as PasswordBox;
			PasswordBox? passwordBox3 = base.GetTemplateChild(ElementCPConfirmPasswordPasswordBox) as PasswordBox;
			if (button != null)
			{
				button.KeyDown-=(new KeyEventHandler(this.ChangePasswordDialogHandleKeyDown));
			}
			if (button2 != null)
			{
				button2.KeyDown-=(new KeyEventHandler(this.ChangePasswordDialogHandleKeyDown));
			}
			if (passwordBox != null)
			{
				passwordBox.KeyDown-=(new KeyEventHandler(this.ChangePasswordDialogHandleKeyDown));
			}
			if (passwordBox2 != null)
			{
				passwordBox2.KeyDown-=(new KeyEventHandler(this.ChangePasswordDialogHandleKeyDown));
			}
			if (passwordBox3 != null)
			{
				passwordBox3.KeyDown-=(new KeyEventHandler(this.ChangePasswordDialogHandleKeyDown));
			}
		}
	}
}
