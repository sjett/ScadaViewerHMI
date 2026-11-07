// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core.Helper.Models;
using HMI.Runtime.Communication;
using System;
using System.Collections.Generic;
namespace HMI.Runtime.Core
{
    public delegate void OnCommunicationErrorEventHandler(string serverName, Exception error, int result);
    public delegate void OnRegisterErrorEventHandler(string serverName, Exception error, int result, int[] signalResults, string[] signalNames);
    public delegate void OnResultDelegate(Exception? error, int[] results);
    public delegate void OnSignalChangedDelegate(string name, object value);

    /// <summary>

    /// </summary>
    public interface IHMIConnector : IDisposable, IHMISignalInformation, IHMITranslationProvider, IHMISecurity
	{
		event OnCommunicationErrorEventHandler CommunicationError;
		event OnRegisterErrorEventHandler RegisterError;

		IConnectorStatistics Statistics
		{
			get;
		}
		void RegisterSignalChangedHandler(string serverName, string signalName, OnSignalChangedDelegate onSignalChangedDelegate);
		void UnregisterSignalChangedHandler(string serverName, string signalName, OnSignalChangedDelegate onSignalChangedDelegate);
		void RegisterSignal(string serverName, string signalName);
		void UnregisterSignal(string serverName, string signalName);
		object GetSignalValue(string serverName, string signalName);
        /// <summary>Queries unacknowledged events, not current active alarm states.</summary>
        Task<IReadOnlyList<UnacknowledgedEvent>> GetUnacknowledgedEventsAsync(
            string serverName, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);
        /// <summary>Queries alarm events in the specified time range, including their acknowledgement state.</summary>
        Task<IReadOnlyList<AlarmHistoryEvent>> GetAlarmHistoryAsync(
            string serverName, DateTimeOffset startTime, DateTimeOffset endTime, CancellationToken cancellationToken = default);
        /// <summary>Queries the RapidSCADA event stream for the specified time range.</summary>
        Task<IReadOnlyList<OperationEvent>> GetOperationEventsAsync(
            string serverName, DateTimeOffset startTime, DateTimeOffset endTime, int maxEvents,
            CancellationToken cancellationToken = default);
        /// <summary>Gets the hierarchical alarm group and type filters available on a server.</summary>
        Task<AlarmFilterMetadata> GetAlarmFilterMetadataAsync(
            string serverName, CancellationToken cancellationToken = default);
        /// <summary>Queries historical samples for the specified RapidSCADA channels.</summary>
        Task<IReadOnlyList<HistoricalTrendSeries>> GetHistoricalTrendsAsync(
            string serverName, IReadOnlyList<string> signalNames, DateTimeOffset startTime, DateTimeOffset endTime,
            int archiveBit = 0, CancellationToken cancellationToken = default);
        /// <summary>Lists signal names exposed by a server that provides a signal catalog.</summary>
        Task<IReadOnlyList<string>> GetSignalNamesAsync(
            string serverName, CancellationToken cancellationToken = default);
		void WriteSignal(string serverName, string signalName, object value);
		void WriteSignal(string serverName, string signalName, object value, OnResultDelegate resultDelegate);
		void WriteSignal(string serverName, string signalName, object value, string user, string password);
		void WriteSignal(string serverName, string signalName, object value, string user, string password, OnResultDelegate resultDelegate);
		void WriteSignal(string serverName, string signalName, object value, string user, string password, bool isDomainUser);
		void WriteSignal(string serverName, string signalName, object value, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void WriteSignal(string serverName, string signalName, object value, string password);
		void WriteSignal(string serverName, string signalName, object value, string password, OnResultDelegate resultDelegate);
		void ToggleSignal(string serverName, string signalName, object onValue, object offValue);
		void ToggleSignal(string serverName, string signalName, object onValue, object offValue, OnResultDelegate resultDelegate);
		void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation, string user, string password, OnResultDelegate resultDelegate);
		void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation, OnResultDelegate resultDelegate);
		void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation, string user, string password, bool isDomainUser);
		void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation, string user, string password);
		void SetAlarmState(string serverName, string AlarmId, HMIAlarmStates state, DateTime reactivation);
		void AcknowledgeAllAlarms(string serverName, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void AcknowledgeAllAlarms(string serverName);
		void AcknowledgeAllAlarms(string serverName, OnResultDelegate resultDelegate);
		void AcknowledgeAllAlarms(string serverName, string password);
		void AcknowledgeAllAlarms(string serverName, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAllAlarms(string serverName, string user, string password);
		void AcknowledgeAllAlarms(string serverName, string user, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAllAlarms(string serverName, string user, string password, bool isDomainUser);
		void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment);
		void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, OnResultDelegate resultDelegate);
		void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string password);
		void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string user, string password);
		void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string user, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAlarmsByGroup(string serverName, string groupName, string comment, string user, string password, bool isDomainUser);
		void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void AcknowledgeAllAlarmsWithComment(string serverName, string comment);
		void AcknowledgeAllAlarmsWithComment(string serverName, string comment, OnResultDelegate resultDelegate);
		void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string password);
		void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string user, string password);
		void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string user, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAllAlarmsWithComment(string serverName, string comment, string user, string password, bool isDomainUser);
		void AcknowledgeAllGoneAlarms(string serverName, string comment, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void AcknowledgeAllGoneAlarms(string serverName, string comment);
		void AcknowledgeAllGoneAlarms(string serverName, string comment, OnResultDelegate resultDelegate);
		void AcknowledgeAllGoneAlarms(string serverName, string comment, string password);
		void AcknowledgeAllGoneAlarms(string serverName, string comment, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAllGoneAlarms(string serverName, string comment, string user, string password);
		void AcknowledgeAllGoneAlarms(string serverName, string comment, string user, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAllGoneAlarms(string serverName, string comment, string user, string password, bool isDomainUser);
		void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment);
		void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, OnResultDelegate resultDelegate);
		void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string password);
		void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string user, string password);
		void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string user, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAlarms(string serverName, IEnumerable<Guid> alarmIds, string comment, string user, string password, bool isDomainUser);
		void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void AcknowledgeAlarms(string serverName, Guid alarmId, string comment);
		void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, OnResultDelegate resultDelegate);
		void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string password);
		void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string user, string password);
		void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string user, string password, OnResultDelegate resultDelegate);
		void AcknowledgeAlarms(string serverName, Guid alarmId, string comment, string user, string password, bool isDomainUser);
		void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations, string user, string password, OnResultDelegate resultDelegate);
		void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations);
		void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations, OnResultDelegate resultDelegate);
		void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations, string user, string password, bool isDomainUser);
		void SetAlarmStates(string serverName, string[] AlarmIds, HMIAlarmStates[] states, DateTime[] reactivations, string user, string password);
		void SetSignalState(string serverName, string signalName, HMISignalState state, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void SetSignalState(string serverName, string signalName, HMISignalState state, string user, string password, OnResultDelegate resultDelegate);
		void SetSignalState(string serverName, string signalName, HMISignalState state, OnResultDelegate resultDelegate);
		void SetSignalState(string serverName, string signalName, HMISignalState state, string user, string password, bool isDomainUser);
		void SetSignalState(string serverName, string signalName, HMISignalState state, string user, string password);
		void SetSignalState(string serverName, string signalName, HMISignalState state);
		void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue, string user, string password, OnResultDelegate resultDelegate);
		void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue, OnResultDelegate resultDelegate);
		void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue, string user, string password, bool isDomainUser);
		void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue, string user, string password);
		void SetSignalState(string serverName, string signalName, HMISignalState state, object inactiveValue);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, string user, string password, OnResultDelegate resultDelegate);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, OnResultDelegate resultDelegate);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, string user, string password);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, string user, string password, bool isDomainUser);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues, string user, string password, OnResultDelegate resultDelegate);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues, string user, string password, bool isDomainUser, OnResultDelegate resultDelegate);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues, OnResultDelegate resultDelegate);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues, string user, string password);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues, string user, string password, bool isDomainUser);
		void SetSignalStates(string serverName, string[] signalNames, HMISignalState[] states, object[] inactiveValues);
		void OutputDebugText(HMIDebugLevel level, string textFormat, params object[] textFormatParams);
		void SetSecurityServer(string serverName);

		//void SetRefreshParameter(int refresh);
		//int GetRefreshParameter();
		//CommunicationKeys GetCommunicationKeys(string serverName);
	}
}
