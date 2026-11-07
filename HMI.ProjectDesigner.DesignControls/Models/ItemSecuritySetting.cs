// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
namespace HMI.Infrastructure.Models
{
	public abstract class ItemSecuritySetting : CollectionItem
	{
		private string securityAuthorizationName;
		private bool isVisible = true;
		private SecurityDenyAccessBehavior securityDenyAccessBehavior;

		public event System.EventHandler<ValueChangedEventArgs<string>> SecurityAuthorizationNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> IsVisibleChanged;
		public event System.EventHandler<ValueChangedEventArgs<SecurityDenyAccessBehavior>> SecurityDenyAccessBehaviorChanged;

		[ProjectAuthorizationsBrowser, PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public string SecurityAuthorizationName
		{
			get
			{
				return this.securityAuthorizationName;
			}
			set
			{
				if (this.securityAuthorizationName != value)
				{
					base.RaisePropertyChanging<string>(() => this.SecurityAuthorizationName);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.securityAuthorizationName, value);
					this.securityAuthorizationName = value;
					this.SecurityAuthorizationNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SecurityAuthorizationName);
				}
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public bool IsVisible
		{
			get
			{
				return this.isVisible;
			}
			set
			{
				if (this.isVisible != value)
				{
					base.RaisePropertyChanging<bool>(() => this.IsVisible);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isVisible, value);
					this.isVisible = value;
					this.IsVisibleChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.IsVisible);
				}
			}
		}
		[PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public SecurityDenyAccessBehavior SecurityDenyAccessBehavior
		{
			get
			{
				return this.securityDenyAccessBehavior;
			}
			set
			{
				if (this.securityDenyAccessBehavior != value)
				{
					base.RaisePropertyChanging<SecurityDenyAccessBehavior>(() => this.SecurityDenyAccessBehavior);
					ValueChangedEventArgs<SecurityDenyAccessBehavior> e = new ValueChangedEventArgs<SecurityDenyAccessBehavior>(this.securityDenyAccessBehavior, value);
					this.securityDenyAccessBehavior = value;
					this.SecurityDenyAccessBehaviorChanged.Raise(this, e);
					base.RaisePropertyChanged<SecurityDenyAccessBehavior>(() => this.SecurityDenyAccessBehavior);
				}
			}
		}
		protected IContentReplacementService ContentReplacementService
		{
			get;
			private set;
		}
		public ItemSecuritySetting(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
			this.ContentReplacementService = container.Resolve<IContentReplacementService>();
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			ItemSecuritySetting itemSecuritySetting = target as ItemSecuritySetting;
			itemSecuritySetting.IsVisible = this.IsVisible;
			itemSecuritySetting.SecurityDenyAccessBehavior = this.SecurityDenyAccessBehavior;
			itemSecuritySetting.SecurityAuthorizationName = this.SecurityAuthorizationName;
		}
	}
}
