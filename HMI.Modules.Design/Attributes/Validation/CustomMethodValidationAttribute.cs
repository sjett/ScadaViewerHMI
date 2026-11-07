// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Attributes.Validation.Rules;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Windows.Controls;

namespace HMI.Modules.Design.Attributes.Validation
{
    /// <summary>
    /// 自定义方法验证属性
    /// </summary>
    public class CustomMethodValidationAttribute : ValidationRuleAttribute
    {
        private readonly string methodName;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="methodName"></param>
        public CustomMethodValidationAttribute(string methodName)
        {
            Contract.Requires<ArgumentNullException>(methodName != null, "methodName");
            Contract.Requires<ArgumentException>(methodName.Length > 0);
            this.methodName = methodName;
        }
        /// <summary>
        /// 创建验证规则
        /// </summary>
        /// <param name="sourceObject"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        protected override ValidationRule CreateValidationRule(object sourceObject, string propertyName)
        {
            return new CustomMethodValidationRule(sourceObject, methodName);
        }
    }
}
