// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.ViewModels;
using System.ComponentModel;
using System.Windows.Input;

namespace HMI.Common.Browsers.ViewModels
{
	public interface IBrowserViewModel : INotifyPropertyChanged, IDisposable, IViewModel
	{
		event EventHandler<DialogClosedEventArgs> Closed;
		event EventHandler<ExceptionEventArgs> Error;
		ICommand OKCommand
		{
			get;
		}
		ICommand CancelCommand
		{
			get;
		}
	}
}
