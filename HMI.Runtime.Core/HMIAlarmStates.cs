// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Core
{
	public enum HMIAlarmStates
	{
		OFF,
		NotProcessedButVisible = 0,
		ON,
		ProcessedAndVisible = 1,
		NotProcessedAndNotVisible,
		ProcessedButNotVisible
	}
}
