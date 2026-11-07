// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    public class DialogManager : IDisposable
    {
        private readonly Func<object> createContentCallback;
        private DialogControl dialog;
        private readonly EventHandler<DialogClosedEventArgs> dialogClosedCallback;
        private readonly Action disposeContentCallback;
        private bool isDisposed;

        public DialogManager(Func<object> createContentCallback, Action disposeContentCallback = null, EventHandler<DialogClosedEventArgs> dialogClosedCallback = null)
        {
            this.createContentCallback = createContentCallback;
            this.disposeContentCallback = disposeContentCallback ?? new Action(DisposeDialogContent);
            this.dialogClosedCallback = dialogClosedCallback;
        }

        private void AttachDialogClosedHandler()
        {
            if (dialogClosedCallback != null)
            {
                dialog.DialogClosed += dialogClosedCallback;
            }
        }

        public void Close(DialogResult dialogResult)
        {
            if (dialog != null)
            {
                dialog.Close(dialogResult);
            }
        }

        private void CloseAndDisposeDialog()
        {
            if (dialog != null)
            {
                RemoveDialogClosedHandler();
                dialog.Close();
                disposeContentCallback();
                dialog.Dispose();
            }
        }

        private void CreateAndShowDialog(object contentControl, DialogModality modality, string dialogTitle)
        {
            CreateDialog(contentControl, dialogTitle);
            ShowDialog(modality);
        }

        private DependencyObject CreateContentFromPopupSource(Control currentControl, string popupSource, IEnumerable<ParameterItem> popupParameters)
        {
            DependencyObject popupContent = PopupUtilities.CreatePopupContent(currentControl, popupSource, popupParameters);
            if (popupContent == null)
            {
                throw new Exception("Unknown object '" + popupSource + "'.");
            }
            return popupContent;
        }

        private void CreateDialog(object contentControl, string dialogTitle)
        {
            DialogControl control = new DialogControl {
                HeaderText = dialogTitle
            };
            control.Content=contentControl;
            dialog = control;
            AttachDialogClosedHandler();
        }

        public void Dispose()
        {
            if (!isDisposed)
            {
                DisposeManagedResources();
            }
            isDisposed = true;
        }

        private void DisposeDialogContent()
        {
            if (dialog != null)
            {
                NavigationHelper.DisposeControls(dialog.Content as DependencyObject);
            }
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

        public void Show(string title = "HMI", DialogModality modality = 0)
        {
            CloseAndDisposeDialog();
            if (createContentCallback != null)
            {
                object contentControl = createContentCallback();
                CreateAndShowDialog(contentControl, modality, title);
            }
        }

        public void Show(Control currentControl, string popupSource, ObservableCollection<ParameterItem> popupParameters, string title = "HMI", DialogModality modality = 0)
        {
            CloseAndDisposeDialog();
            DependencyObject contentControl = CreateContentFromPopupSource(currentControl, popupSource, popupParameters);
            CreateAndShowDialog(contentControl, modality, title);
        }

        private void ShowDialog(DialogModality modality)
        {
            if (modality == DialogModality.Modal)
            {
                dialog.ShowDialog();
            }
            else
            {
                dialog.Show();
            }
        }
    }
}
