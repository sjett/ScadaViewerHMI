// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMI.Infrastructure
{
    public enum ColumnTypes
    {
        Symbol,
        EventType,
        Time,
        EventText,
        UserName,
        ClientMachine,
        ServerName,
        SignalName,
        SignalDescription,
        SignalUnit,
        OpcItemName,
        Connector,
        SignalGroup,
        AlarmText,
        AlarmGroup,
        AlarmType,
        AlarmAcknowledgedText,
        AlarmPriority,
        SignalNewValue,
        SignalOldValue,
        ResultCode,
        ResultText,
        AffectedUserName
    }
}
