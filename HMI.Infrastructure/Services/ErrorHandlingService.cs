// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.Infrastructure.Services
{
    public class ErrorHandlingService : IErrorHandlingService
    {
        public void HandleWarning(string messageFormat, params object[] arguments)
        {
            if (string.IsNullOrEmpty(messageFormat))
            {
                throw new ArgumentException("messageFormat is null or empty.", "messageFormat");
            }
            ShowMessage(messageFormat, arguments, "Warning", MessageBoxImage.Exclamation);
        }
        public void HandleError(string messageFormat, params object[] arguments)
        {
            if (string.IsNullOrEmpty(messageFormat))
            {
                throw new ArgumentException("messageFormat is null or empty.", "messageFormat");
            }
            ShowMessage(messageFormat, arguments, "Error", MessageBoxImage.Hand);
        }
        private static void ShowMessage(string messageFormat, object[] arguments, string title, MessageBoxImage icon)
        {
            string messageBoxText = string.Format(messageFormat, arguments);
            MessageBox.Show(messageBoxText, title, MessageBoxButton.OK, icon);
        }
    }
}
