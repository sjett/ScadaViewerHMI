// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 弹出参数序列化扩展
    /// </summary>
    public static class PopupParametersSerializationExtensions
    {
        /// <summary>
        /// 序列化参数值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="parameterValues"></param>
        /// <returns></returns>
        public static JProperty SerializeParameterValues(this BaseSerializer serializer, IEnumerable<IParameterValue> parameterValues)
        {
            var xElement = new List<JObject>();
            ISerializer serializer2 = serializer.SerializerFactory.GetSerializer<ParameterValue>();
            foreach (IParameterValue current in parameterValues)
            {
                xElement.Add(serializer2.Serialize(current));
            }
            return new JProperty("PopupParameters", xElement);
        }
        /// <summary>
        /// 反序列化参数值
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="propertyCollectionsElement"></param>
        /// <param name="node"></param>
        /// <param name="parameterValues"></param>
        /// <param name="version"></param>
        public static void DeserializeParameterValues(this BaseSerializer serializer, JObject propertyCollectionsElement, IExplorerNodeViewModel node, IEnumerable<IParameterValue> parameterValues, Version version)
        {
            var xElement = propertyCollectionsElement["PopupParameters"]?.ToArray();
            if (xElement == null)
                return;

            var p_serializer = serializer.SerializerFactory.GetSerializer<ParameterValue>();
            IEnumerable<ParameterValue> arg_73_0 = xElement.Select(t => p_serializer.Deserialize(node, (JObject)t, version) as ParameterValue);

            Dictionary<string, ParameterValue> dictionary = arg_73_0.ToDictionary(t => t.Name);
            // 设置参数值
            foreach (IParameterValue current in parameterValues)
            {
                ParameterValue parameterValue;
                if (dictionary.TryGetValue(current.Name, out parameterValue))
                {
                    current.Value = parameterValue.Value;
                }
            }
        }
        
    }
}
