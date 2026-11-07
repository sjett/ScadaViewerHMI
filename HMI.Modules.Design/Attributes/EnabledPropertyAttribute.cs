// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Modules.Design.Attributes
{
    /// <summary>
    /// 启用属性特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EnabledPropertyAttribute : Attribute
    {
        /// <summary>
        /// 是否启用属性名称
        /// </summary>
        public string IsEnablePropertyName
        {
            get;
            set;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="isEnablePropertyName"></param>
        public EnabledPropertyAttribute(string isEnablePropertyName)
        {
            IsEnablePropertyName = isEnablePropertyName;
        }
    }
}
