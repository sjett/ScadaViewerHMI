// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 控件组容器序列化器
    /// </summary>
    public class ControlGroupContainerSerializer : CommonControlSerializer
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serializerFactory"></param>
        /// <param name="container"></param>
        /// <param name="projectManagerService"></param>
        public ControlGroupContainerSerializer(ISerializerFactory serializerFactory, IServiceProvider container, IProjectManagerService projectManagerService)
            : base(serializerFactory, container, projectManagerService)
        {
        }
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="controlType"></param>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        protected override ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
        {
            return new ControlGroupContainerViewModel(Container, parentNode, controlType);
        }
        /// <summary>
        /// 反序列化集合
        /// </summary>
        /// <param name="propertyCollectionsElement"></param>
        /// <param name="node"></param>
        /// <param name="version"></param>
        protected override void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel node, Version version)
        {
            base.DeserializeCollections(propertyCollectionsElement, node, version);
            ControlGroupContainerViewModel groupContainer = node as ControlGroupContainerViewModel;
            DeserializeChildren(propertyCollectionsElement, groupContainer, version);
        }
        /// <summary>
        /// 序列化集合
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected override IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
        {
            var serializedCollections = base.SerializeCollections(source);
            ControlGroupContainerViewModel groupContainer = source as ControlGroupContainerViewModel;
            List<JProperty> serializedChildren = new List<JProperty>();
            SerializeChildren(serializedChildren, groupContainer);
            return serializedCollections.Concat(serializedChildren);
        }

        // 子控件反序列化与序列化
        private void DeserializeChildren(JObject properties, IControlGroupContainerViewModel groupContainer, Version version)
        {
            groupContainer.UpdateBoundsWhenChildrenChange = false;
            var childrenToken = properties["Children"]?.OfType<JObject>() ?? Enumerable.Empty<JObject>();
            ControlSerializerExtensions.DeserializeControlList(groupContainer, childrenToken, groupContainer.Children, version, this.SerializerFactory);
            groupContainer.UpdateBoundsWhenChildrenChange = true;
        }
        // 子控件序列化
        private void SerializeChildren(List<JProperty> serializedProperties, ControlGroupContainerViewModel groupContainer)
        {
            List<JObject> serializedChildren = new();
            ControlSerializerExtensions.SerializeChildList(groupContainer.Children.OfType<ICommonControlViewModel>(), serializedChildren, this.SerializerFactory);
            
            serializedProperties.Add(new JProperty("Children", serializedChildren)); //JSON路径: [Root] -> Properties -> Collections -> Children -> [{pname,pvalue},]
        }
    }
}
