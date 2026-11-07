// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMI.Infrastructure.Presentation
{
    public enum DateRange
    {
        None,
        Day,
        Week,
        Month,
        Year,
        Range,
        Yesterday,
        CurrentDay,
        LastNDays,
        LastNHours,
        LastNMinutes
    }

    public struct Range
    {
        public static readonly Range None = new Range(DateRange.None);
        public DateRange dateRange;
        public DateTime? endDate;
        public int number;
        public DateTime startDate;
        public Range(DateRange range)
        {
            dateRange = range;
            startDate = DateTime.MinValue;
            endDate = null;
            number = 0;
        }
        public Range(DateRange range, int count)
        {
            dateRange = range;
            startDate = DateTime.MinValue;
            endDate = null;
            number = count;
        }
        public Range(DateRange range, DateTime start, DateTime? end = null, int count = 0)
        {
            dateRange = range;
            startDate = start;
            endDate = end;
            number = count;
        }
        public override bool Equals(object obj)
        {
            return obj is Range && this == (Range)obj;
        }
        public static bool operator ==(Range a, Range b)
        {
            return a.dateRange == b.dateRange && a.startDate == b.startDate && a.endDate == b.endDate && a.number == b.number;
        }
        public static bool operator !=(Range a, Range b)
        {
            return !(a == b);
        }
    }
}
