// SPDX-License-Identifier: GPL-3.0-only
using System.Globalization;

namespace HMI.RuntimeControls;

internal static class HistoricalTrendTimeRange
{
    public static bool TryCreate(
        DateTime? selectedDate,
        string? selectedTime,
        bool useUtc,
        out DateTimeOffset result)
    {
        result = default;
        if (selectedDate is not DateTime date ||
            !TimeOnly.TryParseExact(
                selectedTime,
                ["HH:mm:ss", "HH:mm"],
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out TimeOnly time))
        {
            return false;
        }

        DateTime dateTime = date.Date.Add(time.ToTimeSpan());
        if (useUtc)
        {
            result = new DateTimeOffset(DateTime.SpecifyKind(dateTime, DateTimeKind.Utc));
            return true;
        }

        try
        {
            result = new DateTimeOffset(DateTime.SpecifyKind(dateTime, DateTimeKind.Local));
            return true;
        }
        catch (ArgumentException)
        {
            return false;
        }
    }
}
