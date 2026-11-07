// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using HMI.Runtime.Core;
using HMI.RuntimeControls.General.Constants;
using HMI.RuntimeControls.General.UserLoginDialogs;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Dialogs;
using HMI.RuntimeControls.Labels;
namespace HMI.RuntimeControls.General
{
    /// <summary>

    /// </summary>
    [
        StyleTypedProperty(Property = "UserLoginDialogStyle", StyleTargetType = typeof(HMIUserLogin1DialogContent)), 
		StyleTypedProperty(Property = "UserPasswordChangeStyle", StyleTargetType = typeof(HMIUserLoginPasswordChangeContent)), 
		TemplateVisualState(Name = "Disabled", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Focused", GroupName = "FocusStates"), 
		TemplateVisualState(Name = "MouseOver", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Normal", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Pressed", GroupName = "CommonStates"), 
		TemplateVisualState(Name = "Unfocused", GroupName = "FocusStates")]
	public sealed class HMIUserLogin1 : Button, IDisposable
	{
		private const string SNLoginSuccessfully = "HMISN_Ctrl_HMIUserLogin_LoginSuccessfully";
		private const string SNLoginFailed = "HMISN_Ctrl_HMIUserLogin_LoginFailed";
		private const string SNLogoutSuccessfully = "HMISN_Ctrl_HMIUserLogin_LogoutSuccessfully";
		private const string SNLogoutFailed = "HMISN_Ctrl_HMIUserLogin_LogoutFailed";
		private const string SNPasswordChangeError = "HMISN_Ctrl_HMIUserLogin_PasswordChangeError";
		private const string SNPasswordChangeSuccess = "HMISN_Ctrl_HMIUserLogin_PasswordChangeSuccess";
		private const string SNLoginFailedInvalidCommandSyntax = "HMISN_Ctrl_HMIUserLogin_LoginFailed_InvalidCommandSyntax";
		private const string SNLoginFailedExpiredPassword = "HMISN_Ctrl_HMIUserLogin_LoginFailed_ExpiredPassword";
		private const string SNLoginFailedInternalServerError = "HMISN_Ctrl_HMIUserLogin_LoginFailed_InternalServerError";
		private const string SNLoginFailedMaxCountReached = "HMISN_Ctrl_HMIUserLogin_LoginFailed_MaxCountReached";
		private const string SNLoginFailedInvalidPassword = "HMISN_Ctrl_HMIUserLogin_LoginFailed_InvalidPassword";
		private const string SNLoginFailedUnknownUser = "HMISN_Ctrl_HMIUserLogin_LoginFailed_UnknownUser";
		private const string SNLoginFailedInvalidUsernameOrPassword = "HMISN_Ctrl_HMIUserLogin_LoginFailed_InvalidUsernameOrPassword";
		private const string SNNumberOfFailledLogonsExceddedUsernameBlocked = "HMISN_Ctrl_HMIUserLogin_NumberOfFailledLogonsExcedded_UsernameBlocked";
		private const string SNLoginFailedAlreadyLoggedOn = "HMISN_Ctrl_HMIUserLogin_LoginFailed_AlreadyLoggedOn";
		private const string SNLoginFailedCannotObtainCurrentLoggedInUserName = "HMISN_Ctrl_HMIUserLogin_LoginFailed_CannotObtainCurrentLoggedInUserName";
		private const string SNLoginFailedStopAutoLogin = "HMISN_Ctrl_HMIUserLogin_LoginFailed_StopAutoLogin";
		private const string SNLoginFailedAutoLoginWithoutMessages = "HMISN_Ctrl_HMIUserLogin_LoginFailed_AutoLoginWithoutMessages";
		private const string SNLoginFailedAutoLoginWithMessages = "HMISN_Ctrl_HMIUserLogin_LoginFailed_AutoLoginWithMessages";
		private const string SNPasswordChangeErrorInternalServerError = "HMISN_Ctrl_HMIUserLogin_PasswordChangeError_InternalServerError";
		private const string SNPasswordChangeErrorPasswordPolicyViolation = "HMISN_Ctrl_HMIUserLogin_PasswordChangeError_PasswordPolicyViolation";
		private const string SNPasswordChangeErrorPasswordsDontMatch = "HMISN_Ctrl_HMIUserLogin_PasswordChangeError_PasswordsDontMatch";
		private const string SNPasswordChangeErrorUnknownUser = "HMISN_Ctrl_HMIUserLogin_PasswordChangeError_UnknownUser";
		private const string SNPasswordChangeErrorInsufficientAuthorizations = "HMISN_Ctrl_HMIUserLogin_PasswordChangeError_InsufficientAuthorizations";
		private const string SNPasswordChangeErrorInvalidCommandSyntax = "HMISN_Ctrl_HMIUserLogin_PasswordChangeError_InvalidCommandSyntax";
		
		private const string DefaultTextLoginSuccessfully = "登录成功。";
		private const string DefaultTextLoginFailed = "登录失败。";
		private const string DefaultTextLogoutSuccessfully = "退出成功。";
		private const string DefaultTextLogoutFailed = "退出失败。";
		private const string DefaultTextPasswordChangeError = "无法修改密码。";
		private const string DefaultTextPasswordChangeSuccess = "密码修改成功。";
		private const string DefaultTextLoginFailedInvalidCommandSyntax = "命令语法无效，登录失败。";
		private const string DefaultTextLoginFailedExpiredPassword = "密码已过期，登录失败。";
		private const string DefaultTextLoginFailedInternalServerError = "服务器内部错误，登录失败。";
		private const string DefaultTextLoginFailedMaxCountReached = "已达到最大用户数，登录失败。";
		private const string DefaultTextLoginFailedInvalidPassword = "密码不正确，登录失败。";
		private const string DefaultTextLoginFailedAlreadyLoggedOn = "用户已在另一客户端登录，登录失败。";
		private const string DefaultTextLoginFailedUnknownUser = "未知用户，登录失败。";
		private const string DefaultTextLoginFailedInvalidUsernameOrPassword = "用户名或密码无效，登录失败。";
		private const string DefaultTextLoginFailedCannotObtainCurrentLoggedInUserName = "无法获取当前登录用户名。";
		private const string DefaultTextLoginFailedStopAutoLogin = "停止自动登录";
		private const string DefaultTextLoginFailedAutoLoginWithoutMessages = "静默自动登录";
		private const string DefaultTextLoginFailedAutoLoginWithMessages = "显示消息的自动登录";
		private const string DefaultTextPasswordChangeErrorInternalServerError = "服务器内部错误，密码修改失败。";
		private const string DefaultTextPasswordChangeErrorPasswordPolicyViolation = "新密码不符合当前密码策略，密码修改失败。";
		private const string DefaultTextPasswordChangeErrorPasswordsDontMatch = "确认密码不正确，密码修改失败。";
		private const string DefaultTextPasswordChangeErrorUnknownUser = "未知用户，密码修改失败。";
		private const string DefaultTextPasswordChangeErrorInsufficientAuthorizations = "权限不足，密码修改失败。";
		private const string DefaultTextPasswordChangeErrorInvalidCommandSyntax = "命令语法无效，密码修改失败。";
		private const string DefaultTextNumberOfFailledLogonsExceddedUsernameBlocked = "登录失败次数过多，用户已被锁定。";
		
		private readonly IList<DialogResource> resources;
		private HMIUserLoginPasswordChange changePasswordDialog;
		private HMIUserLogin1Dialog loginDialog;
		private MessageBoxManager messageBoxManager;
		private IHMIConnector connector;
		private bool isLoggedIn;
		private bool isExpiredPassword;
		private string userNameTryingToLoggin;
		private DispatcherTimer initialUserTimer;
		private string currentUser;
		private bool useInitialUser;
		private bool? isFirstUserContextChanged;
		private AutoLogin autoLoginAtStartup;
		private AutoLogin autoLoginAfterLogout;
		private bool userTryingToLoginIsDomainUser;
		private bool alwaysShowErrorMessages;
		private bool shouldShowLoginFailedErrorMessage;
        private bool canConnectWithDomainUser;
        private bool userActionStoppedAutologin;

        public static readonly DependencyProperty PopupModalityProperty = DependencyProperty.Register("PopupModality", typeof(DialogModality), typeof(HMIUserLogin1), new PropertyMetadata(DialogModality.Modeless, new PropertyChangedCallback(HMIUserLogin1.OnPopupModalityPropertyChanged)));
		public static readonly DependencyProperty ShowConfirmationProperty = DependencyProperty.Register("ShowConfirmation", typeof(bool), typeof(HMIUserLogin1), new PropertyMetadata(false, new PropertyChangedCallback(HMIUserLogin1.OnShowConfirmationPropertyChanged)));
		public static readonly DependencyProperty CanChangePasswordProperty = DependencyProperty.Register("CanChangePassword", typeof(bool), typeof(HMIUserLogin1), new PropertyMetadata(true, new PropertyChangedCallback(HMIUserLogin1.OnCanChangePasswordPropertyChanged)));
		public static readonly DependencyProperty UserLoginDialogStyleProperty = DependencyProperty.Register("UserLoginDialogStyle", typeof(Style), typeof(HMIUserLogin1), null);
		public static readonly DependencyProperty UserPasswordChangeStyleProperty = DependencyProperty.Register("UserPasswordChangeStyle", typeof(Style), typeof(HMIUserLogin1), null);
		
		[Category("HMI")]
		public DialogModality PopupModality
		{
			get
			{
				return (DialogModality)base.GetValue(HMIUserLogin1.PopupModalityProperty);
			}
			set
			{
				base.SetValue(HMIUserLogin1.PopupModalityProperty, value);
			}
		}
		[Category("HMI")]
		public bool ShowConfirmation
		{
			get
			{
				return (bool)base.GetValue(HMIUserLogin1.ShowConfirmationProperty);
			}
			set
			{
				base.SetValue(HMIUserLogin1.ShowConfirmationProperty, value);
			}
		}
		[Category("HMI")]
		public bool CanChangePassword
		{
			get
			{
				return (bool)base.GetValue(HMIUserLogin1.CanChangePasswordProperty);
			}
			set
			{
				base.SetValue(HMIUserLogin1.CanChangePasswordProperty, value);
			}
		}
		[Category("HMI")]
		public string InitialUserName
		{
			get;
			set;
		}
		[Category("HMI")]
		public string InitialUserPassword
		{
			get;
			set;
		}
		[Category("HMI")]
		public bool InitialUserIsDomainUser
		{
			get;
			set;
		}
		[Category("HMI")]
		public bool InitialUserRuntimeCheck
		{
			get;
			set;
		}
		[Category("HMI")]
		public AutoLogin AutoLoginAtStartup
		{
			get
			{
				return this.autoLoginAtStartup;
			}
			set
			{
				this.autoLoginAtStartup = value;
			}
		}
		[Category("HMI")]
		public AutoLogin AutoLoginAfterLogout
		{
			get
			{
				return this.autoLoginAfterLogout;
			}
			set
			{
				this.autoLoginAfterLogout = value;
			}
		}
		[Category("HMI")]
		public bool CanConnectWithDomainUser
		{
			get
			{
				return this.canConnectWithDomainUser;
			}
			set
			{
				this.canConnectWithDomainUser = value;
			}
		}
		public bool AlwaysShowErrorMessages
		{
			get
			{
				return this.alwaysShowErrorMessages;
			}
			set
			{
				this.alwaysShowErrorMessages = value;
			}
		}
		public Style UserLoginDialogStyle
		{
			get
			{
				return (Style)base.GetValue(HMIUserLogin1.UserLoginDialogStyleProperty);
			}
			set
			{
				base.SetValue(HMIUserLogin1.UserLoginDialogStyleProperty, value);
			}
		}
		public Style UserPasswordChangeStyle
		{
			get
			{
				return (Style)base.GetValue(HMIUserLogin1.UserPasswordChangeStyleProperty);
			}
			set
			{
				base.SetValue(HMIUserLogin1.UserPasswordChangeStyleProperty, value);
			}
		}
		private bool HasAutoLogin
		{
			get
			{
				return ((!this.isFirstUserContextChanged.HasValue || !this.isFirstUserContextChanged.Value) ? this.AutoLoginAfterLogout : this.AutoLoginAtStartup) != AutoLogin.NoAutoLogin;
			}
		}
		private static void OnPopupModalityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnShowConfirmationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnCanChangePasswordPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
		{
		}
		public HMIUserLogin1()
		{
			List<DialogResource> list = new List<DialogResource>();
			list.Add(new DialogResource(null, SNLoginSuccessfully, DefaultTextLoginSuccessfully));
			list.Add(new DialogResource(null, SNLoginFailed, DefaultTextLoginFailed));
			list.Add(new DialogResource(null, SNLogoutSuccessfully, DefaultTextLogoutSuccessfully));
			list.Add(new DialogResource(null, SNLogoutFailed, DefaultTextLogoutFailed));
			list.Add(new DialogResource(null, SNPasswordChangeError, DefaultTextPasswordChangeError));
			list.Add(new DialogResource(null, SNPasswordChangeSuccess, DefaultTextPasswordChangeSuccess));
			list.Add(new DialogResource(null, SNLoginFailedInvalidCommandSyntax, DefaultTextLoginFailedInvalidCommandSyntax));
			list.Add(new DialogResource(null, SNLoginFailedExpiredPassword, DefaultTextLoginFailedExpiredPassword));
			list.Add(new DialogResource(null, SNLoginFailedInternalServerError, DefaultTextLoginFailedInternalServerError));
			list.Add(new DialogResource(null, SNLoginFailedMaxCountReached, DefaultTextLoginFailedMaxCountReached));
			list.Add(new DialogResource(null, SNLoginFailedInvalidPassword, DefaultTextLoginFailedInvalidPassword));
			list.Add(new DialogResource(null, SNLoginFailedAlreadyLoggedOn, DefaultTextLoginFailedAlreadyLoggedOn));
			list.Add(new DialogResource(null, SNLoginFailedUnknownUser, DefaultTextLoginFailedUnknownUser));
			list.Add(new DialogResource(null, SNLoginFailedInvalidUsernameOrPassword, DefaultTextLoginFailedInvalidUsernameOrPassword));
			list.Add(new DialogResource(null, SNLoginFailedCannotObtainCurrentLoggedInUserName, DefaultTextLoginFailedCannotObtainCurrentLoggedInUserName));
			list.Add(new DialogResource(null, SNPasswordChangeErrorInternalServerError, DefaultTextPasswordChangeErrorInternalServerError));
			list.Add(new DialogResource(null, SNPasswordChangeErrorPasswordPolicyViolation, DefaultTextPasswordChangeErrorPasswordPolicyViolation));
			list.Add(new DialogResource(null, SNPasswordChangeErrorPasswordsDontMatch, DefaultTextPasswordChangeErrorPasswordsDontMatch));
			list.Add(new DialogResource(null, SNPasswordChangeErrorUnknownUser, DefaultTextPasswordChangeErrorUnknownUser));
			list.Add(new DialogResource(null, SNPasswordChangeErrorInsufficientAuthorizations, DefaultTextPasswordChangeErrorInsufficientAuthorizations));
			list.Add(new DialogResource(null, SNPasswordChangeErrorInvalidCommandSyntax, DefaultTextPasswordChangeErrorInvalidCommandSyntax));
			list.Add(new DialogResource(null, SNNumberOfFailledLogonsExceddedUsernameBlocked, DefaultTextNumberOfFailledLogonsExceddedUsernameBlocked));
			list.Add(new DialogResource(null, SNLoginFailedStopAutoLogin, DefaultTextLoginFailedStopAutoLogin));
			list.Add(new DialogResource(null, SNLoginFailedAutoLoginWithMessages, DefaultTextLoginFailedAutoLoginWithMessages));
			list.Add(new DialogResource(null, SNLoginFailedAutoLoginWithoutMessages, DefaultTextLoginFailedAutoLoginWithoutMessages));
			this.resources = list;
			this.isFirstUserContextChanged = default(bool?);
			this.shouldShowLoginFailedErrorMessage = true;
			this.canConnectWithDomainUser = true;
			
			base.DefaultStyleKey=(typeof(HMIUserLogin1));
			this.CreateInitialUserTimer();
			this.messageBoxManager = new MessageBoxManager(new EventHandler<DialogClosedEventArgs>(this.messageBox_DialogClosed));
		}
		private void CreateInitialUserTimer()
		{
			this.initialUserTimer = new DispatcherTimer();
			this.initialUserTimer.Interval=(TimeSpan.FromSeconds(30.0));
			this.initialUserTimer.Tick+=(new EventHandler(this.initialUserTimer_Tick));
		}
		~HMIUserLogin1()
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
			this.StopStoryboards();
			this.DisposeLoginDialog();
			this.DisposeChangePasswordDialog();
			this.DisposeMouseHandlers();
			this.messageBoxManager.Dispose();
			if (this.initialUserTimer != null)
			{
				this.initialUserTimer.Stop();
				this.initialUserTimer.Tick-=(new EventHandler(this.initialUserTimer_Tick));
			}
			if (this.connector == null)
			{
				return;
			}
			this.connector.UserContextChanged -= new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
			this.connector.Dispose();
		}
		private void DisposeLoginDialog()
		{
			if (this.loginDialog == null)
			{
				return;
			}
			this.loginDialog.ChangePasswordTriggered -= new Action(this.ShowChangePasswordDialog);
			this.loginDialog.LogoutTriggered -= new Action(this.Logout);
			this.loginDialog.LoginTriggered -= new Action<string, string, bool>(this.LoginTriggeredHandler);
			this.loginDialog.Close();
			this.loginDialog.Dispose();
		}
		private void DisposeChangePasswordDialog()
		{
			if (this.changePasswordDialog == null)
			{
				return;
			}
			this.changePasswordDialog.ChangePasswordTriggered -= new Action<string, string>(this.ChangePasswordTriggeredHandler);
			this.changePasswordDialog.Close();
			this.changePasswordDialog.Dispose();
		}
		private void DisposeMouseHandlers()
		{
			base.MouseEnter-=(new MouseEventHandler(this.HMIControl_MouseEnter));
			base.MouseLeave-=(new MouseEventHandler(this.HMIControl_MouseLeave));
		}
		private void OnChangePasswordResultHandler(Exception error, int[] results)
		{
			if (HMIUserLogin1.IsSuccesfull(error, results))
			{
				this.HandleSuccessfulPasswordChange();
				return;
			}
			this.HandleFailedPasswordChange(error, results);
		}
		private void OnLoginResultHandler(Exception error, int[] results)
		{
			this.HandleLoginResult(error, results);
			if (this.InitialUserRuntimeCheck)
			{
				this.initialUserTimer.Start();
			}
		}
		private void connector_UserContextChanged(string userName)
		{
			this.currentUser = userName;
			this.isFirstUserContextChanged = new bool?(!this.isFirstUserContextChanged.HasValue);
			if (this.isFirstUserContextChanged.Value)
			{
				this.HandleFirstUserContextChange();
				return;
			}
			this.HandleUserContextChange();
		}
		private void messageBox_DialogClosed(object sender, DialogClosedEventArgs e)
		{
			if (this.isExpiredPassword)
			{
				this.ShowChangePasswordDialog();
			}
		}
		private void initialUserTimer_Tick(object sender, EventArgs e)
		{
			this.initialUserTimer.Stop();
			if (string.CompareOrdinal(this.currentUser, this.InitialUserName) != 0)
			{
				this.LoginInitialUser();
			}
		}
		private void OnLanguageChangedHandler(string[] symbolicnames, string[] translationtexts)
		{
			for (int i = 0; i < symbolicnames.Length; i++)
			{
				string symbolicname = symbolicnames[i];
				string translation = translationtexts[i];
				DialogResource? dialogResource = this.resources.FirstOrDefault( (x) => x.SymbolicText == symbolicname);
				if (dialogResource != null)
				{
					dialogResource.Translation = translation;
				}
			}
			base.IsEnabled=(true);
		}
		private void ChangePasswordTriggeredHandler(string currentPassword, string newPassword)
		{
			this.ChangePassword(this.connector.GetUserName(), currentPassword, newPassword);
		}
		private void LoginTriggeredHandler(string userName, string password, bool isDomainUser)
		{
			this.userActionStoppedAutologin = true;
			this.Login(userName, password, isDomainUser, false);
		}
		private void HMIControl_MouseEnter(object sender, MouseEventArgs e)
		{
			base.Cursor=(Cursors.Hand);
		}
		private void HMIControl_MouseLeave(object sender, MouseEventArgs e)
		{
			base.Cursor=(Cursors.Arrow);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			bool isInDesignMode;
			if (!(isInDesignMode = DesignerProperties.GetIsInDesignMode(this)) && this.connector == null)
			{
				this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
				base.IsEnabled=(false);
				if (!this.CanConnectWithDomainUser)
				{
					this.InitialUserIsDomainUser = false;
				}
				this.connector.UserContextChanged += new OnUserContextChangedEventHandler(this.connector_UserContextChanged);
				this.HandleTextTranslations();
				this.RegisterMouseHandlers();
				return;
			}
		}
		protected override void OnClick()
		{
			base.OnClick();
			this.ShowLoginDialog();
		}
		private void HandleFailedPasswordChange(Exception error, int[] results)
		{
			string symbolicTextForErrorMessage = this.GetSymbolicTextForErrorMessage(error, results);
			this.messageBoxManager.Show(symbolicTextForErrorMessage, "HMI", MessageBoxType.Error, "OK", "Cancel");
		}
		private string GetSymbolicTextForErrorMessage(Exception error, int[] results)
		{
			if (error != null)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, error.Message), new object[0]);
				return SNPasswordChangeError;
			}
			return this.GetChangePasswordSymbolicTextFromErrorCode(results[0]);
		}
		private void HandleSuccessfulPasswordChange()
		{
			this.isExpiredPassword = false;
			this.userNameTryingToLoggin = string.Empty;
			this.ShowMessage(SNPasswordChangeSuccess, MessageBoxType.Information);
			this.CloseLoginDialog();
		}
		private void HandleTextTranslations()
		{
			try
			{
				string[] symbolicNames = Enumerable.ToArray<string>(Enumerable.Select<DialogResource, string>(this.resources, (DialogResource x) => x.SymbolicText));
				this.connector.RegisterLanguageChangedHandler(symbolicNames, new OnLanguageChangedDelegate(this.OnLanguageChangedHandler));
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
		}
		private void CloseLoginDialog()
		{
			if (this.loginDialog != null)
			{
				this.loginDialog.Close();
				this.loginDialog.Dispose();
				this.loginDialog = null;
			}
		}
		private void CloseChangePasswordDialog()
		{
			if (this.changePasswordDialog != null)
			{
				this.changePasswordDialog.Close();
				this.changePasswordDialog.Dispose();
				this.changePasswordDialog = null;
			}
		}
		private void ShowLoginDialog()
		{
			this.DisposeLoginDialog();
			this.loginDialog = new HMIUserLogin1Dialog(this.UserLoginDialogStyle, this.CanConnectWithDomainUser, this.CanChangePassword);
			this.loginDialog.ChangePasswordTriggered += new Action(this.ShowChangePasswordDialog);
			this.loginDialog.LogoutTriggered += new Action(this.Logout);
			this.loginDialog.LoginTriggered += new Action<string, string, bool>(this.LoginTriggeredHandler);
			if (this.PopupModality == DialogModality.Modal)
			{
				this.loginDialog.ShowDialog();
				return;
			}
			this.loginDialog.Show();
		}
		private void ShowChangePasswordDialog()
		{
			this.DisposeChangePasswordDialog();
			this.changePasswordDialog = new HMIUserLoginPasswordChange(this.UserPasswordChangeStyle, this.isExpiredPassword, this.userNameTryingToLoggin);
			this.changePasswordDialog.ChangePasswordTriggered += new Action<string, string>(this.ChangePasswordTriggeredHandler);
			this.changePasswordDialog.ShowDialog();
		}
		private void LoginInitialUser()
		{
			this.Login(this.InitialUserName, this.InitialUserPassword, this.InitialUserIsDomainUser, true);
		}
		private void Login(string userName, string password, bool isDomainUser, bool isInitialUserLogin = false)
		{
			try
			{
				this.TryLogin(userName, password, isDomainUser, isInitialUserLogin);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			this.CloseLoginDialog();
		}
		private void TryLogin(string userName, string password, bool isDomainUser, bool isInitialUserLogin = false)
		{
			if (string.IsNullOrEmpty(userName))
			{
				return;
			}
			this.useInitialUser = isInitialUserLogin;
			this.userNameTryingToLoggin = userName;
			this.userTryingToLoginIsDomainUser = isDomainUser;
			this.connector.OutputDebugText(HMIDebugLevel.Hint, string.Format("{0} Try to login as \"{1}\" as {2} user.", base.GetType().Name, userName, isDomainUser ? "domain" : "HMI"), new object[0]);
			this.connector.LoginUser(userName, password, isDomainUser, new OnResultDelegate(this.OnLoginResultHandler));
		}
		private void Logout()
		{
			try
			{
				this.useInitialUser = false;
				this.userActionStoppedAutologin = false;
				this.connector.LogoutUser();
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			this.CloseLoginDialog();
		}
		private void ShowMessage(string symbolicText, MessageBoxType messageBoxType = MessageBoxType.Information)
		{
			if (messageBoxType == MessageBoxType.Information && !this.ShowConfirmation)
			{
				return;
			}
			HMILabel1 translatableControl = this.GetTranslatableControl(symbolicText);
			this.messageBoxManager.ShowObject(translatableControl, "HMI", messageBoxType, "确定", "取消");
		}
		private void ChangePassword(string userName, string currentPassword, string newPassword)
		{
			try
			{
				this.connector.ChangePasswordAsUser(userName, currentPassword, newPassword, this.connector.GetIsDomainUser(), new OnResultDelegate(this.OnChangePasswordResultHandler));
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			this.CloseChangePasswordDialog();
		}
		private string GetChangePasswordSymbolicTextFromErrorCode(int errorCode)
		{
			string result;
			switch (errorCode)
			{
			case -4094:
				result = SNPasswordChangeErrorUnknownUser;
				return result;
			case -4093:
				result = SNPasswordChangeErrorInsufficientAuthorizations;
				return result;
			case -4091:
				result = SNPasswordChangeErrorPasswordsDontMatch;
				return result;
			case -4089:
				result = SNPasswordChangeErrorInternalServerError;
				return result;
			case -4086:
				result = SNPasswordChangeErrorInvalidCommandSyntax;
				return result;
			case -4085:
				result = SNPasswordChangeErrorPasswordPolicyViolation;
				return result;
			}
			result = SNPasswordChangeError;
			return result;
		}
		private string GetSymbolicTextFromErrorCode(int errorCode)
		{
			string result;
			switch (errorCode)
			{
			case -4094:
				result = SNLoginFailedInvalidUsernameOrPassword;
				return result;
			case -4093:
			case -4088:
			case -4085:
			case -4084:
			case -4083:
				break;
			case -4092:
				result = SNLoginFailedAlreadyLoggedOn;
				return result;
			case -4091:
				result = SNLoginFailedInvalidUsernameOrPassword;
				return result;
			case -4090:
				result = SNLoginFailedMaxCountReached;
				return result;
			case -4089:
				result = SNLoginFailedInternalServerError;
				return result;
			case -4087:
				result = SNLoginFailedExpiredPassword;
				return result;
			case -4086:
				result = SNLoginFailedInvalidCommandSyntax;
				return result;
			case -4082:
				result = SNNumberOfFailledLogonsExceddedUsernameBlocked;
				return result;
			default:
				if (errorCode == -404)
				{
					result = SNLoginFailedCannotObtainCurrentLoggedInUserName;
					return result;
				}
				break;
			}
			result = SNLoginFailed;
			return result;
		}
		private void HandleAutoLogin()
		{
			if (!this.InitialUserRuntimeCheck && this.isFirstUserContextChanged.HasValue)
			{
				this.HandleAutoLogin(this.isFirstUserContextChanged.Value ? this.AutoLoginAtStartup : this.AutoLoginAfterLogout);
				return;
			}
		}
		private void HandleAutoLogin(AutoLogin autoLogin)
		{
			if (this.userActionStoppedAutologin)
			{
				return;
			}
			switch (autoLogin)
			{
			case AutoLogin.NoAutoLogin:
				return;
			case AutoLogin.LoginCurrentWindowsUser:
				this.AttemptLoginAsCurrentWindowUser();
				return;
			case AutoLogin.LoginInitialUser:
				this.LoginInitialUser();
				return;
			}
		}
		private void AttemptLoginAsCurrentWindowUser()
		{
			this.connector.LoginCurrentWindowsUser(new OnResultDelegate(this.OnLoginResultHandler));
		}
		private void HandleUserIntendedLogout()
		{
			if (!this.useInitialUser && this.isLoggedIn && string.IsNullOrEmpty(this.currentUser))
			{
				this.ShowMessage(SNLogoutSuccessfully, MessageBoxType.Information);
				this.isLoggedIn = false;
				return;
			}
		}
		private void HandleUserContextChange()
		{
			this.HandleUserIntendedLogout();
			if (this.InitialUserRuntimeCheck)
			{
				this.LoginInitialUser();
				return;
			}
			if (string.IsNullOrEmpty(this.currentUser))
			{
				this.HandleAutoLogin(this.AutoLoginAfterLogout);
			}
		}
		private void HandleFirstUserContextChange()
		{
			if (this.InitialUserRuntimeCheck)
			{
				this.LoginInitialUser();
				return;
			}
			this.HandleAutoLogin(this.AutoLoginAtStartup);
		}
		private void HandleLoginResult(Exception error, int[] results)
		{
			int errorCode = (results == null) ? 0 : Enumerable.FirstOrDefault<int>(results);
			if (HMIUserLogin1.IsSuccesfull(error, results))
			{
				this.HandleLoginSuccess();
				return;
			}
			this.HandleLoginFailure(error, errorCode);
		}
		private static bool IsSuccesfull(Exception error, IEnumerable<int> results)
		{
			int num = (results == null) ? 0 : Enumerable.FirstOrDefault<int>(results);
			return error == null && num == 0;
		}
		private void HandleLoginSuccess()
		{
			this.userNameTryingToLoggin = string.Empty;
			this.isLoggedIn = true;
			if (!this.useInitialUser)
			{
				this.ShowMessage(SNLoginSuccessfully, MessageBoxType.Information);
			}
		}
		private void HandleLoginFailure(Exception error, int errorCode)
		{
			string symbolicTextFromErrorCode = this.GetSymbolicTextFromErrorCode(errorCode);
			string text = (error != null) ? error.Message : symbolicTextFromErrorCode;
			this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1} {2}", base.GetType().Name, text, symbolicTextFromErrorCode), new object[0]);
			this.SetPasswordExpired(errorCode);
			if (this.shouldShowLoginFailedErrorMessage)
			{
				if (this.CanShowErrorMessage())
				{
					this.ShowErrorDialog(symbolicTextFromErrorCode);
					return;
				}
			}
			else
			{
				this.HandleAutoLogin();
			}
		}
		private void ShowErrorDialog(string text)
		{
			if (this.HasAutoLogin)
			{
				this.ShowAutoLoginErrorMessageBox(text);
				return;
			}
			this.ShowMessage(text, MessageBoxType.Error);
		}
		private void ShowAutoLoginErrorMessageBox(string text)
		{
			MultiActionMessageBoxControl multiActionMessageBoxControl = new MultiActionMessageBoxControl();
			multiActionMessageBoxControl.Content=(this.GetTranslatableControl(text));
			multiActionMessageBoxControl.MessageBoxType = MessageBoxType.Error;
			multiActionMessageBoxControl.HeaderText = "HMI";
			MultiActionMessageBoxControl multiActionMessageBoxControl2 = multiActionMessageBoxControl;
			multiActionMessageBoxControl2.ButtonActions.Add(new CustomUserAction(this.GetTranslatableControl(SNLoginFailedStopAutoLogin), new UserInteractionHandler(this.StopAutoLogin), null));
			multiActionMessageBoxControl2.ButtonActions.Add(new CustomUserAction(this.GetTranslatableControl(SNLoginFailedAutoLoginWithoutMessages), new UserInteractionHandler(this.AutoLoginWithNoMessage), null));
			multiActionMessageBoxControl2.ButtonActions.Add(new CustomUserAction(this.GetTranslatableControl(SNLoginFailedAutoLoginWithMessages), new UserInteractionHandler(this.AutoLoginWithMessage), null));
			if (this.PopupModality == DialogModality.Modal)
			{
				multiActionMessageBoxControl2.ShowDialog();
				return;
			}
			multiActionMessageBoxControl2.Show();
		}
		private HMILabel1 GetTranslatableControl(string text)
		{
			HMILabel1 wFLabel = new HMILabel1();
			wFLabel.SymbolicText = text;
            wFLabel.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
			return wFLabel;
		}
		private void StopAutoLogin(object state)
		{
			if (this.isFirstUserContextChanged.HasValue && this.isFirstUserContextChanged.Value)
			{
				this.AutoLoginAtStartup = AutoLogin.NoAutoLogin;
				return;
			}
			this.AutoLoginAfterLogout = AutoLogin.NoAutoLogin;
		}
		private void AutoLoginWithMessage(object state)
		{
			this.shouldShowLoginFailedErrorMessage = true;
			this.HandleAutoLogin();
		}
		private void AutoLoginWithNoMessage(object state)
		{
			this.shouldShowLoginFailedErrorMessage = false;
			this.HandleAutoLogin();
		}
		private void SetPasswordExpired(int errorCode)
		{
			this.isExpiredPassword = (errorCode == -4087 && this.CanChangePassword);
		}
		private bool CanShowErrorMessage()
		{
			return !this.useInitialUser || this.AlwaysShowErrorMessages;
		}
		private void RegisterMouseHandlers()
		{
			base.MouseEnter+=(new MouseEventHandler(this.HMIControl_MouseEnter));
			base.MouseLeave+=(new MouseEventHandler(this.HMIControl_MouseLeave));
		}
	}
}
