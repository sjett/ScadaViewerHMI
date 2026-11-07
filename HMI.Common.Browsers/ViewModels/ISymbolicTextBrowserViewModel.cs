// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public interface ISymbolicTextBrowserViewModel : INotifyPropertyChanged, IDisposable, IViewModel, IBrowserViewModel, ISelectorBrowserViewModel
	{
	}
}
