// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using Newtonsoft.Json.Linq;
using HMI.Common.Instrumentation.ErrorHandling;
using System.IO;
using System.Reflection;
using System.Windows.Media;


namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 项目序列化器
    /// </summary>
    public class ProjectSerializer : BaseSerializer, IProjectSerializer
    {
        private const string ProjectTitleName = "ProjectTitle";// 项目标题属性名称常量
        private const string StretchContentName = "StretchContent";// 是否拉伸页面以适应屏幕属性名称常量
        private const string StateDefaultBackgroundName = "StateDefaultBackground";// 状态默认背景色属性名称常量
        private const string StateDefaultForegroundName = "StateDefaultForeground";// 状态默认前景色属性名称常量
        private const string PageDefaultBackgroundName = "PageDefaultBackground";// 页面默认背景色属性名称常量
        private const string HtmlPageDefaultBackgroundName = "HtmlPageDefaultBackground";// HTML页面背景色属性名称常量
        private const string PagesName = "Pages";// 页面集合属性名称常量
        // 服务器信息属性名称常量
        private readonly IFileSystemService fileSystemService;

        /// <summary>
        /// 项目序列化器
        /// </summary>
        /// <param name="serializerFactory"></param>
        /// <param name="fileSystemService"></param>
        /// <param name="container"></param>
        public ProjectSerializer(ISerializerFactory serializerFactory, IFileSystemService fileSystemService, IServiceProvider container)
            : base(container, serializerFactory)
        {
            Contract.Requires<ArgumentNullException>(fileSystemService != null, "fileSystemService");
            this.fileSystemService = fileSystemService!;
        }
        /// <summary>
        /// 序列化项目
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override JObject Serialize(object source)
        {
            IProjectViewModel projectViewModel = source as IProjectViewModel;
            var rootElement = new JObject();//"EditorProject"
            AddVersionProperty(rootElement);// 添加版本信息
            AddProjectTitle(projectViewModel, rootElement);// 添加项目标题
            AddStretchContent(projectViewModel, rootElement);// 添加是否拉伸页面以适应屏幕
            AddStateDefaultBackground(projectViewModel, rootElement);// 添加状态默认背景色
            AddStateDefaultForeground(projectViewModel, rootElement);// 添加状态默认前景色
            AddPageDefaultBackground(projectViewModel, rootElement);// 添加页面默认背景色
            AddHtmlPageDefaultBackground(projectViewModel, rootElement);// 添加HTML页面背景色
            AddBlinkingFrequencies(projectViewModel, rootElement);// 添加闪烁频率
            AddServerInfo(projectViewModel, rootElement);// 添加服务器信息

            var content = new JProperty(PagesName, SerializeChildPages(projectViewModel));
            rootElement.Add(content);// JSON路径: [Root] -> Pages -> [page1,page2,...]
            return rootElement;
        }
        // 添加页面信息
        IEnumerable<JObject> SerializeChildPages(IExplorerNodeViewModel node)
        {
            return node.Children!.SelectMany(SerializePageNode);

        }
        // 获取页面信息
        IEnumerable<JObject> SerializePageNode(IExplorerNodeViewModel node)
        {
            // 情况 1：如果是项目页面（ProjectPage）
            if (node is IProjectPageViewModel)
            {
                var elementObj = new JObject
                {
                    new JProperty("Name", node.ID),
                    //addNavigationTarget(node),
                    new JProperty("IsTemplatePage", node is ITemplatePageViewModel),
                    new JProperty(PagesName, SerializeChildPages(node))// 添加子页面信息
                };
                var navigationTargetProperty = CreateNavigationTargetProperty(node);
                if (navigationTargetProperty != null)
                    elementObj.Add(navigationTargetProperty);

                yield return elementObj;
                yield break;
            }

            // 情况 2：否则，遍历子节点
            foreach (var child in SerializeChildPages(node))
            {
                yield return child;
            }
        }
        // 添加导航目标信息属性
        JProperty? CreateNavigationTargetProperty(IExplorerNodeViewModel node)
        {
            INavigationTargetViewModel navigationTarget = node.Parent as INavigationTargetViewModel;
            if (navigationTarget != null && !navigationTarget.IsInvalidTarget)
            {
                return new JProperty("NavigationTarget", navigationTarget.ID);
            }
            return null;
        }

        // 添加版本信息
        void AddVersionProperty(JObject projectJson)
        {
            projectJson.Add("Version", Assembly.GetEntryAssembly()?.GetName().Version?.ToString());
        }
        // 添加项目标题
        void AddProjectTitle(IProjectViewModel project, JObject projectJson)
        {
            projectJson.Add(ProjectTitleName, string.IsNullOrEmpty(project.ProjectTitle) ? string.Empty : project.ProjectTitle);
        }
        // 添加是否拉伸页面以适应屏幕
        private void AddStretchContent(IProjectViewModel project, JObject projectJson)
        {
            projectJson.Add(StretchContentName, project.StretchPages);
        }
        // 添加状态默认背景色
        void AddStateDefaultBackground(IProjectViewModel project, JObject projectJson)
        {
            if (project.StateDefaultBackground != null)
            {
                projectJson.Add(StateDefaultBackgroundName, project.StateDefaultBackground.ToString());
            }
        }
        // 添加状态默认前景色
        void AddStateDefaultForeground(IProjectViewModel project, JObject projectJson)
        {
            if (project.StateDefaultForeground != null)
            {
                projectJson.Add(StateDefaultForegroundName, project.StateDefaultForeground.ToString());
            }
        }
        // 添加页面默认背景色
        void AddPageDefaultBackground(IProjectViewModel project, JObject projectJson)
        {
            if (project.PageDefaultBackground != null)
            {
                projectJson.Add(PageDefaultBackgroundName, project.PageDefaultBackground.ToString());
            }
        }
        // 添加HTML页面背景色
        void AddHtmlPageDefaultBackground(IProjectViewModel project, JObject projectJson)
        {
            projectJson.Add(HtmlPageDefaultBackgroundName, project.AppPageBackground.ToString());
        }
        // 添加闪烁频率
        void AddBlinkingFrequencies(IProjectViewModel project, JObject projectJson)
        {
            projectJson.Add("BlinkingFrequencies", new JObject
            {
                { "Fast", project.FastBlinkSpeed },
                { "Medium", project.MediumBlinkSpeed },
                { "Slow", project.SlowBlinkSpeed }
            });
        }
        // 添加服务器信息
        void AddServerInfo(IProjectViewModel project, JObject root)
        {
            root.Add("ServerName", project.ServerName);
            root.Add("Host", project.Host);
            root.Add("Port", project.Port);
            //root.Add("Protocol", project.Protocol.ToString());
            root.Add("Timeout", project.Timeout);
            root.Add("Username", project.Username);
            root.Add("Password", project.Password);
            root.Add("SecretKey", project.SecretKey);

        }
        // 反序列化服务器信息
        void DeserializeServerInfo(ProjectViewModel project, JObject root)
        {
            project.ServerName = ((string?)root["ServerName"]) ?? project.ServerName;
            project.Host = ((string?)root["Host"]) ?? project.Host;
            project.Port = ((int?)root["Port"]) ?? project.Port;
            project.Timeout = ((int?)root["Timeout"]) ?? project.Timeout;
            project.Username = ((string?)root["Username"]) ?? project.Username;
            project.Password = ((string?)root["Password"]) ?? project.Password;
            project.SecretKey = ((string?)root["SecretKey"]) ?? project.SecretKey;


            //if(Enum.TryParse<HttpProtocol>(((string?)root["Protocol"]) ?? project.Protocol.ToString(), out var protocol))
            //{
            //    project.Protocol = protocol;
            //}
        }
        /// <summary>
        /// 反序列化闪烁频率
        /// </summary>
        /// <param name="project"></param>
        /// <param name="root"></param>
        /// <param name="version"></param>
        protected virtual void DeserializeBlinkingFrequencies(IProjectViewModel project, JObject root, Version version)
        {
            project.FastBlinkSpeed = ((double?)root["BlinkingFrequencies"]?["Fast"]) ?? project.FastBlinkSpeed;
            project.MediumBlinkSpeed = ((double?)root["BlinkingFrequencies"]?["Medium"]) ?? project.MediumBlinkSpeed;
            project.SlowBlinkSpeed = ((double?)root["BlinkingFrequencies"]?["Slow"]) ?? project.SlowBlinkSpeed;
        }

        /// <summary>
        /// 获取反序列化对象
        /// </summary>
        /// <param name="ancestorNode"></param>
        /// <param name="root"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, Version version)
        {
            if (version == null)
            {
                version = new Version(1, 0);
                var vers = ((string?)root["Version"]);
                if (!string.IsNullOrEmpty(vers))
                {
                    version = new Version(vers);
                }
            }

            string? folder = (string?)root["Folder"];
            string? name = (string?)root[ProjectTitleName];

            var projectViewModel = new ProjectViewModel(Container, name, folder);

            projectViewModel.StretchPages = ((bool?)root[StretchContentName]) ?? projectViewModel.StretchPages;
            if(root[StateDefaultBackgroundName] != null)
                projectViewModel.StateDefaultBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)root[StateDefaultBackgroundName]!)).GetAsFrozen();
            if(root[StateDefaultForegroundName] != null)
                projectViewModel.StateDefaultForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)root[StateDefaultForegroundName]!)).GetAsFrozen();
            if (root[PageDefaultBackgroundName] != null)
                projectViewModel.PageDefaultBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)root[PageDefaultBackgroundName]!)).GetAsFrozen();
            if (root[HtmlPageDefaultBackgroundName] != null)
                projectViewModel.AppPageBackground = (Color)ColorConverter.ConvertFromString((string)root[HtmlPageDefaultBackgroundName]!);
            
            DeserializeBlinkingFrequencies(projectViewModel, root, version);

            DeserializeServerInfo(projectViewModel, root);

            DeserializePages(root, version, projectViewModel, projectViewModel);

            IExplorerNodeViewModel projectNode = projectViewModel;
            CollectProjectPages(projectViewModel, projectNode);
            PublishDeserializationFinished(projectViewModel);
            return projectViewModel;
        }
        void PublishDeserializationFinished(IProjectViewModel project)
        {
            DeserializationFinishedEvent deserializationFinishedEvent = EventAggregator.GetEvent<DeserializationFinishedEvent>();
            if (deserializationFinishedEvent != null)
            {
                deserializationFinishedEvent.Publish(project);
            }
        }
        void CollectProjectPages(IProjectViewModel project, IExplorerNodeViewModel parent)
        {
            project.Pages.AddRange(parent.Children.OfType<IProjectPageViewModel>());
            foreach (IExplorerNodeViewModel child in parent.Children)
            {
                CollectProjectPages(project, child);
            }
        }
        /// <summary>
        /// 反序列化页面集合
        /// </summary>
        /// <param name="project"></param>
        /// <param name="parent"></param>
        /// <param name="pages"></param>
        /// <param name="version"></param>
        protected virtual void DeserializePageElements(IProjectViewModel project, IExplorerNodeViewModel parent, IEnumerable<JObject> pages, Version version)
        {
            foreach (var pageJson in pages)
            {
                if (((bool)pageJson["IsTemplatePage"]!) == false)
                    DeserializePage<IProjectPageViewModel>(parent, pageJson, project, version);
            }
        }
        /// <summary>
        /// 反序列化模板页面集合
        /// </summary>
        /// <param name="project"></param>
        /// <param name="parent"></param>
        /// <param name="pages"></param>
        /// <param name="version"></param>
        protected virtual void DeserializeTemplatePageElements(IProjectViewModel project, IExplorerNodeViewModel parent, IEnumerable<JObject> pages, Version version)
        {
            foreach (var pageJson in pages)
            {
                if (((bool)pageJson["IsTemplatePage"]!) == true)
                    DeserializePage<ITemplatePageViewModel>(parent, pageJson, project, version);
            }
        }
        // 反序列化页面集合
        private void DeserializePages(JObject root, Version version, IProjectViewModel project, IExplorerNodeViewModel projectNode)
        {
            var pages = root[PagesName]?.OfType<JObject>() ?? Enumerable.Empty<JObject>();
            DeserializePageElements(project, projectNode, pages, version);
            DeserializeTemplatePageElements(project, projectNode, pages, version);
        }
        // 反序列化单个页面
        private void DeserializePage<T>(IExplorerNodeViewModel parent, JObject pageJson, IProjectViewModel project, Version version) where T : class, IProjectPageViewModel
        {
            IExplorerNodeViewModel? pageNode = FindNavigationTarget(pageJson, parent);
            if (pageNode == null)
            {
                return;
            }
            string pageName = (string)pageJson["Name"]!;
            T pageViewModel = CreatePage<T>(project, pageNode, version, pageName);
            pageNode.Children.Add(pageViewModel);

            DeserializePages(pageJson, version, project, pageViewModel);
        }
        // 查找导航目标节点
        IExplorerNodeViewModel? FindNavigationTarget(JObject pageJson, IExplorerNodeViewModel parentNode)
        {
            IProjectPageViewModel? parentPage = parentNode as IProjectPageViewModel;
            if (parentPage != null)
            {
                return FindNavigationTarget(parentPage, pageJson);
            }
            return parentNode;
        }
        // 查找导航目标节点
        INavigationTargetViewModel? FindNavigationTarget(IProjectPageViewModel parent, JObject pageJson)
        {
            var navigationTargetId = ((string?)pageJson["NavigationTarget"]);
            return FindNavigationTarget(parent, navigationTargetId);
        }
        // 查找导航目标节点
        INavigationTargetViewModel? FindNavigationTarget(IExplorerNodeViewModel parent, string? navigationTargetId)
        {
            foreach (IExplorerNodeViewModel child in parent.Children)
            {
                INavigationTargetViewModel navigationTarget = child as INavigationTargetViewModel;
                if (navigationTarget != null && navigationTarget.ID == navigationTargetId)
                {
                    return navigationTarget;
                }

                INavigationTargetViewModel childNavigationTarget = FindNavigationTarget(child, navigationTargetId);
                if (childNavigationTarget != null)
                {
                    return childNavigationTarget;
                }
            }
            return null;
        }
        // 创建页面实例
        private T CreatePage<T>(IProjectViewModel project, IExplorerNodeViewModel ancestorNode, Version version, string pageName) where T : class, IProjectPageViewModel
        {
            ISerializer serializer = SerializerFactory.GetSerializer<T>();
            using (Stream stream = OpenPageFile(pageName, project))
            {
                JObject pageJson = JObject.Load(new Newtonsoft.Json.JsonTextReader(new StreamReader(stream)));
                pageJson.Add(new JProperty("Folder", project.ProjectFolder));
                pageJson.Add(new JProperty("Name", pageName));

                return serializer.Deserialize(ancestorNode, pageJson, version) as T;
            }
        }
        // 打开页面文件流
        Stream OpenPageFile(string pageName, IProjectViewModel project)
        {
            string filePath = GetPageFilePath(project, pageName);
            return this.fileSystemService.OpenRead(filePath);
        }
        // 获取页面文件路径
        string GetPageFilePath(IProjectViewModel project, string pageName)
        {
            string pageFileName = pageName + ".Page";
            return Path.Combine(project.ProjectFolder, pageFileName);
        }

    }
}
