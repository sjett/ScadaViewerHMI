// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;

namespace HMI.ProjectDesigner.DesignControls.UserManagement.ViewModels
{
	public class HMIUserManagerViewModel : CommonControlViewModel
	{
		private bool editPasswordPolicyIsVisible = true;
		private bool editUserDefaultSettingsIsVisible = true;
		private System.Windows.Media.Brush domainUserGridRowBackground;
		private System.Windows.Media.Brush domainUserGridRowForeground = (System.Windows.Media.Brush)new System.Windows.Media.SolidColorBrush(System.Windows.Media.Color.FromArgb(255, 0, 0, 255)).GetAsFrozen();
		
		public event System.EventHandler<ValueChangedEventArgs<bool>> EditPasswordPolicyIsVisibleChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> EditUserDefaultSettingsIsVisibleChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> DomainUserGridRowBackgroundChanged;
		public event System.EventHandler<ValueChangedEventArgs<System.Windows.Media.Brush>> DomainUserGridRowForegroundChanged;

		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool EditPasswordPolicyIsVisible
		{
			get
			{
				return this.editPasswordPolicyIsVisible;
			}
			set
			{
				if (this.editPasswordPolicyIsVisible != value)
				{
					base.RaisePropertyChanging<bool>(() => this.EditPasswordPolicyIsVisible);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.editPasswordPolicyIsVisible, value);
					this.editPasswordPolicyIsVisible = value;
					this.EditPasswordPolicyIsVisibleChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.EditPasswordPolicyIsVisible);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool EditUserDefaultSettingsIsVisible
		{
			get
			{
				return this.editUserDefaultSettingsIsVisible;
			}
			set
			{
				if (this.editUserDefaultSettingsIsVisible != value)
				{
					base.RaisePropertyChanging<bool>(() => this.EditUserDefaultSettingsIsVisible);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.editUserDefaultSettingsIsVisible, value);
					this.editUserDefaultSettingsIsVisible = value;
					this.EditUserDefaultSettingsIsVisibleChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.EditUserDefaultSettingsIsVisible);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush DomainUserGridRowBackground
		{
			get
			{
				return this.domainUserGridRowBackground;
			}
			set
			{
				if (this.domainUserGridRowBackground != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.DomainUserGridRowBackground);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.domainUserGridRowBackground, value);
					this.domainUserGridRowBackground = value;
					this.DomainUserGridRowBackgroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.DomainUserGridRowBackground);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public System.Windows.Media.Brush DomainUserGridRowForeground
		{
			get
			{
				return this.domainUserGridRowForeground;
			}
			set
			{
				if (this.domainUserGridRowForeground != value)
				{
					base.RaisePropertyChanging<System.Windows.Media.Brush>(() => this.DomainUserGridRowForeground);
					ValueChangedEventArgs<System.Windows.Media.Brush> e = new ValueChangedEventArgs<System.Windows.Media.Brush>(this.domainUserGridRowForeground, value);
					this.domainUserGridRowForeground = value;
					this.DomainUserGridRowForegroundChanged.Raise(this, e);
					base.RaisePropertyChanged<System.Windows.Media.Brush>(() => this.DomainUserGridRowForeground);
				}
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(500.0, 500.0);
			}
		}
		public HMIUserManagerViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIUserManager";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMIUserManagement=\"clr-namespace:HMI.RuntimeControls.UserManagement;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIUserManagement;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("EditPasswordPolicyIsVisible", this.EditPasswordPolicyIsVisible);
			base.AddXamlAttribute("EditUserDefaultSettingsIsVisible", this.EditUserDefaultSettingsIsVisible);
			base.AddXamlAttribute("DomainUserGridRowBackground", this.DomainUserGridRowBackground);
			base.AddXamlAttribute("DomainUserGridRowForeground", this.DomainUserGridRowForeground);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
