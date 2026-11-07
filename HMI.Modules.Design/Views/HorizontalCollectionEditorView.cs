// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics.CodeAnalysis;
using HMI.Common.Browsers.Views;
using HMI.Modules.Design.ViewModels.Browsers;
using HMI.Modules.Design.Views.Browsers;
namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage]
	public class HorizontalCollectionEditorView : CollectionEditorView
	{
		protected override BrowserWindow CreateCollectionBrowser(ICollectionBrowserViewModel viewModel)
		{
			return new HorizontalCollectionBrowser(viewModel);
		}
		protected override ICollectionBrowserViewModel CreateCollectionBrowserViewModel()
		{
			return base.ServiceLocator.Resolve<IHorizontalCollectionBrowserViewModel>();
		}
	}
}
