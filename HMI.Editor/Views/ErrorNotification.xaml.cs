// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Telerik.Windows.Controls;
using HMI.Common.Presentation.Events.Interactions;
using HMI.Editor.Resources;

namespace HMI.Editor.Views
{
    /// <summary>
    /// ErrorNotification.xaml 的交互逻辑
    /// </summary>
    public partial class ErrorNotification : UserControl
    {
        private NotificationButtons buttons;
        private NotificationSeverity notificationSeverity;

        public string Message
        {
            get;
            set;
        }
        public string Details
        {
            get;
            set;
        }
        public string Image
        {
            get;
            set;
        }
        public bool IsDetailsPanelVisible
        {
            get
            {
                return !string.IsNullOrWhiteSpace(this.Details);
            }
        }
        public bool IsOKButtonVisible
        {
            get;
            set;
        }
        public bool IsCancelButtonVisible
        {
            get;
            set;
        }
        public bool IsYesButtonVisible
        {
            get;
            set;
        }
        public bool IsNoButtonVisible
        {
            get;
            set;
        }
        public NotificationSeverity NotificationSeverity
        {
            get
            {
                return this.notificationSeverity;
            }
            set
            {
                this.notificationSeverity = value;
                this.UpdateNotificationImage();
            }
        }
        public NotificationButtons Buttons
        {
            get
            {
                return this.buttons;
            }
            set
            {
                this.buttons = value;
                this.UpdateButtonVisibility();
            }
        }

        public ErrorNotification()
        {
            InitializeComponent();

            this.UpdateButtonVisibility();
            this.UpdateNotificationImage();
            base.DataContext = this;
        }

        private void UpdateButtonVisibility()
        {
            this.IsOKButtonVisible = false;
            this.IsCancelButtonVisible = false;
            this.IsYesButtonVisible = false;
            this.IsNoButtonVisible = false;
            switch (this.Buttons)
            {
                case NotificationButtons.OK:
                    this.IsOKButtonVisible = true;
                    return;
                case NotificationButtons.OKCancel:
                    this.IsOKButtonVisible = true;
                    this.IsCancelButtonVisible = true;
                    return;
                case NotificationButtons.YesNo:
                    this.IsYesButtonVisible = true;
                    this.IsNoButtonVisible = true;
                    return;
                case NotificationButtons.YesNoCancel:
                    this.IsYesButtonVisible = true;
                    this.IsNoButtonVisible = true;
                    this.IsCancelButtonVisible = true;
                    return;
                default:
                    return;
            }
        }
        private void UpdateNotificationImage()
        {
            switch (this.NotificationSeverity)
            {
                case NotificationSeverity.Error:
                    this.Image = "/HMI.Editor;component/Resources/Images/error.png";
                    return;
                case NotificationSeverity.Warning:
                    this.Image = "/HMI.Editor;component/Resources/Images/warning.png";
                    return;
                default:
                    this.Image = "/HMI.Editor;component/Resources/Images/info.png";
                    return;
            }
        }
        private void uxCopyToClipboard_Click(object sender, RoutedEventArgs e)
        {
            string notificationDetails = this.GetNotificationDetails();
            Clipboard.SetText(notificationDetails);
        }
        private string GetNotificationDetails()
        {
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
            string value = this.GetNotificationSeverity();
            stringBuilder.AppendLine(value);
            stringBuilder.AppendLine("-----------------------------");
            stringBuilder.AppendLine(this.Message);
            stringBuilder.AppendLine("-----------------------------");
            stringBuilder.AppendLine(ErrorNotificationTexts.Details);
            stringBuilder.AppendLine();
            stringBuilder.AppendLine(this.Details);
            return stringBuilder.ToString();
        }
        private string GetNotificationSeverity()
        {
            string result = string.Empty;
            switch (this.NotificationSeverity)
            {
                case NotificationSeverity.Error:
                    result = ErrorNotificationTexts.Error;
                    break;
                case NotificationSeverity.Warning:
                    result = ErrorNotificationTexts.Warning;
                    break;
            }
            return result;
        }
        private void uxOK_Click(object sender, RoutedEventArgs e)
        {
            this.CloseWindow(new bool?(true));
        }
        private void CloseWindow(bool? dialogResult)
        {
            RadWindow parentRadWindow = RadWindow.GetParentRadWindow(this);
            parentRadWindow.DialogResult=(dialogResult);
            parentRadWindow.Close();
        }
        private void uxYes_Click(object sender, RoutedEventArgs e)
        {
            this.CloseWindow(new bool?(true));
        }
        private void uxNo_Click(object sender, RoutedEventArgs e)
        {
            this.CloseWindow(new bool?(false));
        }
        private void uxCancel_Click(object sender, RoutedEventArgs e)
        {
            this.CloseWindow(null);
        }
    }
}
