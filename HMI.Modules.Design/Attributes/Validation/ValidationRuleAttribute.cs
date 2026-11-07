// SPDX-License-Identifier: GPL-3.0-only
using System.Windows.Controls;

namespace HMI.Modules.Design.Attributes.Validation
{
    /// <summary>
    /// 验证规则属性基类
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class ValidationRuleAttribute : Attribute
    {
        /// <summary>
        /// 验证步骤
        /// </summary>
        public ValidationStep ValidationStep
        {
            get;
            set;
        }
        /// <summary>
        /// 创建验证规则
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected abstract ValidationRule CreateValidationRule(object sourceObject, string propertyName);
        /// <summary>
        /// 获取验证规则
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public ValidationRule GetValidationRule(object sourceObject, string propertyName)
        {
            ValidationRule validationRule = CreateValidationRule(sourceObject, propertyName);
            validationRule.ValidationStep = ValidationStep;
            return validationRule;
        }
    }
}
