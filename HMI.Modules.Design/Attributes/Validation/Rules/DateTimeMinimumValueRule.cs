// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Modules.Design.Attributes.Validation.Rules
{
	public class DateTimeMinimumValueRule : DateTimeRangeValidationRule
	{
		public DateTimeMinimumValueRule(DateTime? minValue) : base(minValue, null)
		{
		}
	}
}
