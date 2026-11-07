// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Modules.Design.Attributes.Validation.Rules
{
	public class DateTimeMaximumValueRule : DateTimeRangeValidationRule
	{
		public DateTimeMaximumValueRule(DateTime? maxValue) : base(null, maxValue)
		{
		}
	}
}
