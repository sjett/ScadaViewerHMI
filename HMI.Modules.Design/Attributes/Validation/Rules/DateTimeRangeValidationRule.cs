// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Resources;
using System.Globalization;
using System.Windows.Controls;
namespace HMI.Modules.Design.Attributes.Validation.Rules
{
	public class DateTimeRangeValidationRule : ValidationRule
	{
		protected internal DateTime? MaxValue
		{
			protected get;
			set;
		}
		protected internal DateTime? MinValue
		{
			protected get;
			set;
		}
		public DateTimeRangeValidationRule(DateTime? minValue, DateTime? maxValue)
		{
            MinValue = minValue;
            MaxValue = maxValue;
		}
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			ValidationResult result;
			try
			{
				if (value != null)
				{
					DateTime t = Convert.ToDateTime(value);
					if (MinValue.HasValue && t < MinValue.Value)
					{
						result = new ValidationResult(false, string.Format(cultureInfo, ValidationErrorTexts.ValueMustNotBeLessThan, new object[]
						{
                            MinValue
                        }));
						return result;
					}
					if (MaxValue.HasValue && t > MaxValue.Value)
					{
						result = new ValidationResult(false, string.Format(cultureInfo, ValidationErrorTexts.ValueMustNotBeGreaterThan, new object[]
						{
                            MaxValue
                        }));
						return result;
					}
				}
				result = ValidationResult.ValidResult;
			}
			catch (InvalidCastException)
			{
				result = new ValidationResult(false, ValidationErrorTexts.InvalidValue);
			}
			catch (FormatException)
			{
				result = new ValidationResult(false, ValidationErrorTexts.InvalidValue);
			}
			return result;
		}
	}
}
