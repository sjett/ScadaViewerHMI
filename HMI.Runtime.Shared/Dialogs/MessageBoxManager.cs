// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Dialogs
{
    using System;
    using System.Runtime.InteropServices;
    using HMI.Runtime.Shared.Common.Events.Interactions;

    public class MessageBoxManager : IDisposable
    {
        private readonly EventHandler<DialogClosedEventArgs> dialogClosedCallback;
        private bool isDisposed;
        private MessageBoxControl messageBox;

        public MessageBoxManager(EventHandler<DialogClosedEventArgs> dialogClosedCallback = null)
        {
            this.dialogClosedCallback = dialogClosedCallback;
        }

        private void AttachDialogClosedHandler()
        {
            if (dialogClosedCallback != null)
            {
                messageBox.DialogClosed += dialogClosedCallback;
            }
        }

        private void CloseAndDisposeMessageBox()
        {
            if (messageBox != null)
            {
                RemoveDialogClosedHandler();
                messageBox.Close();
                messageBox.Dispose();
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
            CloseAndDisposeMessageBox();
        }

        private MessageBoxType GetMessageBoxType(NotificationSeverity severity)
        {
            switch (severity)
            {
                case NotificationSeverity.Error:
                    return MessageBoxType.Error;

                case NotificationSeverity.Warning:
                    return MessageBoxType.Warning;
            }
            return MessageBoxType.Information;
        }

        private void RemoveDialogClosedHandler()
        {
            if (dialogClosedCallback != null)
            {
                messageBox.DialogClosed -= dialogClosedCallback;
            }
        }

        public void Show(NotificationDetails details, string okButtonText = "OK", string cancelButtonText = "Cancel")
        {
            if (details != null)
            {
                this.Show(details.Content, details.Title, this.GetMessageBoxType(details.Severity), okButtonText, cancelButtonText);
            }
        }

        public void Show(string message, string headerText = "HMI", MessageBoxType messageBoxType = 0, string okButtonText = "OK", string cancelButtonText = "Cancel")
        {
            ShowMessageBox(message, headerText, messageBoxType, okButtonText, cancelButtonText);
        }

        private void ShowMessageBox(object content, string headerText, MessageBoxType messageBoxType, string okButtonText, string cancelButtonText)
        {
            CloseAndDisposeMessageBox();
            MessageBoxControl control = new MessageBoxControl();
            control.Content=content;
            control.HeaderText = headerText;
            control.MessageBoxType = messageBoxType;
            control.ButtonOKText = okButtonText;
            control.ButtonCancelText = cancelButtonText;
            messageBox = control;
            AttachDialogClosedHandler();
            messageBox.ShowDialog();
        }

        public void ShowObject(object content, string headerText = "HMI", MessageBoxType messageBoxType = 0, string okButtonText = "OK", string cancelButtonText = "Cancel")
        {
            ShowMessageBox(content, headerText, messageBoxType, okButtonText, cancelButtonText);
        }
    }
}

