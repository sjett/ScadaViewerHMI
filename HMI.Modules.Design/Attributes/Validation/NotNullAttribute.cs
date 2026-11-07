// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Attributes.Validation.Rules;
using System.Windows.Controls;

namespace HMI.Modules.Design.Attributes.Validation
{
    /// <summary>
    /// 非空验证属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class NotNullAttribute : ValidationRuleAttribute
    {
        /// <summary>
        /// 创建验证规则
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected override ValidationRule CreateValidationRule(object sourceObject, string propertyName)
        {
            return new NotNullValidationRule();
        }
    }
}
