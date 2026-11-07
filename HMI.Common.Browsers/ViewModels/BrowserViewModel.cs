// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.ViewModels;
using System.Windows.Input;

namespace HMI.Common.Browsers.ViewModels
{
	public abstract class BrowserViewModel : DisposableViewModel, IBrowserViewModel
	{
		public event EventHandler<DialogClosedEventArgs>? Closed;

		public event EventHandler<ExceptionEventArgs>? Error;

		public ICommand OKCommand
		{
			get;
			private set;
		}
		public ICommand CancelCommand
		{
			get;
			private set;
		}

		protected BrowserViewModel()
		{
            OKCommand = new DelegateCommand(new Action(ExecuteOKCommand), new Func<bool>(CanExecuteOKCommand));
            CancelCommand = new DelegateCommand(new Action(ExecuteCancelCommand), new Func<bool>(CanExecuteCancelCommand));
		}
		protected virtual bool CanExecuteOKCommand()
		{
			return true;
		}
		protected virtual void ExecuteOKCommand()
		{
			if (!CanExecuteOKCommand())
			{
				return;
			}
            Closed?.Invoke(this, new DialogClosedEventArgs(new bool?(true)));
		}
		protected virtual bool CanExecuteCancelCommand()
		{
			return true;
		}
		protected virtual void ExecuteCancelCommand()
		{
			if (!CanExecuteCancelCommand())
			{
				return;
			}
            Closed?.Invoke(this, new DialogClosedEventArgs(new bool?(false)));
		}
		protected internal void OnError(Exception ex)
		{
            Error?.Invoke(this, new ExceptionEventArgs(ex));
		}
		protected internal void OnCompleted(bool? dialogResult)
		{
            Closed?.Invoke(this, new DialogClosedEventArgs(dialogResult));
		}
	}
}
