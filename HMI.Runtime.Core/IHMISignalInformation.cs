// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Core
{
    public delegate void OnSignalInformationChangedDelegate(string serverName, string[] signalNames, HMISignalDefinition[] signalDefinitions);

    public interface IHMISignalInformation
	{
		void RegisterSignalInformationChangedHandler(string serverName, string signalName, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate);
		void RegisterSignalInformationChangedHandler(string serverName, string[] signalNames, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate);
		void UnregisterSignalInformationChangedHandler(string serverName, string signalName, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate);
		void UnregisterSignalInformationChangedHandler(string serverName, string[] signalNames, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate);
	}
}
