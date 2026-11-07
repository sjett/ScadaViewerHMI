// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Windows.Data;
using HMI.Infrastructure.Presentation;
using HMI.Modules.Design.Resources;
using Range = HMI.Infrastructure.Presentation.Range;

namespace HMI.Modules.Design.Converters
{
	public class RangeToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Range range = (Range)value;
			DateRange dateRange = range.dateRange;
			DateTime startDate = range.startDate;
			DateTime? endDate = range.endDate;
			switch (dateRange)
			{
			case DateRange.None:
				return DateRangeTexts.Format_None;
			case DateRange.Day:
				return string.Format(DateRangeTexts.Format_Day, startDate.Date);
			case DateRange.Week:
			{
				DateTime dateTime = RangeToStringConverter.StartOfWeek(startDate.Date, CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
				DateTime dateTime2 = dateTime.AddDays(7.0).AddTicks(-1L);
				return string.Format(DateRangeTexts.Format_Week, dateTime, dateTime2);
			}
			case DateRange.Month:
			{
				DateTime dateTime3 = new DateTime(startDate.Year, startDate.Month, 1);
				DateTime dateTime4 = dateTime3.AddMonths(1).AddTicks(-1L);
				return string.Format(DateRangeTexts.Format_Month, dateTime3, dateTime4);
			}
			case DateRange.Year:
			{
				DateTime dateTime5 = new DateTime(startDate.Year, 1, 1);
				DateTime dateTime6 = dateTime5.AddYears(1).AddTicks(-1L);
				return string.Format(DateRangeTexts.Format_Year, dateTime5, dateTime6);
			}
			case DateRange.Range:
				return string.Format(DateRangeTexts.Format_Range, startDate, endDate);
			case DateRange.Yesterday:
				return DateRangeTexts.Format_Yesterday;
			case DateRange.CurrentDay:
				return DateRangeTexts.Format_CurrentDay;
			case DateRange.LastNDays:
				return string.Format(DateRangeTexts.Format_LastNDays, range.number);
			case DateRange.LastNHours:
				return string.Format(DateRangeTexts.Format_LastNHours, range.number);
			case DateRange.LastNMinutes:
				return string.Format(DateRangeTexts.Format_LastNMinutes, range.number);
			default:
				throw new ArgumentOutOfRangeException();
			}
		}
		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotSupportedException();
		}
		public static DateTime StartOfWeek(DateTime dt, DayOfWeek startOfWeek)
		{
			int num = dt.DayOfWeek - startOfWeek;
			if (num < 0)
			{
				num += 7;
			}
			return dt.AddDays((double)(-(double)num)).Date;
		}
	}
}
