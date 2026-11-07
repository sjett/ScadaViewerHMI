// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;

namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 参数序列化器
    /// </summary>
    public abstract class ParameterSerializer : BaseSerializer
    {
        public ParameterSerializer(IServiceProvider container, ISerializerFactory serializerFactory)
            : base(container, serializerFactory)
        {
        }
    }
}
