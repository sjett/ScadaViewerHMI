// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 控件序列化器扩展方法
    /// </summary>
    public static class ControlSerializerExtensions
    {
        /// <summary>
        /// 反序列化控件列表
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="controls"></param>
        /// <param name="controlList"></param>
        /// <param name="version"></param>
        /// <param name="serializerFactory"></param>
        public static void DeserializeControlList(IExplorerNodeViewModel parent, IEnumerable<JObject> controls, IList<IExplorerNodeViewModel> controlList, Version version, ISerializerFactory serializerFactory)
        {
            foreach (var current in controls)
            {
                Type controlType = CommonControlSerializer.GetControlType(current["Definition"]);
                if (controlType != null)
                {
                    ISerializer serializer = serializerFactory.GetSerializer(controlType);
                    IExplorerNodeViewModel item = serializer.Deserialize(parent, current, version) as IExplorerNodeViewModel;
                    controlList.Add(item);
                }
            }
        }
        /// <summary>
        /// 序列化子控件列表
        /// </summary>
        /// <param name="controls"></param>
        /// <param name="childrenElement"></param>
        /// <param name="serializerFactory"></param>
        public static void SerializeChildList(IEnumerable<ICommonControlViewModel> controls, List<JObject> childrenElement, ISerializerFactory serializerFactory)
        {
            foreach (ICommonControlViewModel current in controls)
            {
                SerializeChild(childrenElement, current, serializerFactory);
            }
        }
        /// <summary>
        /// 序列化子控件
        /// </summary>
        /// <param name="children"></param>
        /// <param name="child"></param>
        /// <param name="serializerFactory"></param>
        public static void SerializeChild(List<JObject> children, ICommonControlViewModel child, ISerializerFactory serializerFactory)
        {
            ISerializer serializer = serializerFactory.GetSerializer(child.ControlType);
            var xElement = serializer.Serialize(child);
            if (xElement != null)
            {
                children.Add(xElement);
            }
        }
    }
}
