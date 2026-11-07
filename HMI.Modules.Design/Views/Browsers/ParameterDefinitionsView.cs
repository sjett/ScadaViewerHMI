// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using System.Windows;
using Telerik.Windows.Controls;
using HMI.Common.Browsers.Views;
using HMI.Common.Presentation.Views;
using HMI.Modules.Design.ViewModels.Browsers;
namespace HMI.Modules.Design.Views.Browsers
{
	[ExcludeFromCodeCoverage]
	public class ParameterDefinitionsView : BrowserView<ITemplateBrowserPageViewModel>
	{
		static ParameterDefinitionsView()
		{
			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ParameterDefinitionsView), new FrameworkPropertyMetadata(typeof(ParameterDefinitionsView)));
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
            RadGridView itemsGrid = this.FindTemplateChild<RadGridView>("uxParameterDefinitions");
			base.HookGridActionHandlers(itemsGrid);
			base.InitializeGridSettings(itemsGrid, "ParameterDefinitionsGrid");
		}
	}
}
