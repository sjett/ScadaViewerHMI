// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using Newtonsoft.Json.Linq;

namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 空序列化器
    /// </summary>
    public class NullSerializer : BaseSerializer
    {
        public NullSerializer(IServiceProvider container, ISerializerFactory serializerFactory)
            : base(container, serializerFactory)
        {
        }
        /// <summary>
        /// 序列化, 返回空对象
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public override JObject Serialize(object source)
        {
            return new JObject();//"{}" //"Dummy"
        }
    }
}
