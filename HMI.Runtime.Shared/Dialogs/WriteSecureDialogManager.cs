// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Dialogs
{
    using System;

    public class WriteSecureDialogManager
    {
        private HMIWriteSecureDialog dialog;
        private EventHandler<DialogClosedEventArgs> dialogClosedCallback;
        private bool isDisposed;

        public WriteSecureDialogManager(EventHandler<DialogClosedEventArgs> dialogClosedCallback)
        {
            this.dialogClosedCallback = dialogClosedCallback;
        }

        private void AttachDialogClosedHandler()
        {
            if (dialogClosedCallback != null)
            {
                dialog.DialogClosed += dialogClosedCallback;
            }
        }

        private void CloseAndDisposeDialog()
        {
            if (dialog != null)
            {
                RemoveDialogClosedHandler();
                dialog.Close();
                dialog.Dispose();
            }
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                DisposeManagedResources();
            }
            isDisposed = true;
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeManagedResources()
        {
            CloseAndDisposeDialog();
        }

        private void RemoveDialogClosedHandler()
        {
            if (dialogClosedCallback != null)
            {
                dialog.DialogClosed -= dialogClosedCallback;
            }
        }

        public void Show()
        {
            dialog = new HMIWriteSecureDialog();
            AttachDialogClosedHandler();
            dialog.ShowDialog();
        }

        public string Password
        {
            get
            {
                if (dialog == null)
                {
                    return string.Empty;
                }
                return dialog.Password;
            }
        }
    }
}

