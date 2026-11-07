// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
namespace HMI.Infrastructure.Presentation.Serialization
{
    /// <summary>
    /// 页面序列化器
    /// </summary>
    public class PageSerializer : PageControlSerializer
    {
        public PageSerializer(IServiceProvider container, ISerializerFactory serializerFactory, IProjectManagerService projectManagerService)
            : base(container, serializerFactory, projectManagerService)
        {
        }
        //protected override XElement UpgradeXml(XElement root, Version version)
        //{
        //    if (version < new Version(3, 0, 3, 0))
        //    {
        //        root = base.UpgradeXml(root, version);
        //        string staticTextUpdate_v3_0_2__to__v3_0_ = SerializationUpdateTemplates.StaticTextUpdate_v3_0_2__to__v3_0_3;
        //        root = base.TransformXmlElement(root, staticTextUpdate_v3_0_2__to__v3_0_, version);
        //    }
        //    return base.UpgradeXml(root, version);
        //}
    }
}
