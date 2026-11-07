// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Media;
using HMI.Modules.Design.Resources;

namespace HMI.Modules.Design.Attributes.Validation.Rules
{
	public class ColorValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			ColorConverter colorConverter = new ColorConverter();
			BrushConverter brushConverter = new BrushConverter();
			if (value != null && !string.IsNullOrEmpty(value.ToString()))
			{
				string colorText = value.ToString();
				return new ValidationResult(colorConverter.IsValid(colorText) || brushConverter.IsValid(colorText), ValidationErrorTexts.InvalidValue);
			}
			return new ValidationResult(false, ValidationErrorTexts.InvalidValue);
		}
	}
}
