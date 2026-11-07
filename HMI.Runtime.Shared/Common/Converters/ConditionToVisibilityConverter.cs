// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;

namespace HMI.Runtime.Shared.Common.Converters
{
	public class ConditionToVisibilityConverter : IValueConverter
	{
		public static class Signs
		{
			public const string Equal = "==";
			public const string Grater = ">";
			public const string Less = "<";
			public const string GraterEqual = ">=";
			public const string LessEqual = "<=";
			public const string Diferent = "!=";
		}
		private static readonly List<string> AvailableOperators;
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			int integerValue = (value is int) ? ((int)value) : int.MinValue;
			string conditionExpression = parameter as string;
			if (integerValue == int.MinValue || conditionExpression == null || Enumerable.FirstOrDefault<string>(ConditionToVisibilityConverter.AvailableOperators, new Func<string, bool>(conditionExpression.Contains)) == null)
			{
				return 1;
			}
			string comparisonOperator = Enumerable.First<string>(ConditionToVisibilityConverter.AvailableOperators, new Func<string, bool>(conditionExpression.Contains));
			string expectedValueText = conditionExpression.Replace(comparisonOperator, string.Empty);
			if (expectedValueText.Length != 1)
			{
				return 1;
			}
			return ConditionToVisibilityConverter.EvaluateIntegerCondition(integerValue, expectedValueText, comparisonOperator) ? 0 : 1;
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
		private static bool EvaluateIntegerCondition(int actualValue, string expectedValueText, string comparisonOperator)
		{
            int result = 0;
            if (int.TryParse(expectedValueText, NumberStyles.Number, CultureInfo.InvariantCulture, out result))
            {
                switch (comparisonOperator)
                {
                    case "==":
                        return (actualValue == result);

                    case ">":
                        return (actualValue > result);

                    case "<":
                        return (actualValue < result);

                    case ">=":
                        return (actualValue >= result);

                    case "<=":
                        return (actualValue <= result);

                    case "!=":
                        return (actualValue != result);
                }
            }
            return false;

		}
		static ConditionToVisibilityConverter()
		{
			// Note: this type is marked as 'beforefieldinit'.
			List<string> list = new List<string>();
			list.Add("==");
			list.Add("!=");
			list.Add(">=");
			list.Add("<=");
			list.Add("<");
			list.Add(">");
			ConditionToVisibilityConverter.AvailableOperators = list;
		}
	}
}
