// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Resources;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.Reflection;
using System.Windows.Controls;
namespace HMI.Modules.Design.Attributes.Validation.Rules
{
	public class DynamicNumericRangeValidationRule : NumericRangeValidationRule
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
        public DynamicNumericRangeValidationRule(object source, string minPropertyName, string maxPropertyName)
            : base(double.MinValue, double.MaxValue)
		{
			Contract.Requires<ArgumentNullException>(source != null, "source");
            Source = source;
            MaxPropertyName = maxPropertyName;
            MinPropertyName = minPropertyName;
		}
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			object defaultRange = new Range<double, double>(double.MinValue, double.MaxValue);
			try
			{
				UpdateMinimumValue(defaultRange);
				UpdateMaximumValue(defaultRange);
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

        private void UpdateMinimumValue(object defaultRange)
        {
            object source = string.IsNullOrEmpty(MinPropertyName) ? defaultRange : Source;
            string propertyName = string.IsNullOrEmpty(MinPropertyName) ? "MinValue" : MinPropertyName;
            MinValue = Convert.ToDouble(GetRangeValue(propertyName, source));
        }
        private void UpdateMaximumValue(object defaultRange)
        {
            object source = string.IsNullOrEmpty(MaxPropertyName) ? defaultRange : Source;
            string propertyName = string.IsNullOrEmpty(MaxPropertyName) ? "MaxValue" : MaxPropertyName;
            MaxValue = Convert.ToDouble(GetRangeValue(propertyName, source));
        }
        private object GetRangeValue(string propertyName, object source)
        {
            PropertyInfo property = source.GetType().GetProperty(propertyName);
            return property.GetValue(source, null);
        }

        internal sealed record Range<T, U>(T MinValue, U MaxValue);
    }
}
