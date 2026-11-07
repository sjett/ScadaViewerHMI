// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Runtime.Core.Helper.Extensions;
namespace HMI.Runtime.Core
{
	public class MachineSettingsProvider : IDisposable, IMachineSettingsProvider
	{
		private bool _isDisposed;
		
		public Action<MachineSettings> Callback
		{
			get;
			set;
		}
		public MachineSettingsProvider(Action<MachineSettings> callback = null)
		{
			this.Callback = callback;
			this.Refresh();
		}
		
		public void Refresh()
		{
			if (this.Callback != null && !DesignerPropertiesExt.IsInDesignTool)
			{
				ApplySettings(new MachineSettings
				{
					Id = Guid.NewGuid(),
					MachineName = Environment.MachineName,
					HasVirtualKeyboard = true,
					UpdateRate = 500,
					MachineSettingsApplied = true
				});

                return;
			}
		}
		private void ApplySettings(MachineSettings machineSettings)
		{
			if (this.Callback != null)
			{
				this.Callback.Invoke(machineSettings);
			}
		}
		protected virtual void DisposeManagedResources()
		{
		}
		public void Dispose()
		{
			if (this._isDisposed)
			{
				return;
			}
			this.DisposeManagedResources();
			this._isDisposed = true;
			GC.SuppressFinalize(this);
		}
	}
}
