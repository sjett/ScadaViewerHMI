// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Modules.Design.Attributes.Validation
{
	public abstract class RangeValidationAttribute : ValidationRuleAttribute
	{
		protected bool IsPropertyOfType<T>(object sourceObject, string property)
		{
			return sourceObject.GetType().GetProperty(property).PropertyType == typeof(T);
		}
	}
}
