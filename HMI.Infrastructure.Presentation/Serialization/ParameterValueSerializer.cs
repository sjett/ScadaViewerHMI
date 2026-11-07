// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 参数值序列化器
    /// </summary>
    public class ParameterValueSerializer : BaseSerializer
    {
        public ParameterValueSerializer(IServiceProvider container, ISerializerFactory serializerFactory)
            : base(container, serializerFactory)
        {
        }
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override JObject Serialize(object source)
        {
            ParameterValue value = source as ParameterValue;
            return SerializeParameterValue(value);
        }
        /// <summary>
        /// 反序列化升级后的对象
        /// </summary>
        /// <param name="ancestorNode"></param>
        /// <param name="root"></param>
        /// <param name="version"></param>
        /// <returns></returns>
        protected sealed override object GetDeserializedObjectFromUpgraded(IExplorerNodeViewModel ancestorNode, JObject root, Version version)
        {
            var newParam = new ParameterValue(Container, ancestorNode);
            newParam.Name = ((string?)root["Name"]) ?? newParam.Name;
            newParam.Value = ((string?)root["Value"]) ?? newParam.Value;
            return newParam;
        }
        /// <summary>
        /// 序列化参数值
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected JObject SerializeParameterValue(ParameterValue value)
        {
            var jo = new JObject
            {
                { "Name", value.Name },
                { "Value", value.Value }
            };
            return jo;
        }
    }
}
