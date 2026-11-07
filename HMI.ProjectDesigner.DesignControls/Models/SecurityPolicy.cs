// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Models
{
	public class SecurityPolicy : ItemSecuritySetting
	{
		private AlarmViewerSecuredAction action;

		public event System.EventHandler<ValueChangedEventArgs<AlarmViewerSecuredAction>> ActionChanged;

		[Browsable(false)]
		public AlarmViewerSecuredAction Action
		{
			get
			{
				return this.action;
			}
			set
			{
				if (this.action != value)
				{
					base.RaisePropertyChanging<AlarmViewerSecuredAction>(() => this.Action);
					ValueChangedEventArgs<AlarmViewerSecuredAction> e = new ValueChangedEventArgs<AlarmViewerSecuredAction>(this.action, value);
					this.action = value;
					this.ActionChanged.Raise(this, e);
					base.RaisePropertyChanged<AlarmViewerSecuredAction>(() => this.Action);
					base.UpdateEditorDisplayText();
				}
			}
		}
		public SecurityPolicy(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
		}
		public SecurityPolicy(IServiceProvider container, IExplorerNodeViewModel parentNode, AlarmViewerSecuredAction action) : base(container, parentNode)
		{
			this.action = action;
		}
		public override ICollectionItem CreateClone()
		{
			return new SecurityPolicy(base.Services, base.Parent);
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			base.DeepCopyFields(target);
			SecurityPolicy securityPolicy = target as SecurityPolicy;
			securityPolicy.Action = this.Action;
		}
		public override string GetXaml()
		{
			string securityPolicyXamlTemplate = this.GetSecurityPolicyXamlTemplate();
			string text = string.Format("<HMIAlarms:HMIAlarmViewerSecurityPolicies.{0}>{1}</HMIAlarms:HMIAlarmViewerSecurityPolicies.{0}>", this.Action, securityPolicyXamlTemplate);
			return base.ContentReplacementService.ReplaceStringContent(text, this);
		}
		private string GetSecurityPolicyXamlTemplate()
		{
			return string.Format("<HMIShared:SecurityPolicy SecurityAuthorizationName=\"[SecurityAuthorizationName]\" Visibility=\"[IsVisible]\" SecurityDenyAccessBehaviour=\"[SecurityDenyAccessBehavior]\"></HMIShared:SecurityPolicy>", new object[0]);
		}
		public override string ToString()
		{
			return this.Action.ToString();
		}
	}
}
