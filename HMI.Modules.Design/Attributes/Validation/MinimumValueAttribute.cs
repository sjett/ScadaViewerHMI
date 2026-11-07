// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Attributes.Validation.Rules;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Windows.Controls;
namespace HMI.Modules.Design.Attributes.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class MinimumValueAttribute : RangeValidationAttribute
	{
		public double? NumericMinimumValue
		{
			get;
			private set;
		}
		public string MinimumValueProperty
		{
			get;
			private set;
		}
		public DateTime? DateTimeMinimumValue
		{
			get;
			private set;
		}
		public MinimumValueAttribute(double value)
		{
            NumericMinimumValue = new double?(value);
		}
		public MinimumValueAttribute(DateTime? value)
		{
            DateTimeMinimumValue = value;
		}
		public MinimumValueAttribute(string propertyName)
		{
			Contract.Requires<ArgumentNullException>(propertyName != null, "propertyName");
			Contract.Requires<ArgumentException>(propertyName.Length > 0, "propertyName");
            MinimumValueProperty = propertyName;
		}
		protected override ValidationRule CreateValidationRule(object sourceObject, string propertyName)
		{
			if (NumericMinimumValue.HasValue)
			{
				return new NumericMinimumValueRule(NumericMinimumValue.Value);
			}
			if (DateTimeMinimumValue.HasValue)
			{
				return new DateTimeMinimumValueRule(new DateTime?(DateTimeMinimumValue.Value));
			}
			if (!IsPropertyOfType<DateTime>(sourceObject, MinimumValueProperty) && !IsPropertyOfType<DateTime?>(sourceObject, MinimumValueProperty))
			{
				return new DynamicNumericMinimumValueRule(sourceObject, MinimumValueProperty);
			}
			return new DynamicDateTimeMinimumValueRule(sourceObject, MinimumValueProperty);
		}
	}
}
