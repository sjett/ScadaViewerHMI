// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Attributes.Validation.Rules;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Windows.Controls;
namespace HMI.Modules.Design.Attributes.Validation
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class MaximumValueAttribute : RangeValidationAttribute
	{
		public double? NumericMaximumValue
		{
			get;
			private set;
		}
		public string MaximumValueProperty
		{
			get;
			private set;
		}
		public DateTime? DateTimeMaximumValue
		{
			get;
			private set;
		}
		public MaximumValueAttribute(double value)
		{
            NumericMaximumValue = new double?(value);
		}
		public MaximumValueAttribute(DateTime? value)
		{
            DateTimeMaximumValue = value;
		}
		public MaximumValueAttribute(string propertyName)
		{
			Contract.Requires<ArgumentNullException>(propertyName != null, "propertyName");
			Contract.Requires<ArgumentException>(propertyName.Length > 0, "propertyName");
            MaximumValueProperty = propertyName;
		}
		protected override ValidationRule CreateValidationRule(object sourceObject, string propertyName)
		{
			if (NumericMaximumValue.HasValue)
			{
				return new NumericMaximumValueRule(NumericMaximumValue.Value);
			}
			if (DateTimeMaximumValue.HasValue)
			{
				return new DateTimeMaximumValueRule(new DateTime?(DateTimeMaximumValue.Value));
			}
			if (!IsPropertyOfType<DateTime>(sourceObject, MaximumValueProperty) && !IsPropertyOfType<DateTime?>(sourceObject, MaximumValueProperty))
			{
				return new DynamicNumericMaximumValueRule(sourceObject, MaximumValueProperty);
			}
			return new DynamicDateTimeMaximumValueRule(sourceObject, MaximumValueProperty);
		}
	}
}
