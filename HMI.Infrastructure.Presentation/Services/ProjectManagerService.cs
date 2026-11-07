// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using HMI.Common.Instrumentation.ErrorHandling;
using System.IO;
using System.Text;
namespace HMI.Infrastructure.Presentation.Services
{

    /// <summary>
    /// 项目管理服务
    /// </summary>
    public class ProjectManagerService : IProjectManagerService
    {

        private readonly IServiceProvider container;
        private readonly IFileSystemService fileSystemService;
        private readonly IPageBuilderService pageBuilderService;
        private readonly ISerializerFactory serializerFactory;

        // 配置 JsonSerializerSettings 来忽略 null 值
        //private readonly JsonSerializerSettings josnSettings = new JsonSerializerSettings
        //{
        //    NullValueHandling = NullValueHandling.Ignore,
        //    Formatting = Formatting.Indented // 可选：美化输出格式
        //};

        public ProjectManagerService(IServiceProvider container)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            this.container = container;
            this.pageBuilderService = container.Resolve<IPageBuilderService>();
            this.fileSystemService = container.Resolve<IFileSystemService>();
            this.serializerFactory = container.Resolve<ISerializerFactory>();
        }
        public IProjectViewModel NewProject(string folder, string name)
        {
            Contract.Requires<ArgumentNullException>(folder != null);
            Contract.Requires<ArgumentException>(folder.Length != 0);
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentException>(name.Length != 0);

            IProjectViewModel projectViewModel = ActivatorUtilities.CreateInstance<ProjectViewModel>(container, name, folder);

            this.pageBuilderService.AddPage<IProjectPageViewModel>(projectViewModel, "Page");
            projectViewModel.ActionMonitor.Reset();
            return projectViewModel;
        }
        public IProjectViewModel OpenProject(string fileName)
        {
            Contract.Requires<ArgumentNullException>(fileName != null);
            Contract.Requires<ArgumentException>(fileName.Length > 0);

            JObject root = DeserializeProjectFile(fileName);
            IProjectViewModel projectViewModel = DeserializeProject(root);
            projectViewModel.ActionMonitor.Reset();
            return projectViewModel;
        }
        IProjectViewModel DeserializeProject(JObject root)
        {
            ISerializer serializer = serializerFactory.GetSerializer<IProjectViewModel>();
            return serializer.Deserialize(null, root, null) as IProjectViewModel;
        }
        JObject DeserializeProjectFile(string fileName)
        {
            using (Stream stream = fileSystemService.OpenRead(fileName))
            {
                return ParseProjectFile(stream, fileName);
            }
        }
        JObject ParseProjectFile(Stream projectStream, string fileName)
        {
            string projectFolder = fileSystemService.GetFolder(fileName);

            using (var reader = new StreamReader(projectStream, Encoding.UTF8))
            {
                var projectJson = JObject.Parse(reader.ReadToEnd());

                projectJson["Folder"] = projectFolder;
                return projectJson;
            }
        }
        public void Save(IProjectViewModel project)
        {
            Contract.Requires<ArgumentNullException>(project != null);

            PrepareProjectForSave(project);
            SaveProjectFile(project);
            SavePages(project);
        }
        void SavePages(IProjectViewModel project)
        {
            foreach (IProjectPageViewModel page in project.Pages)
            {
                SavePage(page, project);
            }
        }
        void SavePage(IProjectPageViewModel page, IProjectViewModel project)
        {
            ISerializer serializer = GetPageSerializer(page);
            string filePath = Path.Combine(project.ProjectFolder, page.ID + ".Page");
            using (Stream stream = fileSystemService.Create(filePath))
            {
                WritePageJson(page, stream, serializer);
            }
        }
        void WritePageJson(IProjectPageViewModel page, Stream stream, ISerializer serializer)
        {
            var pageJson = serializer.Serialize(page);

            using (var writer = new StreamWriter(stream, Encoding.UTF8))
            {
                //writer.Write(JsonConvert.SerializeObject(pageJson, Formatting.Indented, josnSettings));
                writer.Write(pageJson.ToString(Formatting.Indented));
            }
        }
        ISerializer GetPageSerializer(IProjectPageViewModel page)
        {
            if (page is ITemplatePageViewModel)
            {
                return serializerFactory.GetSerializer<ITemplatePageViewModel>();
            }
            return serializerFactory.GetSerializer<IProjectPageViewModel>();
        }
        void SaveProjectFile(IProjectViewModel project)
        {
            ISerializer serializer = serializerFactory.GetSerializer<IProjectViewModel>();
            var projectJson = serializer.Serialize(project);

            using (Stream stream = fileSystemService.Create(project.ProjectFullPath))
            {
                using (var writer = new StreamWriter(stream, Encoding.UTF8))
                {
                    writer.Write(projectJson.ToString(Formatting.Indented));
                }
            }
        }
        void PrepareProjectForSave(IProjectViewModel project)
        {
            RenameProjectIfNeeded(project);
            DeleteObsoleteProjectFiles(project);
            DeleteObsoleteProjectFolders(project);
        }
        void RenameProjectIfNeeded(IProjectViewModel project)
        {
            if (project.ProjectName != project.OldProjectName)
            {
                string parentFolder = GetProjectParentFolder(project);
                fileSystemService.Rename(parentFolder, project.ProjectName, project.OldProjectName);
                project.SetProjectProperties(project.ProjectName, Path.Combine(parentFolder, project.ProjectName));
            }
        }
        string GetProjectParentFolder(IProjectViewModel project)
        {
            int projectNameIndex = project.ProjectFolder.LastIndexOf(project.OldProjectName);
            if (projectNameIndex != -1)
            {
                return project.ProjectFolder.Substring(0, projectNameIndex);
            }
            return project.ProjectFolder;
        }
        void DeleteObsoleteProjectFiles(IProjectViewModel project)
        {
            IEnumerable<string> filesInFolder = fileSystemService.GetFilesInFolder(project.ProjectFolder);
            foreach (string fileName in filesInFolder)
            {
                fileSystemService.Delete(project.ProjectFolder, fileName);
            }
        }
        void DeleteObsoleteProjectFolders(IProjectViewModel project)
        {
            var imagesFolder = project.ProjectImagesFolder.Replace(project.ProjectFolder, string.Empty);
            var mediaFolder = project.ProjectMediaFolder.Replace(project.ProjectFolder, string.Empty);
            var xamlFolder = project.ProjectXamlsFolder.Replace(project.ProjectFolder, string.Empty);
            var webFolder = project.AppProjectFolder.Replace(project.ProjectFolder, string.Empty);

            imagesFolder = imagesFolder.Replace("\\", string.Empty);
            mediaFolder = mediaFolder.Replace("\\", string.Empty);
            xamlFolder = xamlFolder.Replace("\\", string.Empty);
            webFolder = webFolder.Replace("\\", string.Empty);

            IEnumerable<string> foldersToDelete = fileSystemService
                .GetDirectoriesInFolder(project.ProjectFolder)
                .Where(folder => folder != imagesFolder && folder != mediaFolder && folder != xamlFolder && folder != webFolder);

            //删除多余文件夹
            foreach (string folderName in foldersToDelete)
            {
                fileSystemService.Delete(project.ProjectFolder, folderName);
            }
        }
        public void SaveAs(IProjectViewModel project, string projectFolder, string projectName)
        {
            Contract.Requires<ArgumentNullException>(projectName != null);
            Contract.Requires<ArgumentNullException>(projectFolder != null);
            Contract.Requires<ArgumentNullException>(project != null);
            Contract.Requires<ArgumentException>(projectFolder.Length > 0);
            Contract.Requires<ArgumentException>(projectName.Length > 0);

            string projectImagesFolder = project.ProjectImagesFolder;
            string projectMediaFolder = project.ProjectMediaFolder;
            string projectXamlsFolder = project.ProjectXamlsFolder;
            string webProjectFolder = project.AppProjectFolder;
            project.SetProjectProperties(projectName, projectFolder);
            this.fileSystemService.CopyFolder(projectImagesFolder, project.ProjectImagesFolder, true);
            this.fileSystemService.CopyFolder(projectMediaFolder, project.ProjectMediaFolder, true);
            this.fileSystemService.CopyFolder(projectXamlsFolder, project.ProjectXamlsFolder, true);
            this.fileSystemService.CopyFolder(webProjectFolder, project.AppProjectFolder, true);

            this.Save(project);

        }
        public void RemovePage(IProjectPageViewModel page)
        {
            IExplorerNodeViewModel parent = page.Parent;
            parent.Children.Remove(page);
            parent.IsSelected = true;
            parent.Project.Pages.Remove(page);
        }
        public void RemoveItems(IExplorerNodeViewModel parent, IEnumerable<IExplorerNodeViewModel> items)
        {
            HashSet<IExplorerNodeViewModel> itemsToRemove = new HashSet<IExplorerNodeViewModel>(items);
            RemoveSelectedItems(items, itemsToRemove, parent);
        }
        public virtual IProjectPageViewModel AddPage(IExplorerNodeViewModel parent, string name)
        {
            return this.pageBuilderService.AddPage<IProjectPageViewModel>(parent, name);
        }
        public ITemplatePageViewModel AddTemplatePage(IExplorerNodeViewModel parent, string name)
        {
            return this.pageBuilderService.AddPage<ITemplatePageViewModel>(parent, name);
        }
        public IProjectPageViewModel AddNavigationPage(IExplorerNodeViewModel parent, string name)
        {
            IProjectPageViewModel projectPageViewModel = this.AddPage(parent, name);
            SetNavigationPageSize(projectPageViewModel, parent);

            return projectPageViewModel;
        }
        void SetNavigationPageSize(IProjectPageViewModel page, IExplorerNodeViewModel parent)
        {
            page.Size = string.Format("{0}x{1}", (int)((CommonControlViewModel)parent).ControlWidth, (int)((CommonControlViewModel)parent).ControlHeight);
        }
        private void RemoveSelectedItems(IEnumerable<IExplorerNodeViewModel> nodes, HashSet<IExplorerNodeViewModel> itemsToRemove, IExplorerNodeViewModel parent)
        {
            foreach (IExplorerNodeViewModel node in nodes.ToList())
            {
                if (itemsToRemove.Contains(node))
                {
                    ICommonControlViewModel commonControlViewModel = node as ICommonControlViewModel;
                    if (commonControlViewModel != null)
                    {
                        commonControlViewModel.ToggleSelection();
                    }
                    foreach (IExplorerNodeViewModel child in node.Children)
                    {
                        itemsToRemove.Add(child);
                    }
                    this.RemoveSelectedItems(node.Children, itemsToRemove, node);
                    IProjectPageViewModel projectPageViewModel = node as IProjectPageViewModel;
                    if (projectPageViewModel != null)
                    {
                        parent.Project.Pages.Remove(projectPageViewModel);
                    }
                    parent.Children.Remove(node);
                    itemsToRemove.Remove(node);
                }
            }
        }
    }
}
