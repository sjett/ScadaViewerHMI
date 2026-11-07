// SPDX-License-Identifier: GPL-3.0-only
using System.Globalization;
using System.Text.RegularExpressions;

namespace HMI.MathExpressions.Common;

public static class ExpressionNumberFormatConverter
{
    private static readonly Regex HexRegex = new(
        @"(?<prefix>\b|[^0-9A-Za-z_])0x(?<hex>[0-9A-Fa-f_]+)(?<suffix>\b|[^0-9A-Za-z_])",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    private static readonly Regex BinRegex = new(
        @"(?<prefix>\b|[^0-9A-Za-z_])(?<bin>[01_]+)[bB](?<suffix>\b|[^0-9A-Za-z_])",
        RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public static string ConvertNumbersToDecimal(string expression)
    {
        if (string.IsNullOrWhiteSpace(expression))
            return expression;

        expression = ConvertHex(expression);
        expression = ConvertBinary(expression);

        return expression;
    }

    private static string ConvertHex(string input)
    {
        return HexRegex.Replace(input, match =>
        {
            string hex = match.Groups["hex"].Value.Replace("_", "");
            string dec = Convert.ToInt64(hex, 16).ToString(CultureInfo.InvariantCulture);

            return $"{match.Groups["prefix"].Value}{dec}{match.Groups["suffix"].Value}";
        });
    }

    private static string ConvertBinary(string input)
    {
        return BinRegex.Replace(input, match =>
        {
            string bin = match.Groups["bin"].Value.Replace("_", "");
            string dec = Convert.ToInt64(bin, 2).ToString(CultureInfo.InvariantCulture);

            return $"{match.Groups["prefix"].Value}{dec}{match.Groups["suffix"].Value}";
        });
    }
}
