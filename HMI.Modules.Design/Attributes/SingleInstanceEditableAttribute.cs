// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Modules.Design.Attributes
{
    /// <summary>
    /// 单实例可编辑属性特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SingleInstanceEditableAttribute : Attribute
    {
    }
}
