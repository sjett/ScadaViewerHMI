// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public enum AlarmVisibility
	{
		ActiveOrNotAcknowledged,
		NotAcknowledged = 2,
		Active = 4,
		All = 8
	}
}
