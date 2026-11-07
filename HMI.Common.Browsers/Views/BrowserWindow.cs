// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Presentation;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using Telerik.Windows.Controls;
namespace HMI.Common.Browsers.Views
{
    public abstract class BrowserWindow : RadWindow, IDisposable
    {
        private bool isDisposed;
        private IBrowserViewModel ViewModel
        {
            get
            {
                return base.DataContext as IBrowserViewModel;
            }
            set
            {
                base.DataContext = value;
            }
        }
        public BrowserWindow()
        {
            base.Owner = UserInterface.GetActiveWindow();
            base.ResizeMode = ResizeMode.CanResizeWithGrip;
            base.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        public BrowserWindow(IBrowserViewModel viewModel)
            : this()
        {
            Contract.Requires<ArgumentNullException>(viewModel != null, "viewModel");
            this.ViewModel = viewModel;
            this.SubscribeCloseWithDialogResultOnLayoutUpdated();
        }
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void SubscribeCloseWithDialogResultOnLayoutUpdated()
        {
            Observable
                .FromEventPattern(
                    addHandler => base.LayoutUpdated += addHandler,
                    removeHandler => base.LayoutUpdated -= removeHandler)
                .TakeWhile(e => !this.isDisposed)
                .Take(1)
                .Subscribe(equals => this.SubscribeCloseWithDialogResult());
        }
        private void SubscribeCloseWithDialogResult()
        {
            Observable
                .FromEventPattern<DialogClosedEventArgs>(
                    addHandler => this.ViewModel.Closed += addHandler,
                    removeHandler => this.ViewModel.Closed -= removeHandler)
                .TakeWhile((EventPattern<DialogClosedEventArgs> pattern) => !this.isDisposed)
                .Take(1)
                .Subscribe(delegate(EventPattern<DialogClosedEventArgs> pattern)
                {
                    this.CloseWithDialogResult(pattern.EventArgs);
                });
        }
        private void CloseWithDialogResult(DialogClosedEventArgs e)
        {
            base.DialogResult = e.DialogResult;
            base.Close();
        }
        protected override void OnClosed()
        {
            base.OnClosed();
            this.Dispose();
        }
        protected virtual void DisposeManagedResources()
        {
            if (this.ViewModel != null)
            {
                this.ViewModel.Dispose();
            }
        }
        protected virtual void DisposeUnmanagedResources()
        {
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                if (disposing)
                {
                    this.DisposeManagedResources();
                }
                this.DisposeUnmanagedResources();
            }
            this.isDisposed = true;
        }
    }
}
