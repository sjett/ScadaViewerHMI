// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation;
using HMI.Common.Configuration;
using HMI.Editor.Resources;
using HMI.Infrastructure.Services;
using Microsoft.Win32;
using System.CodeDom.Compiler;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;

namespace HMI.Editor
{
    /// <summary>
    /// NewProjectWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewProjectWindow : RadWindow
    {
        private enum ProjectValidationStatus
        {
            Valid,
            InvalidIdentifier,
            InvalidFolder,
            FolderExists
        }
        private const string ProjectNamePattern = "Project{0}";
        private readonly IFileSystemService fileSystemService;
        private readonly HmiSettingsStore settingsStore;
        private bool nameChanged;
        private string projectFolder;
        private readonly INotificationService notificationService;

        public string ProjectFolder
        {
            get
            {
                return this.projectFolder;
            }
        }
        public string ProjectName
        {
            get
            {
                return this.uxName.Text;
            }
        }

        public NewProjectWindow(
            IFileSystemService fileSystemService,
            INotificationService notificationService,
            HmiSettingsStore settingsStore)
        {
            this.InitializeComponent();
            this.fileSystemService = fileSystemService;
            this.notificationService = notificationService;
            this.settingsStore = settingsStore;
            this.uxCancelButtonElement.Click += new RoutedEventHandler(this.uxCancelButtonElement_Click);
            this.uxOKButtonElement.Click += new RoutedEventHandler(this.uxOKButtonElement_Click);
            this.uxBrowseLocation.Click += new RoutedEventHandler(this.uxBrowseLocation_Click);
            this.uxName.TextChanged += new TextChangedEventHandler(this.uxName_TextChanged);
            this.uxLocation.TextChanged += new TextChangedEventHandler(this.uxLocation_TextChanged);
            string lastProjectDirectory = settingsStore.GetString("Editor.LastProjectDirectory") ?? string.Empty;
            if (!string.IsNullOrEmpty(lastProjectDirectory) && System.IO.Directory.Exists(lastProjectDirectory))
            {
                this.uxLocation.Text = lastProjectDirectory;
            }
            else
            {
                this.uxLocation.Text = fileSystemService.DefaultProjectFolder;
            }
            this.uxName.Text = this.GetNextProjectName(this.uxLocation.Text);
            this.nameChanged = false;
        }

        private void uxBrowseLocation_Click(object sender, RoutedEventArgs e)
        {
            //FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            OpenFolderDialog folderBrowserDialog = new OpenFolderDialog();
            //folderBrowserDialog.Title = "新项目";
            folderBrowserDialog.Multiselect = false;
            //folderBrowserDialog.RootDirectory = Environment.GetFolderPath(browseFolderInfo.RootFolder);
            folderBrowserDialog.FolderName = this.uxLocation.Text;

            var owener = UserInterface.GetActiveWindow(); //Window.GetWindow(this);
            var dialogResult = folderBrowserDialog.ShowDialog(owener);
            if (dialogResult == true)
            {
                this.uxLocation.Text = folderBrowserDialog.FolderName;
                if (!this.nameChanged)
                {
                    this.uxName.Text = this.GetNextProjectName(this.uxLocation.Text);
                    this.nameChanged = false;
                }
            }
            //folderBrowserDialog.Dispose();
        }
        private void uxCancelButtonElement_Click(object sender, RoutedEventArgs e)
        {
            base.DialogResult=(new bool?(false));
            base.Close();
        }
        private void uxLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.SetButtonState();
        }
        private void uxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.nameChanged = true;
            this.SetButtonState();
        }
        private void uxOKButtonElement_Click(object sender, RoutedEventArgs e)
        {
            this.SetProjectFolderProperty();
            switch (this.ValidateProjectName())
            {
                case NewProjectWindow.ProjectValidationStatus.Valid:
                    this.HandleProjectValidationSuccess();
                    return;
                case NewProjectWindow.ProjectValidationStatus.InvalidIdentifier:
                    this.notificationService.ShowWarning(ErrorTexts.InvalidProjectName, null);
                    return;
                case NewProjectWindow.ProjectValidationStatus.InvalidFolder:
                    this.notificationService.ShowWarning(ErrorTexts.InvalidDirectoryName, null);
                    return;
                case NewProjectWindow.ProjectValidationStatus.FolderExists:
                    this.HandleProjectFolderAlreadyExists();
                    return;
                default:
                    return;
            }
        }
        private void HandleProjectFolderAlreadyExists()
        {
            if (!this.CheckIfUserWantsToOverwriteProject())
            {
                return;
            }
            this.fileSystemService.Delete(this.projectFolder);
            this.HandleProjectValidationSuccess();
        }
        private void HandleProjectValidationSuccess()
        {
            try
            {
                this.fileSystemService.EnsureFolderExists(this.projectFolder);
                settingsStore.SetString("Editor.LastProjectDirectory", this.uxLocation.Text);
                base.DialogResult=(new bool?(true));
                base.Close();
            }
            catch
            {
                this.notificationService.ShowErrorDialog(ErrorTexts.CannotCreateProject, null);
            }
        }
        private NewProjectWindow.ProjectValidationStatus ValidateProjectName()
        {
            if (!this.IsValidIdentifier(this.uxName.Text))
            {
                return NewProjectWindow.ProjectValidationStatus.InvalidIdentifier;
            }
            if (!this.fileSystemService.IsValidFolder(this.ProjectFolder))
            {
                return NewProjectWindow.ProjectValidationStatus.InvalidFolder;
            }
            if (this.fileSystemService.FolderExists(this.projectFolder) && !this.IsProjectFolderEmpty())
            {
                return NewProjectWindow.ProjectValidationStatus.FolderExists;
            }
            return NewProjectWindow.ProjectValidationStatus.Valid;
        }
        private bool CheckIfUserWantsToOverwriteProject()
        {
            return this.notificationService.ShowYesNoMessageBox(RibbonBarTexts.MessageBoxTitle, ErrorTexts.OverwriteProject) == MessageBoxResult.Yes;
        }
        private bool IsProjectFolderEmpty()
        {
            return !this.fileSystemService.GetDirectoriesInFolder(this.projectFolder).Any<string>() && !this.fileSystemService.GetFilesInFolder(this.projectFolder).Any<string>();
        }
        private void SetProjectFolderProperty()
        {
            this.projectFolder = System.IO.Path.Combine(this.uxLocation.Text, this.uxName.Text);
        }
        private bool IsValidIdentifier(string text)
        {
            bool result = false;
            try
            {
                result = CodeDomProvider.CreateProvider("C#").IsValidIdentifier(text);
            }
            catch (System.ArgumentException)
            {
            }
            catch (System.IO.PathTooLongException)
            {
            }
            catch (System.NotSupportedException)
            {
            }
            return result;
        }
        private void SetButtonState()
        {
            try
            {
                this.uxOKButtonElement.IsEnabled = (!string.IsNullOrEmpty(this.uxLocation.Text) && !string.IsNullOrEmpty(this.uxName.Text) && System.IO.Path.IsPathRooted(this.uxLocation.Text));
            }
            catch
            {
                this.uxOKButtonElement.IsEnabled = false;
            }
        }
        private string GetNextProjectName(string folderName)
        {
            int num = 0;
            string newProjectName;
            string fullProjectPath;
            do
            {
                newProjectName = this.GetNewProjectName(++num);
                fullProjectPath = System.IO.Path.Combine(folderName, newProjectName);
            }
            while (this.ExistsOnDisk(fullProjectPath));
            return newProjectName;
        }
        private bool ExistsOnDisk(string fullProjectPath)
        {
            return this.fileSystemService.ExistsFile(fullProjectPath) || this.fileSystemService.FolderExists(fullProjectPath);
        }
        private string GetNewProjectName(int duplicateNameIndex)
        {
            return string.Format(ProjectNamePattern, duplicateNameIndex);
        }
        protected override void OnPreviewKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnPreviewKeyDown(e);
            if (e.Key == Key.Escape)
            {
                base.Close();
            }
        }

    }
}
