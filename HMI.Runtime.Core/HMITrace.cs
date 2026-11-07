// SPDX-License-Identifier: GPL-3.0-only
using NLog;
using System.Globalization;

namespace HMI.Runtime.Core;

public sealed class HMITrace : IHMITrace
{
    private static readonly Logger Logger = LogManager.GetLogger("Runtime");

    public void OutputDebugText(HMIDebugLevel level, string textFormat, params object[] textFormatParams)
    {
        string message = string.Format(CultureInfo.CurrentCulture, textFormat, textFormatParams);
        switch (level)
        {
            case HMIDebugLevel.Error:
                Logger.Error(message);
                break;
            case HMIDebugLevel.Warning:
                Logger.Warn(message);
                break;
            default:
                Logger.Info(message);
                break;
        }
    }

    public void Dispose()
    {
    }
}
