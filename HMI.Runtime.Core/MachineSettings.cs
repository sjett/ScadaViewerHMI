// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
namespace HMI.Runtime.Core
{
	public class MachineSettings
	{
		public Guid Id
		{
			get;
			set;
		}
		public string MachineName
		{
			get;
			set;
		}
		public bool HasVirtualKeyboard
		{
			get;
			set;
		}
		public bool MachineSettingsApplied
		{
			get;
			set;
		}
		public int UpdateRate
		{
			get;
			set;
		}
		public MachineSettings()
		{
			this.HasVirtualKeyboard = false;
			this.UpdateRate = 500;
			this.Id = Guid.Empty;
			this.MachineSettingsApplied = false;
		}
		public bool IsEqual(MachineSettings other)
		{
			return this.Id == other.Id && this.MachineName == other.MachineName && this.HasVirtualKeyboard == other.HasVirtualKeyboard && this.UpdateRate == other.UpdateRate;
		}
	}
}
