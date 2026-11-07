// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared;

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
    public DateRange DateRange { get; set; }
    public DateTime TextualStartDate { get; set; }
    public DateTime? TextualEndDate { get; set; }
    public int Number { get; set; }
}
