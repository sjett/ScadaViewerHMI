// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Domain.Services
{
    /// <summary>
    /// 提供显式属性元数据的接口
    /// </summary>
    public interface IExplicitPropertyMetadataProvider
    {
        /// <summary>
        /// 获取属性的特性集合
        /// </summary>
        /// <param name="propertyName">属性名称</param>
        /// <returns></returns>
        ICollection<Attribute>? GetAttributes(string propertyName);
    }
}
