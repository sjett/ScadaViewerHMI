// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Resources;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
namespace HMI.Modules.Design.Attributes.Validation.Rules
{
	public class DynamicDateTimeRangeValidationRule : DateTimeRangeValidationRule
	{
		protected internal string MinPropertyName
		{
			get;
			protected set;
		}
		protected internal string MaxPropertyName
		{
			get;
			protected set;
		}
		protected internal object Source
		{
			get;
			protected set;
		}
		public DynamicDateTimeRangeValidationRule(object source, string minPropertyName, string maxPropertyName) : base(null, null)
		{
			Contract.Requires<ArgumentNullException>(source != null, "source");
            Source = source;
            MaxPropertyName = maxPropertyName;
            MinPropertyName = minPropertyName;
		}
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			try
			{
				UpdateMinimumValue();
				UpdateMaximumValue();
			}
			catch (InvalidCastException)
			{
				return new ValidationResult(false, ValidationErrorTexts.InvalidValue);
			}
			catch (FormatException)
			{
				return new ValidationResult(false, ValidationErrorTexts.InvalidValue);
			}
			catch (OverflowException)
			{
				return new ValidationResult(false, ValidationErrorTexts.InvalidValue);
			}
			return base.Validate(value, cultureInfo);
		}

        private void UpdateMinimumValue()
        {
            MinValue = null;
            if (Source != null && !string.IsNullOrEmpty(MinPropertyName))
            {
                object minimumValue = GetRangeValue(MinPropertyName, Source);
                if (minimumValue != null)
                {
                    MinValue = new DateTime?(Convert.ToDateTime(minimumValue));
                }
            }
        }

        private void UpdateMaximumValue()
        {
            MaxValue = null;
            if (Source != null && !string.IsNullOrEmpty(MaxPropertyName))
            {
                object maximumValue = GetRangeValue(MaxPropertyName, Source);
                if (maximumValue != null)
                {
                    MaxValue = new DateTime?(Convert.ToDateTime(maximumValue));
                }
            }
        }
        private object GetRangeValue(string propertyName, object source)
        {
            PropertyInfo property = source.GetType().GetProperty(propertyName);
            return property.GetValue(source, null);
        }
    }
}
