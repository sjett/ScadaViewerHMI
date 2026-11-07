// SPDX-License-Identifier: GPL-3.0-only
using System.Globalization;
using System.Text.RegularExpressions;

namespace HMI.Infrastructure.Converters
{
    public class QuadDoubleConverterBase
	{
		private const string GeneralFormat = "g";
		
		public  virtual Tuple<double, double, double, double> QuadDoubleFromString(string s)
		{
			string[] values = ExtractNumericValues(s);
			double[] parsedValues = ParseValues(values);
			return CreateQuadruple(parsedValues, values.Length);
		}
        private double[] ParseValues(string[] values)
        {
            double[] parsedValues = new double[4];
            int valueIndex = 0;
            while (valueIndex < values.Length && valueIndex < 4)
            {
                double.TryParse(values[valueIndex], NumberStyles.Number, CultureInfo.InvariantCulture, out parsedValues[valueIndex]);
                valueIndex++;
            }
            return parsedValues;
        }
        private Tuple<double, double, double, double> CreateQuadruple(double[] values, int valueCount)
        {
            if (valueCount == 1)
            {
                return new Tuple<double, double, double, double>(values[0], values[0], values[0], values[0]);
            }
            if (valueCount == 2)
            {
                return new Tuple<double, double, double, double>(values[0], values[1], values[0], values[1]);
            }

            return new Tuple<double, double, double, double>(values[0], values[1], values[2], values[3]);
        }
        private string[] ExtractNumericValues(string value)
        {
            IEnumerable<Match> matches = Regex.Matches(value ?? string.Empty, "\\d*\\.?\\d+").OfType<Match>();

            return matches.Select(match => match.Value).ToArray();
        }
        public virtual string QuadDoubleToString(Tuple<double, double, double, double> quad, string separator = ",", double epsilon = 0.0001)
		{
			if (AreDifferent(quad.Item4, quad.Item2, epsilon) || AreDifferent(quad.Item3, quad.Item1, epsilon))
			{
				string[] array = new string[4];
				array[0] = quad.Item1.ToString(GeneralFormat, CultureInfo.InvariantCulture);
				array[1] = quad.Item2.ToString(GeneralFormat, CultureInfo.InvariantCulture);
				array[2] = quad.Item3.ToString(GeneralFormat, CultureInfo.InvariantCulture);
				array[3] = quad.Item4.ToString(GeneralFormat, CultureInfo.InvariantCulture);
				return string.Join(separator, array);
			}
			if (AreDifferent(quad.Item2, quad.Item1, epsilon))
			{
				string[] twoValueStrings = new string[2];
				twoValueStrings[0] = quad.Item1.ToString(GeneralFormat, CultureInfo.InvariantCulture);
				twoValueStrings[1] = quad.Item2.ToString(GeneralFormat, CultureInfo.InvariantCulture);
				return string.Join(separator, twoValueStrings);
			}
			return quad.Item1.ToString(GeneralFormat, CultureInfo.InvariantCulture);
		}
		public static bool AreDifferent(double x, double y, double epsilon)
		{
			return !AreClose(x, y, epsilon);
		}
		public static bool AreClose(double x, double y, double epsilon)
		{
			return Math.Abs(x - y) < epsilon;
		}
		
	}
}
