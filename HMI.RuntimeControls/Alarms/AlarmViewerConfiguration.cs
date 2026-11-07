// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.ObjectModel;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Alarms;

public enum AlarmCountDisplayMode
{
    FilterAndProject,
    Filter,
    Project
}

public enum AlarmStatusFilter
{
    ActiveOrNotAcknowledged = 0,
    NotAcknowledged = 2,
    Active = 4,
    Gone = 8,
    All = 16
}

public enum ServerSortOrder
{
    DateDescending = 2,
    PriorityDescending = 4
}

public enum FilterColumnType
{
    None,
    Text,
    SignalName,
    OpcItem,
    Name,
    HttpLink,
    HelpCause,
    HelpEffect,
    HelpRepair,
    GeneralComment,
    OccurrenceComment,
    AcknowledgeComment,
    NavigationSource,
    NavigationTarget
}

public sealed class HMIAlarmViewerSecurityPolicies
{
    public ObservableCollection<SecurityPolicy> Help { get; } = [];
    public ObservableCollection<SecurityPolicy> Refresh { get; } = [];
    public ObservableCollection<SecurityPolicy> Filter { get; } = [];
    public ObservableCollection<SecurityPolicy> Acknowledge { get; } = [];
    public ObservableCollection<SecurityPolicy> Comments { get; } = [];
    public ObservableCollection<SecurityPolicy> ProcessingAndDisplay { get; } = [];
    public ObservableCollection<SecurityPolicy> SoundToggle { get; } = [];
    public ObservableCollection<SecurityPolicy> Navigate { get; } = [];
    public ObservableCollection<SecurityPolicy> OnlineMode { get; } = [];
    public ObservableCollection<SecurityPolicy> OfflineMode { get; } = [];
    public ObservableCollection<SecurityPolicy> CustomQueryMode { get; } = [];
    public ObservableCollection<SecurityPolicy> HttpLink { get; } = [];
}
