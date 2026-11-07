// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Modules.Design.Attributes.Validation.Rules
{
	public class NumericMinimumValueRule : NumericRangeValidationRule
	{
		public NumericMinimumValueRule(double minValue) : base(minValue, double.MaxValue)
		{
		}
	}
}
