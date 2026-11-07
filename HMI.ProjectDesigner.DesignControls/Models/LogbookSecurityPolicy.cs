// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Infrastructure.Models
{
	public class LogbookSecurityPolicy : ItemSecuritySetting
	{
		private const string @namespace = "HMILogbook";
		private const string @class = "LogbookSecurityPolicy";
		private const string contentNamespace = "HMILogbook";
		private const string contentClass = "SecurityPolicy";
		private LogbookSecuredButtons securedButton;

		public event System.EventHandler<ValueChangedEventArgs<LogbookSecuredButtons>> ButtonChanged;

		[Browsable(false)]
		public LogbookSecuredButtons SecuredButton
		{
			get
			{
				return this.securedButton;
			}
			set
			{
				if (this.securedButton != value)
				{
					base.RaisePropertyChanging<LogbookSecuredButtons>(() => this.SecuredButton);
					ValueChangedEventArgs<LogbookSecuredButtons> e = new ValueChangedEventArgs<LogbookSecuredButtons>(this.securedButton, value);
					this.securedButton = value;
					this.ButtonChanged.Raise(this, e);
					base.RaisePropertyChanged<LogbookSecuredButtons>(() => this.SecuredButton);
					base.UpdateEditorDisplayText();
				}
			}
		}
		public LogbookSecurityPolicy(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
		}
		public LogbookSecurityPolicy(IServiceProvider container, IExplorerNodeViewModel parentNode, LogbookSecuredButtons button) : base(container, parentNode)
		{
			this.SecuredButton = button;
		}
		public override string GetXaml()
		{
			string text = this.SecuredButton.ToString();
			string contentXaml = this.GetContentXaml();
			return string.Format("<{0}:{1}.{2}>\r\t{3}\r</{0}:{1}.{2}>", new object[]
			{
				"HMILogbook",
				"LogbookSecurityPolicy",
				text,
				contentXaml
			});
		}
		public string GetContentXaml()
		{
			return string.Format("<{0}:{1} Visibility=\"{2}\" SecurityAuthorizationName=\"{3}\" SecurityDenyAccessBehaviour=\"{4}\" />", new object[]
			{
				"HMILogbook",
				"SecurityPolicy",
				base.IsVisible,
				base.SecurityAuthorizationName,
				base.SecurityDenyAccessBehavior
			});
		}
		public override string ToString()
		{
			return this.SecuredButton.ToString();
		}
		public override ICollectionItem CreateClone()
		{
			return new LogbookSecurityPolicy(base.Services, base.Parent);
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			base.DeepCopyFields(target);
			LogbookSecurityPolicy logbookSecurityPolicy = target as LogbookSecurityPolicy;
			logbookSecurityPolicy.SecuredButton = this.SecuredButton;
		}
	}
}
