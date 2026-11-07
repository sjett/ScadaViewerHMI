// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Core.Helper
{
	public interface IHMISignalInformation
	{
		void RegisterSignalInformationChangedHandler(string serverName, string signalName, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate);
		void RegisterSignalInformationChangedHandler(string serverName, string[] signalNames, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate);
		void UnregisterSignalInformationChangedHandler(string serverName, string signalName, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate);
		void UnregisterSignalInformationChangedHandler(string serverName, string[] signalNames, OnSignalInformationChangedDelegate onSignalInformationChangedDelegate);
	}
}
