// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Resources;
using System.Globalization;
using System.Windows.Controls;

namespace HMI.Modules.Design.Attributes.Validation.Rules
{
    /// <summary>
    /// 非空验证规则
    /// </summary>
    public class NotNullValidationRule : ValidationRule
    {
        /// <summary>
        /// 验证值不为null
        /// </summary>
        /// <param name="value"></param>
        /// <param name="cultureInfo"></param>
        /// <returns></returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return new ValidationResult(value != null, ValidationErrorTexts.ValueMustNotBeNull);
        }
    }
}
