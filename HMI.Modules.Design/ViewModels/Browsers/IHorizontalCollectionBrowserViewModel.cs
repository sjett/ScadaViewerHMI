// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Infrastructure.ViewModels;

namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface IHorizontalCollectionBrowserViewModel : IBrowserViewModel,  ICollectionBrowserViewModel
	{
		IHorizontalPropertyEditorViewModel HorizontalPropertyEditorViewModel
		{
			get;
		}
	}
}
