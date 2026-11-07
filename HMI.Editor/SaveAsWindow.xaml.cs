// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Telerik.Windows.Controls;
using HMI.Common.Configuration;
using HMI.Infrastructure.Services;
using HMI.Editor.Resources;
using Microsoft.Win32;
using DialogParameters = Telerik.Windows.Controls.DialogParameters;

namespace HMI.Editor
{
    /// <summary>
    /// SaveAsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SaveAsWindow : RadWindow
    {
        private const string ProjectNamePattern = "Project{0}";
        private bool nameChanged;
        private string projectFolder;
        private readonly HmiSettingsStore settingsStore;

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

        public SaveAsWindow(IFileSystemService fileSystemService, HmiSettingsStore settingsStore)
        {
            this.InitializeComponent();
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
            //folderBrowserDialog.Title = "保存项目";
            folderBrowserDialog.Multiselect = false;
            //folderBrowserDialog.RootDirectory = Environment.GetFolderPath(browseFolderInfo.RootFolder);
            folderBrowserDialog.FolderName = this.uxLocation.Text;

            var dialogResult = folderBrowserDialog.ShowDialog();
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
            if (this.CheckNames())
            {
                settingsStore.SetString("Editor.LastProjectDirectory", this.uxLocation.Text);
                base.DialogResult=(new bool?(true));
                base.Close();
            }
        }
        private bool CheckNames()
        {
            bool isValidProjectName = false;
            try
            {
                isValidProjectName = CodeDomProvider.CreateProvider("C#").IsValidIdentifier(this.uxName.Text);
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
            if (!isValidProjectName)
            {
                DialogParameters dialogParameters = new DialogParameters();
                dialogParameters.Header=(ErrorTexts.Warning);
                dialogParameters.Content=(ErrorTexts.InvalidProjectName);
                dialogParameters.OkButtonContent=(WindowTexts.OK);
                dialogParameters.CancelButtonContent=(WindowTexts.Cancel);
                dialogParameters.Owner=(this);
                DialogParameters dialogParameters2 = dialogParameters;
                RadWindow.Alert(dialogParameters2);
                return false;
            }
            bool isValidProjectFolder = false;
            if (this.uxCreateFolder.IsChecked.HasValue && this.uxCreateFolder.IsChecked.Value)
            {
                this.projectFolder = System.IO.Path.Combine(this.uxLocation.Text, this.uxName.Text);
            }
            else
            {
                this.projectFolder = this.uxLocation.Text;
            }
            try
            {
                new System.IO.DirectoryInfo(this.projectFolder);
                isValidProjectFolder = true;
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
            if (!isValidProjectFolder)
            {
                DialogParameters dialogParameters3 = new DialogParameters();
                dialogParameters3.Header=(ErrorTexts.Warning);
                dialogParameters3.Content=(ErrorTexts.InvalidDirectoryName);
                dialogParameters3.OkButtonContent=(WindowTexts.OK);
                dialogParameters3.CancelButtonContent=(WindowTexts.Cancel);
                dialogParameters3.Owner=(this);
                DialogParameters dialogParameters4 = dialogParameters3;
                RadWindow.Alert(dialogParameters4);
                return false;
            }
            if (!System.IO.Directory.Exists(this.projectFolder))
            {
                bool result;
                try
                {
                    System.IO.Directory.CreateDirectory(this.projectFolder);
                    return true;
                }
                catch
                {
                    DialogParameters dialogParameters5 = new DialogParameters();
                    dialogParameters5.Header=(ErrorTexts.Warning);
                    dialogParameters5.Content=(ErrorTexts.CannotCreateProject);
                    dialogParameters5.OkButtonContent=(WindowTexts.OK);
                    dialogParameters5.CancelButtonContent=(WindowTexts.Cancel);
                    dialogParameters5.Owner=(this);
                    DialogParameters dialogParameters6 = dialogParameters5;
                    RadWindow.Alert(dialogParameters6);
                    result = false;
                }
                return result;
            }
            if (System.IO.Directory.GetFiles(this.projectFolder).Length > 0 || System.IO.Directory.GetDirectories(this.projectFolder).Length > 0)
            {
                DialogParameters dialogParameters7 = new DialogParameters();
                dialogParameters7.Header=(ErrorTexts.Warning);
                dialogParameters7.Content=(ErrorTexts.ProjectAlreadyExists);
                dialogParameters7.OkButtonContent=(WindowTexts.OK);
                dialogParameters7.CancelButtonContent=(WindowTexts.Cancel);
                dialogParameters7.Owner=(this);
                DialogParameters dialogParameters8 = dialogParameters7;
                RadWindow.Alert(dialogParameters8);
                return false;
            }
            return true;
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
            int projectNumber = 1;
            string candidatePath = System.IO.Path.Combine(folderName, string.Format(ProjectNamePattern, projectNumber));
            while (System.IO.File.Exists(candidatePath) || System.IO.Directory.Exists(candidatePath))
            {
                candidatePath = System.IO.Path.Combine(folderName, string.Format(ProjectNamePattern, ++projectNumber));
            }
            return string.Format(ProjectNamePattern, projectNumber);
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
