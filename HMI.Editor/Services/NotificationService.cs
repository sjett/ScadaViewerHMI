// SPDX-License-Identifier: GPL-3.0-only
using Microsoft.Win32;
using System;
using System.Windows;
using Telerik.Windows.Controls;
using HMI.Common.Presentation;
using HMI.Common.Configuration;
using HMI.Common.Presentation.Events.Interactions;
using HMI.Infrastructure.Services;
using HMI.Modules.Application.Views;
using HMI.Editor.Views;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Editor.Resources;

namespace HMI.Editor.Services
{
	public class NotificationService : INotificationService
	{
		private readonly IFileSystemService fileSystemService;
        private readonly HmiSettingsStore settingsStore;
		private readonly Lazy<Shell> shellView;
		public NotificationService(
            IFileSystemService fileSystemService,
            IEventAggregator eventAggregator,
            Func<Shell> shellFactory,
            HmiSettingsStore settingsStore)
		{
			Contract.Requires<ArgumentNullException>(fileSystemService != null, "fileSystemService");
			Contract.Requires<ArgumentNullException>(shellFactory != null, "shellFactory");
			Contract.Requires<ArgumentNullException>(eventAggregator != null, "eventAggregator");
			this.fileSystemService = fileSystemService;
            this.settingsStore = settingsStore;
			eventAggregator.SubscribeNotification(new Action<NotificationDetails>(OnNotification));
            shellView = new Lazy<Shell>(shellFactory);
		}
		public void ShowInfo(string message, Exception exception = null)
		{
			this.ShowNotificationWindow(ErrorTexts.Info, message, exception, NotificationSeverity.Info);
		}
		public void ShowWarning(string message, Exception exception = null)
		{
			this.ShowNotificationWindow(ErrorTexts.Warning, message, exception, NotificationSeverity.Warning);
		}
		public void ShowErrorDialog(string message, Exception exception = null)
		{
			this.ShowNotificationWindow(ErrorTexts.Error, message, exception, NotificationSeverity.Error);
		}
		public MessageBoxResult ShowOKMessageBox(string title, string text)
		{
			return MessageBox.Show(text, title, MessageBoxButton.OK, MessageBoxImage.Question, MessageBoxResult.OK);
		}
		public MessageBoxResult ShowOKCancelMessageBox(string title, string text)
		{
			return MessageBox.Show(text, title, MessageBoxButton.OKCancel, MessageBoxImage.Question, MessageBoxResult.OK);
		}
		public MessageBoxResult ShowYesNoMessageBox(string title, string text)
		{
			return MessageBox.Show(text, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
		}
		public MessageBoxResult ShowYesNoCancelMessageBox(string title, string text)
		{
			return MessageBox.Show(text, title, MessageBoxButton.YesNoCancel, MessageBoxImage.Question, MessageBoxResult.Cancel);
		}
		public bool ShowNewProjectDialog(out string projectFolder, out string projectName)
		{
			projectFolder = null;
			projectName = null;
			NewProjectWindow newProjectWindow = new NewProjectWindow(fileSystemService, this, settingsStore);
			newProjectWindow.Owner=UserInterface.GetActiveWindow();
			newProjectWindow.ShowDialog();
			if (newProjectWindow.DialogResult == true)
			{
				projectFolder = newProjectWindow.ProjectFolder;
				projectName = newProjectWindow.ProjectName;
				return true;
			}
			return false;
		}
		public bool ShowOpenProjectDialog(out string projectFullPath)
		{
			projectFullPath = null;
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = ServicesTexts.OpenFileFilter;
			if (openFileDialog.ShowDialog() == true)
			{
				projectFullPath = openFileDialog.FileName;
				return true;
			}
			return false;
		}
		public bool ShowSaveProjectAsDialog(out string projectFolder, out string projectName)
		{
			projectFolder = null;
			projectName = null;
			SaveAsWindow saveAsWindow = new SaveAsWindow(fileSystemService, settingsStore);
			saveAsWindow.Owner=UserInterface.GetActiveWindow();
			saveAsWindow.ShowDialog();
			if (saveAsWindow.DialogResult == true)
			{
				projectFolder = saveAsWindow.ProjectFolder;
				projectName = saveAsWindow.ProjectName;
				return true;
			}
			return false;
		}
		public string GetNewLanguage(string title, string existingLanguage)
		{
			SelectLanguage selectLanguage = new SelectLanguage();
			selectLanguage.Header = title;
			selectLanguage.Owner=UserInterface.GetActiveWindow();
			SelectLanguage selectLanguage2 = selectLanguage;
			selectLanguage2.ViewModel.SelectedLanguage = existingLanguage;
			selectLanguage2.ShowDialog();
			if (!(selectLanguage2.DialogResult == true))
			{
				return null;
			}
			return selectLanguage2.ViewModel.SelectedLanguage;
		}
		public void ShowBusyIndicator(string message)
		{
            shellView.Value.ShowBusyIndicator(message);
		}
		public void HideBusyIndicator()
		{
            shellView.Value.HideBusyIndicator();
		}
		private void OnNotification(NotificationDetails notificationDetails)
		{
			this.ShowNotificationWindow(notificationDetails.Title, notificationDetails.Content, notificationDetails.Exception, notificationDetails.Severity);
		}
		private void ShowNotificationWindow(string caption, string message, Exception exception, NotificationSeverity notificationSeverity)
		{
			ErrorNotification notification = new ErrorNotification
			{
				Message = message,
				NotificationSeverity = notificationSeverity,
				Buttons = NotificationButtons.OK,
				Details = exception != null ? exception.ToString() : null
			};
            ShowNotificationWindow(caption, notification);
		}
		private void ShowNotificationWindow(string caption, ErrorNotification notification)
		{
			RadWindow radWindow = new RadWindow();
			radWindow.Width = 500.0;
			radWindow.MaxHeight = 300.0;
			radWindow.WindowStartupLocation=WindowStartupLocation.CenterOwner;
			radWindow.Owner=UserInterface.GetActiveWindow();
			radWindow.ResizeMode=ResizeMode.CanResizeWithGrip;
			radWindow.Content = notification;
			radWindow.Header = caption;
			radWindow.ShowDialog();
		}
	}
}
