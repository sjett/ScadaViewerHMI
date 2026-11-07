// SPDX-License-Identifier: GPL-3.0-only
using System.ComponentModel;

namespace HMI.Common.Presentation.ViewModels
{
    public interface IDisposableViewModel : IViewModel, IDisposable
    {
    }

    public class DisposableViewModel : ViewModel, IDisposableViewModel
    {
        private bool isDisposed;
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void DisposeManagedResources()
        {
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
