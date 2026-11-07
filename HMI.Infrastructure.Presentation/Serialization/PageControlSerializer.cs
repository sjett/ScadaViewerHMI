// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
using HMI.Common.Instrumentation.ErrorHandling;
using System.IO;
using System.Windows.Media;

namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 页面控件序列化器
    /// </summary>
    public class PageControlSerializer : BaseSerializer
    {
        // 当前反序列化的页面视图模型
        private IProjectPageViewModel projectPageViewModel;

        /// <summary>
        /// 项目管理服务
        /// </summary>
        public IProjectManagerService ProjectManagerService
        {
            get;
            private set;
        }
        /// <summary>
        /// 页面控件序列化器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="serializerFactory"></param>
        /// <param name="projectManagerService"></param>
        public PageControlSerializer(IServiceProvider container, ISerializerFactory serializerFactory, IProjectManagerService projectManagerService)
            : base(container, serializerFactory)
        {
            Contract.Requires<ArgumentNullException>(projectManagerService != null, "projectManagerService");
            ProjectManagerService = projectManagerService;
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override JObject Serialize(object source)
        {
            IProjectPageViewModel projectPageViewModel = source as IProjectPageViewModel;
            var root = new JObject();//"EditorPage"
            root.Add(SerializePageSize(projectPageViewModel));
            if (projectPageViewModel.Background != null)
            {
                root.Add("Background", projectPageViewModel.Background.ToString());
            }
            root.Add("UsePageNameAsSignalPrefix", projectPageViewModel.UsePageNameAsSignalPrefix);
            root.Add("SignalPrefix", projectPageViewModel.SignalPrefix);
            root.Add("StretchContent", projectPageViewModel.StretchPages);
            SerializeChildControls(projectPageViewModel, root);
            return root;
        }
        private JProperty SerializePageSize(IProjectPageViewModel pageViewModel)
        {
            return new JProperty("Size", new JObject
            {
                { "Width", pageViewModel.Width },
                { "Height", pageViewModel.Height },
                { "Size", pageViewModel.Size }
            });
        }
        // 序列化页面子控件
        private void SerializeChildControls(IProjectPageViewModel pageViewModel, JObject root)
        {
            if (pageViewModel.Children.Any())
            {
                var children = new List<JObject>();
                ControlSerializerExtensions.SerializeChildList(pageViewModel.Children.OfType<ICommonControlViewModel>(), children, this.SerializerFactory);
                root.Add(new JProperty("Controls", children));
            }
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="ancestorNode"></param>
        /// <param name="root"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, Version version)
        {
            IProjectPageViewModel result = DeserializePageViewModel(root, ancestorNode, version);
            DeserializeChildControls(root, version, result);
            return result;
        }
        /// <summary>
        /// 反序列化页面视图模型
        /// </summary>
        /// <param name="root"></param>
        /// <param name="ancestorNode"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected virtual IProjectPageViewModel DeserializePageViewModel(JObject root, IExplorerNodeViewModel ancestorNode, Version version)
        {
            string path = Path.Combine((string)root["Folder"]!, (string)root["Name"]!);
            string name = (string)root["Name"]!;
            var projectPageViewModel = CreatePageViewModel(root, ancestorNode, name, path);
            CreateOrphanNavigationTargetViewModel(projectPageViewModel);

            projectPageViewModel.Size = ((string?)root["Size"]?["Size"]) ?? projectPageViewModel.Size;
            if ((string?)root["Background"] != null)
                projectPageViewModel.Background = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString((string)root["Background"]!)).GetAsFrozen();
            projectPageViewModel.UsePageNameAsSignalPrefix = ((bool?)root["UsePageNameAsSignalPrefix"]) ?? projectPageViewModel.UsePageNameAsSignalPrefix;
            projectPageViewModel.SignalPrefix = ((string?)root["SignalPrefix"]) ?? projectPageViewModel.SignalPrefix;
            projectPageViewModel.StretchPages = ((bool?)root["StretchContent"]) ?? projectPageViewModel.StretchPages;
            
            this.projectPageViewModel = projectPageViewModel;
            return projectPageViewModel;
        }
        /// <summary>
        /// 创建页面视图模型
        /// </summary>
        /// <param name="root">序列化数据</param>
        /// <param name="parentNode">上级节点</param>
        /// <param name="name">名称</param>
        /// <param name="path">序列化文件路径</param>
        /// <returns></returns>
        protected virtual IProjectPageViewModel CreatePageViewModel(JObject root, IExplorerNodeViewModel parentNode, string name, string path)
        {
            return new ProjectPageViewModel(Container, parentNode, name);
        }
        // 反序列化页面子控件
        private void DeserializeChildControls(JObject page, Version version, IProjectPageViewModel pageViewModel)
        {
            var controls = page["Controls"]?.OfType<JObject>() ?? Enumerable.Empty<JObject>();
            ControlSerializerExtensions.DeserializeControlList(pageViewModel, controls, pageViewModel.Children, version, this.SerializerFactory);
        }

        // 创建孤立的导航目标节点, 页面根节点下只创建一个该节点
        private void CreateOrphanNavigationTargetViewModel(IProjectPageViewModel pageViewModel)
        {
            string typeName = "HMI.ProjectDesigner.DesignControls.Navigation.HMINavigationTarget";
            string assembly = "HMI.ProjectDesigner.DesignControls";
            Type controlType = ReflectionHelper.GetType(assembly, typeName);
            OrphanNavigationTargetViewModel item = new OrphanNavigationTargetViewModel(this.Container, pageViewModel, controlType);
            pageViewModel.Children.Add(item);
        }

        /// <summary>
        /// 反序列化结束事件，在此执行延迟操作
        /// </summary>
        /// <param name="viewModel"></param>
        public override void OnDeserializationFinished(IExplorerNodeViewModel viewModel)
        {
            base.OnDeserializationFinished(viewModel);

            RemoveOrphanNavigationTargetViewModel();
        }
        // 移除孤立的导航目标节点
        private void RemoveOrphanNavigationTargetViewModel()
        {
            IExplorerNodeViewModel explorerNodeViewModel = this.projectPageViewModel.Children.Single(t=>t is OrphanNavigationTargetViewModel);
            if (!explorerNodeViewModel.Children.Any())
            {
                this.projectPageViewModel.Children.Remove(explorerNodeViewModel);
            }
        }

    }
}
