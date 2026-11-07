// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
namespace HMI.Runtime.Core.Helper.Filter
{
	public class FilterBuilder : IFilterBuilder
	{
		public Predicate<object> CreateFilter(string text)
		{
			if (string.IsNullOrWhiteSpace(text))
			{
				return null;
			}
			Regex filter = CreateRegex(text);
			return new Predicate<object>(value => MatchesFilter(value, filter));
		}
		private Regex CreateRegex(string text)
		{
			string regexPattern = ConvertWildcardToRegex(text);
            return new Regex(regexPattern, RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);// 513
		}
		private bool MatchesFilter(object value, Regex filter)
		{
			return value != null && filter.IsMatch(value.ToString());
		}
		private string ConvertWildcardToRegex(string searchText)
		{
			string text = searchText.ToLower().Replace("*", ".*");
			return "^" + text + "$";
		}
	}
}
