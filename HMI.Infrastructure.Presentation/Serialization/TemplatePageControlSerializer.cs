// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 模板页面控件序列化器
    /// </summary>
    public class TemplatePageControlSerializer : PageControlSerializer
    {
        /// <summary>
        /// 模板页面控件序列化器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="serializerFactory"></param>
        /// <param name="projectManagerService"></param>
        public TemplatePageControlSerializer(IServiceProvider container, ISerializerFactory serializerFactory, IProjectManagerService projectManagerService)
            : base(container, serializerFactory, projectManagerService)
        {
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override JObject Serialize(object source)
        {
            ITemplatePageViewModel templatePageViewModel = source as ITemplatePageViewModel;
            var root = base.Serialize(source);
            var serializedParameters = new List<JObject>();
            foreach (IParameterDefinition parameterDefinition in templatePageViewModel.TemplateParameters)
            {
                ISerializer serializer = SerializerFactory.GetSerializer(parameterDefinition.GetType());
                var content = serializer.Serialize(parameterDefinition);
                serializedParameters.Add(content);
            }
            root.Add(new JProperty("TemplateParameters", serializedParameters));
            return root;
        }
        /// <summary>
        /// 反序列化升级后的对象
        /// </summary>
        /// <param name="ancestorNode"></param>
        /// <param name="root"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, Version version)
        {
            ITemplatePageViewModel templatePageViewModel = this.DeserializePageViewModel(root, ancestorNode, version) as ITemplatePageViewModel;
            var tempParams = root["TemplateParameters"].OfType<JObject>() ?? Enumerable.Empty<JObject>();
            ISerializer serializer = SerializerFactory.GetSerializer<ParameterDefinition>();//在设计器图元组件库中已经注册了ParameterDefinition的序列化器
            foreach (var temp in tempParams)
            {
                templatePageViewModel.TemplateParameters.Add(serializer.Deserialize(templatePageViewModel, temp, version) as ParameterDefinition);
            }
            DeserializeChildControls(root, version, templatePageViewModel);
            return templatePageViewModel;
        }
        /// <summary>
        /// 创建页面视图模型
        /// </summary>
        /// <param name="root">序列化数据</param>
        /// <param name="parentNode">上级节点</param>
        /// <param name="name">名称</param>
        /// <param name="path">序列化文件路径</param>
        /// <returns></returns>
        protected override IProjectPageViewModel CreatePageViewModel(JObject root, IExplorerNodeViewModel parentNode, string name, string path)
        {
            return new TemplatePageViewModel(Container, parentNode, name);
        }
        // 反序列化子控件
        private void DeserializeChildControls(JObject root, Version version, IProjectPageViewModel parent)
        {
            var controls = root["Controls"]?.OfType<JObject>() ?? Enumerable.Empty<JObject>();
            ControlSerializerExtensions.DeserializeControlList(parent, controls, parent.Children, version, this.SerializerFactory);
        }
    }
}
