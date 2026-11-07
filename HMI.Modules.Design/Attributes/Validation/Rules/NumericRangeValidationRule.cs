// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Resources;
using System.Globalization;
using System.Windows.Controls;
namespace HMI.Modules.Design.Attributes.Validation.Rules
{
	public class NumericRangeValidationRule : ValidationRule
	{
		
		protected internal double MaxValue
		{
			protected get;
			set;
		}
		protected internal double MinValue
		{
			protected get;
			set;
		}
		public NumericRangeValidationRule(double minValue, double maxValue)
		{
            MinValue = minValue;
            MaxValue = maxValue;
		}
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			ValidationResult result;
			try
			{
				double num = Convert.ToDouble(value);
				if (num < MinValue)
				{
					result = new ValidationResult(false, string.Format(cultureInfo, ValidationErrorTexts.ValueMustNotBeLessThan, new object[]
					{
                        MinValue
                    }));
				}
				else
				{
					if (num > MaxValue)
					{
						result = new ValidationResult(false, string.Format(cultureInfo, ValidationErrorTexts.ValueMustNotBeGreaterThan, new object[]
						{
                            MaxValue
                        }));
					}
					else
					{
						result = ValidationResult.ValidResult;
					}
				}
			}
			catch (InvalidCastException)
			{
				result = new ValidationResult(false, ValidationErrorTexts.InvalidValue);
			}
			catch (FormatException)
			{
				result = new ValidationResult(false, ValidationErrorTexts.InvalidValue);
			}
			catch (OverflowException)
			{
				result = new ValidationResult(false, ValidationErrorTexts.InvalidValue);
			}
			return result;
		}
	}
}
