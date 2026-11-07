// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.General.ViewModels
{
	public class HMIUserLogin1ViewModel : CommonControlViewModel
	{
		private DialogModality popupModality = DialogModality.Modeless;
		private bool showConfirmation;
		private bool initialUserIsDomainUser;
		private bool canConnectWithDomainUser = true;
		private AutoLoginType autoLoginAtStartup = AutoLoginType.NoAutoLogin;
		private AutoLoginType autoLoginAfterLogout = AutoLoginType.NoAutoLogin;
		private bool alwaysShowErrorMessages;
		private string initialUserName;
		private string initialUserPassword;
		private bool canChangePassword = true;
		private bool initialUserRuntimeCheck;
		public event System.EventHandler<ValueChangedEventArgs<DialogModality>> PopupModalityChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> ShowConfirmationChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> InitialUserIsDomainUserChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> CanConnectWithDomainUserChanged;
		public event System.EventHandler<ValueChangedEventArgs<AutoLoginType>> AutoLoginAtStartupChanged;
		public event System.EventHandler<ValueChangedEventArgs<AutoLoginType>> AutoLoginAfterLogoutChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> AlwaysShowErrorMessagesChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> InitialUserNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> InitialUserPasswordChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> CanChangePasswordChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> InitialUserRuntimeCheckChanged;
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(150.0, 50.0);
			}
		}
		[Browsable(true), PropertyCategory("Popup", typeof(StandardPropertyCategories))]
		public DialogModality PopupModality
		{
			get
			{
				return this.popupModality;
			}
			set
			{
				if (this.popupModality != value)
				{
					ValueChangedEventArgs<DialogModality> e = new ValueChangedEventArgs<DialogModality>(this.popupModality, value);
					base.RaisePropertyChanging<DialogModality>(() => this.PopupModality);
					this.popupModality = value;
					this.PopupModalityChanged.Raise(this, e);
					base.RaisePropertyChanged<DialogModality>(() => this.PopupModality);
				}
			}
		}
		[Browsable(true), PropertyCategory("Popup", typeof(StandardPropertyCategories))]
		public bool ShowConfirmation
		{
			get
			{
				return this.showConfirmation;
			}
			set
			{
				if (this.showConfirmation != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.showConfirmation, value);
					base.RaisePropertyChanging<bool>(() => this.ShowConfirmation);
					this.showConfirmation = value;
					this.ShowConfirmationChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.ShowConfirmation);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public bool InitialUserIsDomainUser
		{
			get
			{
				return this.initialUserIsDomainUser;
			}
			set
			{
				if (this.initialUserIsDomainUser != value)
				{
					base.RaisePropertyChanging<bool>(() => this.InitialUserIsDomainUser);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.initialUserIsDomainUser, value);
					this.initialUserIsDomainUser = value;
					this.InitialUserIsDomainUserChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.InitialUserIsDomainUser);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public bool CanConnectWithDomainUser
		{
			get
			{
				return this.canConnectWithDomainUser;
			}
			set
			{
				if (this.canConnectWithDomainUser != value)
				{
					base.RaisePropertyChanging<bool>(() => this.CanConnectWithDomainUser);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.canConnectWithDomainUser, value);
					this.canConnectWithDomainUser = value;
					this.CanConnectWithDomainUserChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.CanConnectWithDomainUser);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public AutoLoginType AutoLoginAtStartup
		{
			get
			{
				return this.autoLoginAtStartup;
			}
			set
			{
				if (this.autoLoginAtStartup != value)
				{
					base.RaisePropertyChanging<AutoLoginType>(() => this.AutoLoginAtStartup);
					ValueChangedEventArgs<AutoLoginType> e = new ValueChangedEventArgs<AutoLoginType>(this.autoLoginAtStartup, value);
					this.autoLoginAtStartup = value;
					this.AutoLoginAtStartupChanged.Raise(this, e);
					base.RaisePropertyChanged<AutoLoginType>(() => this.AutoLoginAtStartup);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public AutoLoginType AutoLoginAfterLogout
		{
			get
			{
				return this.autoLoginAfterLogout;
			}
			set
			{
				if (this.autoLoginAfterLogout != value)
				{
					base.RaisePropertyChanging<AutoLoginType>(() => this.AutoLoginAfterLogout);
					ValueChangedEventArgs<AutoLoginType> e = new ValueChangedEventArgs<AutoLoginType>(this.autoLoginAfterLogout, value);
					this.autoLoginAfterLogout = value;
					this.AutoLoginAfterLogoutChanged.Raise(this, e);
					base.RaisePropertyChanged<AutoLoginType>(() => this.AutoLoginAfterLogout);
				}
			}
		}
		[PropertyCategory("Popup", typeof(StandardPropertyCategories))]
		public bool AlwaysShowErrorMessages
		{
			get
			{
				return this.alwaysShowErrorMessages;
			}
			set
			{
				if (this.alwaysShowErrorMessages != value)
				{
					base.RaisePropertyChanging<bool>(() => this.AlwaysShowErrorMessages);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.alwaysShowErrorMessages, value);
					this.alwaysShowErrorMessages = value;
					this.AlwaysShowErrorMessagesChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.AlwaysShowErrorMessages);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public string InitialUserName
		{
			get
			{
				return this.initialUserName;
			}
			set
			{
				if (this.initialUserName != value)
				{
					base.RaisePropertyChanging<string>(() => this.InitialUserName);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.initialUserName, value);
					this.initialUserName = value;
					this.InitialUserNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.InitialUserName);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public string InitialUserPassword
		{
			get
			{
				return this.initialUserPassword;
			}
			set
			{
				if (this.initialUserPassword != value)
				{
					base.RaisePropertyChanging<string>(() => this.InitialUserPassword);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.initialUserPassword, value);
					this.initialUserPassword = value;
					this.InitialUserPasswordChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.InitialUserPassword);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public bool CanChangePassword
		{
			get
			{
				return this.canChangePassword;
			}
			set
			{
				if (this.canChangePassword != value)
				{
					base.RaisePropertyChanging<bool>(() => this.CanChangePassword);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.canChangePassword, value);
					this.canChangePassword = value;
					this.CanChangePasswordChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.CanChangePassword);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public bool InitialUserRuntimeCheck
		{
			get
			{
				return this.initialUserRuntimeCheck;
			}
			set
			{
				if (this.initialUserRuntimeCheck != value)
				{
					base.RaisePropertyChanging<bool>(() => this.InitialUserRuntimeCheck);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.initialUserRuntimeCheck, value);
					this.initialUserRuntimeCheck = value;
					this.InitialUserRuntimeCheckChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.InitialUserRuntimeCheck);
				}
			}
		}
		public HMIUserLogin1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIUserLogin1";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIGeneral=\"clr-namespace:HMI.RuntimeControls.General;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIGeneral;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("PopupModality", this.PopupModality);
			base.AddXamlAttribute("ShowConfirmation", this.ShowConfirmation);
			base.AddXamlAttribute("InitialUserIsDomainUser", this.InitialUserIsDomainUser);
			base.AddXamlAttribute("CanConnectWithDomainUser", this.CanConnectWithDomainUser);
			base.AddXamlAttribute("AutoLoginAtStartup", this.AutoLoginAtStartup);
			base.AddXamlAttribute("AutoLoginAfterLogout", this.AutoLoginAfterLogout);
			base.AddXamlAttribute("AlwaysShowErrorMessages", this.AlwaysShowErrorMessages);
			base.AddXamlAttribute("InitialUserName", this.InitialUserName);
			base.AddXamlAttribute("InitialUserPassword", this.InitialUserPassword);
			base.AddXamlAttribute("CanChangePassword", this.CanChangePassword);
			base.AddXamlAttribute("InitialUserRuntimeCheck", this.InitialUserRuntimeCheck);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
