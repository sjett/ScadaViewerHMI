// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.ViewModels;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public interface IVerticalCollectionBrowserViewModel : IBrowserViewModel, IDisposable, IViewModel, INotifyPropertyChanged, ICollectionBrowserViewModel, IPropertyEditorViewModel
	{
	}
}
