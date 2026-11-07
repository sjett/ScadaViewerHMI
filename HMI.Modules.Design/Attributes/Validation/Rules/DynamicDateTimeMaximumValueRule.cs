// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.ErrorHandling;

namespace HMI.Modules.Design.Attributes.Validation.Rules
{
	public class DynamicDateTimeMaximumValueRule : DynamicDateTimeRangeValidationRule
	{
		public DynamicDateTimeMaximumValueRule(object source, string propertyName) : base(source, null, propertyName)
		{
			Contract.Requires<ArgumentNullException>(propertyName != null, "propertyName");
			Contract.Requires<ArgumentException>(propertyName.Length > 0, "propertyName");
		}
	}
}
