// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;

namespace HMI.Modules.Design.Attributes.Validation.Rules
{
    /// <summary>
    /// 自定义方法验证规则
    /// </summary>
    public class CustomMethodValidationRule : ValidationRule
    {
        private readonly object source;
        private readonly string validationMethodName;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="source"></param>
        /// <param name="validationMethodName"></param>
        public CustomMethodValidationRule(object source, string validationMethodName)
        {
            Contract.Requires<ArgumentNullException>(source != null, "source");
            Contract.Requires<ArgumentNullException>(validationMethodName != null, "validationMethodName");
            Contract.Requires<ArgumentException>(validationMethodName.Length > 0, "validationMethodName");
            this.source = source;
            this.validationMethodName = validationMethodName;
        }
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        /// <exception cref="MissingMethodException"></exception>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            MethodInfo? method = source.GetType().GetMethod(validationMethodName, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.FlattenHierarchy);
            if (!(method != null))
            {
                throw new MissingMethodException(source.GetType().Name, validationMethodName);
            }
            return method.Invoke(source, new object[]
            {
                value,
                cultureInfo
            }) as ValidationResult;
        }
    }
}
