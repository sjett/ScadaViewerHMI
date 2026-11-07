// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Infrastructure.Models
{
	public abstract class ButtonSettings : ItemSecuritySetting
	{
		protected abstract string NamespacePrefix
		{
			get;
		}
		protected abstract string ItemClassName
		{
			get;
		}
		public ButtonSettings(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
		}
		public override string GetXaml()
		{
			string text = string.Format("<{0}:{1} SecurityAuthorizationName=\"[SecurityAuthorizationName]\" Visibility=\"[IsVisible]\" SecurityDenyAccessBehaviour=\"[SecurityDenyAccessBehavior]\"></{0}:{1}>", this.NamespacePrefix, this.ItemClassName);
			return base.ContentReplacementService.ReplaceStringContent(text, this);
		}
	}
}
