// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Core
{
	public interface IMachineSettingsProvider : IDisposable
	{
		Action<MachineSettings> Callback
		{
			get;
			set;
		}
		void Refresh();
	}
}
