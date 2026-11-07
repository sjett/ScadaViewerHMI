// SPDX-License-Identifier: GPL-3.0-only
using HMI.Modules.Design.Resources;
using NLog;
using System.Globalization;
using System.Reflection;

namespace HMI.Modules.Design.Attributes.Validation
{
	public static class Validator
	{
		//private sealed class Class15<TProperty, TRules>
		//{
		//	public TProperty property;
		//	public TRules rules;
		//	public Class15(TProperty property, TRules rules)
		//	{
		//		this.property = property;
		//		this.rules = rules;
		//	}
  //          public override string ToString()
  //          {
  //              StringBuilder stringBuilder = new StringBuilder();
  //              stringBuilder.Append("{ property = ");
  //              stringBuilder.Append(this.property);
  //              stringBuilder.Append(", rules = ");
  //              stringBuilder.Append(this.rules);
  //              stringBuilder.Append(" }");
  //              return stringBuilder.ToString();
  //          }
  //          public override bool Equals(object obj)
  //          {
  //              Class15<TProperty, TRules> @class = obj as Class15<TProperty, TRules>;

  //              return @class != null && 
		//			EqualityComparer<TProperty>.Default.Equals(this.property, @class.property) && 
		//			EqualityComparer<TRules>.Default.Equals(this.rules, @class.rules);
  //          }
            
  //          public override int GetHashCode()
  //          {
  //              return EqualityComparer<TProperty>.Default.GetHashCode(this.property) ^ EqualityComparer<TRules>.Default.GetHashCode(this.rules);
  //          }
  //      }
        private record Temp(PropertyInfo property, object[] rules);

        private static readonly Logger Logger = LogManager.GetLogger("Validator");
		
		public static bool IsValid(object objectToValidate)
		{
			if (objectToValidate == null)
			{
				return true;
			}

			PropertyInfo[] properties = objectToValidate
				.GetType()
				.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetProperty);

			var propertiesWithValidationErrors = properties
                .Select(p =>  new Temp(p, p.GetCustomAttributes(typeof(ValidationRuleAttribute), true)) )
				.Where(t => t.rules
							.Cast<ValidationRuleAttribute>()
							.Any(rule => !Validate(rule, t.property, objectToValidate)) );

			return !propertiesWithValidationErrors.Select(t => t.property).Any();
		}
		
        private static bool Validate(ValidationRuleAttribute validationRule, PropertyInfo property, object source)
        {
            bool result;
            try
            {// 调用验证规则进行验证
                result = validationRule.GetValidationRule(source, property.Name).Validate(property.GetValue(source, null), CultureInfo.CurrentCulture).IsValid;
            }
            catch (Exception exception)
            {
                Logger.WarnException(ErrorTexts.AnErrorHasOccurredWhilePerformingValidation, exception);
                result = false;
            }
            return result;
        }

	}
}
