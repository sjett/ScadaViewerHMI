// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Modules.Design.Attributes.Validation.Rules
{
	public class NumericMaximumValueRule : NumericRangeValidationRule
	{
        public NumericMaximumValueRule(double maxValue)
            : base(double.MinValue, maxValue)
		{
            
		}
	}
}
