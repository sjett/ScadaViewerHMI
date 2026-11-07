// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Browsers.Models;
namespace HMI.Common.Browsers.Services
{
	public interface IAlarmService
	{
		IObservable<TranslatedEntity> GetAlarmGroups();
		IObservable<TranslatedEntity> GetAlarmTypes();
		IObservable<ExtendedAlarmProperty> GetExtendedAlarmProperties();
	}
}
