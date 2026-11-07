// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;

namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 模板页面序列化器
    /// </summary>
    public class TemplatePageSerializer : TemplatePageControlSerializer
    {
        /// <summary>
        /// 模板页面序列化器
        /// </summary>
        /// <param name="container"></param>
        /// <param name="serializerFactory"></param>
        /// <param name="projectManagerService"></param>
        public TemplatePageSerializer(IServiceProvider container, ISerializerFactory serializerFactory, IProjectManagerService projectManagerService)
            : base(container, serializerFactory, projectManagerService)
        {
        }
        //protected override XElement UpgradeXml(XElement root, Version version)
        //{
        //    if (version < new Version(3, 2, 3, 58))
        //    {
        //        root = base.UpgradeXml(root, version);
        //        string parameterReferenceUpdate_v2_0__to__v3_2_3_ = SerializationUpdateTemplates.ParameterReferenceUpdate_v2_0__to__v3_2_3_58;
        //        root = base.TransformXmlElement(root, parameterReferenceUpdate_v2_0__to__v3_2_3_, version);
        //    }
        //    return base.UpgradeXml(root, version);
        //}
    }
}
