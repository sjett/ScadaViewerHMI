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
	public class HMIUserLogin2ViewModel : CommonControlViewModel
	{
		private string password;
		private UserPrefixType userPrefix;
		public event System.EventHandler<ValueChangedEventArgs<string>> PasswordChanged;
		public event System.EventHandler<ValueChangedEventArgs<UserPrefixType>> UserPrefixChanged;
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(100.0, 25.0);
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories))]
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				if (this.password != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.password, value);
					base.RaisePropertyChanging<string>(() => this.Password);
					this.password = value;
					this.PasswordChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.Password);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories))]
		public UserPrefixType UserPrefix
		{
			get
			{
				return this.userPrefix;
			}
			set
			{
				if (this.userPrefix != value)
				{
					ValueChangedEventArgs<UserPrefixType> e = new ValueChangedEventArgs<UserPrefixType>(this.userPrefix, value);
					base.RaisePropertyChanging<UserPrefixType>(() => this.UserPrefix);
					this.userPrefix = value;
					this.UserPrefixChanged.Raise(this, e);
					base.RaisePropertyChanged<UserPrefixType>(() => this.UserPrefix);
				}
			}
		}
		public HMIUserLogin2ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIUserLogin2";
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
			base.AddXamlAttribute("Password", this.Password);
			base.AddXamlAttribute("UserPrefix", this.UserPrefix);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
