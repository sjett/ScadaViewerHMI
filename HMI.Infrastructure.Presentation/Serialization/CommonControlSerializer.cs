// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
using HMI.Common.Instrumentation.ErrorHandling;

namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 公共控件序列化器
    /// </summary>
    public class CommonControlSerializer : BaseSerializer
    {
        private readonly IProjectManagerService projectManagerService;// 项目管理服务
        //private readonly IProjectNameService projectNameService;// 项目名称服务
        private ICommonControlViewModel commonControlViewModel;// 公共控件视图模型

        /// <summary>
        /// 项目管理服务
        /// </summary>
        public IProjectManagerService ProjectManagerService
        {
            get
            {
                return projectManagerService;
            }
        }
        /// <summary>
        /// 公共控件序列化器
        /// </summary>
        /// <param name="serializerFactory"></param>
        /// <param name="container"></param>
        /// <param name="projectManagerService"></param>
        public CommonControlSerializer(
            ISerializerFactory serializerFactory, 
            IServiceProvider container, 
            IProjectManagerService projectManagerService) : base(container, serializerFactory)
        {
            Contract.Requires<ArgumentNullException>(container != null, "container");
            Contract.Requires<ArgumentNullException>(projectManagerService != null, "projectManagerService");

            this.projectManagerService = projectManagerService;
            //projectNameService = container.Resolve<IProjectNameService>();
        }
        /// <summary>
        /// 获取控件类型
        /// </summary>
        /// <param name="definitionElement"></param>
        /// <returns></returns>
        public static Type GetControlType(JToken definitionElement)
        {
            string typeName = (string)definitionElement["TypeName"]!;
            return ReflectionHelper.GetType("HMI.ProjectDesigner.DesignControls", typeName);
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override JObject? Serialize(object source)
        {
            ICommonControlViewModel commonControlViewModel = (ICommonControlViewModel)source;

            var result = new JObject()
            {
                new JProperty("Definition", new JObject()
                {
                    new JProperty("TypeName", commonControlViewModel.ControlType.AssemblyQualifiedName)
                }),
                new JProperty("ID", commonControlViewModel.ControlID),
                new JProperty("Left", commonControlViewModel.XTranslation),
                new JProperty("Top", commonControlViewModel.YTranslation),
                new JProperty("Width", commonControlViewModel.ControlWidth),
                new JProperty("Height", commonControlViewModel.ControlHeight),
                new JProperty("ContainerWidth", commonControlViewModel.Width),
                new JProperty("ContainerHeight", commonControlViewModel.Height),
                new JProperty("zIndex", commonControlViewModel.ZIndex),
                new JProperty("Rotate", commonControlViewModel.Angle),
                new JProperty("Properties", new JObject()
                {
                     new JProperty("Attributes", new JObject()
                     {
                        new JProperty("Opacity", commonControlViewModel.Opacity),
                        new JProperty("Name", commonControlViewModel.ID),
                        new JProperty("ShowPopup", commonControlViewModel.ShowTemplate),
                        new JProperty("PopupModality", commonControlViewModel.TemplateModality),
                        new JProperty("PopupSource", commonControlViewModel.TemplateSource),
                        new JProperty("PopupTitle", commonControlViewModel.TemplateTitle),
                        new JProperty("ToolTip", commonControlViewModel.ToolTip),
                        //this.SerializeProperties(commonControlViewModel).ToArray()
                     }),
                    new JProperty("Collections", new JObject())
                })
            };
            foreach(var p in SerializeProperties(commonControlViewModel))
            {
                if ((result["Properties"]!["Attributes"] as JObject)!.ContainsKey(p.Name))
                    (result["Properties"]!["Attributes"] as JObject)!.Remove(p.Name);

                (result["Properties"]!["Attributes"] as JObject)!.Add(p);
            }
            foreach (var p in SerializeCollections(commonControlViewModel))
            {
                if ((result["Properties"]!["Attributes"] as JObject)!.ContainsKey(p.Name))
                    (result["Properties"]!["Attributes"] as JObject)!.Remove(p.Name);

                (result["Properties"]!["Collections"] as JObject)!.Add(p);
            }

            return result;
        }
        /// <summary>
        /// 序列化属性
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected virtual IEnumerable<JProperty> SerializeProperties(ICommonControlViewModel source)
        {
            return Enumerable.Empty<JProperty>();
        }
        /// <summary>
        /// 序列化集合
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        protected virtual IEnumerable<JProperty> SerializeCollections(ICommonControlViewModel source)
        {
            List<JProperty> list = new List<JProperty>();
            if (source.IsTemplateEnabled)
            {
                list.Add(this.SerializeParameterValues(source.ParameterValues));
            }
            return list;
        }
        /// <summary>
        /// 反序列化专用控件属性
        /// </summary>
        /// <param name="root"></param>
        /// <param name="node"></param>
        /// <param name="version"></param>
        protected virtual void DeserializeSpecializedControlProperties(JObject root, ICommonControlViewModel node, Version version)
        {
            var properties = root["Properties"]?["Attributes"];
            if (properties != null)
                DeserializeProperties((JObject?)properties, node, version);

            var propertyCollectionsElement = root["Properties"]?["Collections"];
            if (propertyCollectionsElement != null && propertyCollectionsElement.HasValues)//?
                DeserializeCollections((JObject?)propertyCollectionsElement, node, version);
        }
        /// <summary>
        /// 反序列化集合
        /// </summary>
        /// <param name="propertyCollectionsElement"></param>
        /// <param name="node"></param>
        /// <param name="version"></param>
        protected virtual void DeserializeCollections(JObject? propertyCollectionsElement, ICommonControlViewModel node, Version version)
        {
            // 延迟反序列化参数值，确保所有均已反序列化 
            base.Deferred(() =>
            {
                this.DeserializeParameterValues(propertyCollectionsElement!, node, node.ParameterValues, version);

            });
        }
        /// <summary>
        /// 反序列化属性
        /// </summary>
        /// <param name="properties"></param>
        /// <param name="node"></param>
        /// <param name="version"></param>
        protected virtual void DeserializeProperties(JObject? properties, ICommonControlViewModel node, Version version)
        {
            if (properties == null) return;

            node.Opacity = (double?)properties["Opacity"] ?? node.Opacity;

            node.ShowTemplate = (bool?)properties["ShowPopup"] ?? node.ShowTemplate;

            node.TemplateModality = (properties["PopupModality"]?.ToObject<DialogModality?>()) ?? node.TemplateModality;
            // 延迟设置模板源，确保所有模板页均已反序列化
            Deferred(() =>
            {
                node.TemplateSource = node.TemplatePages.SingleOrDefault((v) => v.ID == (string?)properties["PopupSource"]) ?? node.TemplateSource;
            });

            node.TemplateTitle = string.IsNullOrEmpty((string?)properties["PopupTitle"]) ? node.TemplateTitle : (string)properties["PopupTitle"]!;

            node.ToolTip = string.IsNullOrEmpty((string?)properties["ToolTip"]) ? node.ToolTip : (string)properties["ToolTip"]!;
        }
        /// <summary>
        /// 创建节点
        /// </summary>
        /// <param name="controlType"></param>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        protected virtual ICommonControlViewModel CreateNode(Type controlType, IExplorerNodeViewModel parentNode)
        {
            return new CommonControlViewModel(Container, parentNode, controlType);
        }
        /// <summary>
        /// 反序列化升级后的对象
        /// </summary>
        /// <param name="ancestorNode"></param>
        /// <param name="root"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, Version version)
        {
            var xElement = root["Definition"];
            Type controlType = GetControlType(xElement!);
            if (controlType == null)
            {
                throw new InvalidOperationException("Unknown control type: " + xElement?.ToString());
            }
            //
            commonControlViewModel = CreateNode(controlType, ancestorNode);
            //合并注入属性

            DeserializeCommonControlProperties(root, commonControlViewModel);
            DeserializeSpecializedControlProperties(root, commonControlViewModel, version);
            return commonControlViewModel;
        }
        /// <summary>
        /// 反序列化公共控件属性
        /// </summary>
        /// <param name="root"></param>
        /// <param name="node"></param>
        protected void DeserializeCommonControlProperties(JObject root, ICommonControlViewModel node)
        {
            node.ID = (string?)root["Properties"]?["Attributes"]?["Name"] ?? node.ControlType.Name;

            node.ControlID = (Guid?)root["ID"] ?? node.ControlID;

            node.XTranslation = (double?)root["Left"] ?? node.XTranslation;

            node.YTranslation = (double?)root["Top"] ?? node.YTranslation;

            if ((double?)root["Rotate"] != null)
                node.Rotate((double)root["Rotate"]!, TransformMode.Absolute, false);

            node.ZIndex = (int?)root["zIndex"] ?? node.ZIndex;

            node.ControlWidth = (double?)root["Width"] ?? node.ControlWidth;
            node.Width = (double?)root["Width"] ?? node.Width;

            node.ControlHeight = (double?)root["Height"] ?? node.ControlHeight;
            node.Height = (double?)root["Height"] ?? node.Height;

            node.Opacity = (double?)root["Opacity"] ?? node.Opacity;

            node.Width = (double?)root["ContainerWidth"] ?? node.Width;

            node.Height = (double?)root["ContainerHeight"] ?? node.Height;
        }
    }
}
