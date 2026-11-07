// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Presentation.Events;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public class MachineAvailability : PropertyChangedNotifier
	{
		private long duration;
		private string state;
		public long Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = value;
				base.RaisePropertyChanged<long>(() => this.Duration);
			}
		}
		public double SignalValue
		{
			get;
			private set;
		}
		public string State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
				base.RaisePropertyChanged<string>(() => this.State);
			}
		}
		public MachineAvailability(double signalValue, long duration) : this(signalValue, duration, signalValue.ToString())
		{
		}
		public MachineAvailability(double signalValue, long duration, string state)
		{
			this.Duration = duration;
			this.SignalValue = signalValue;
			this.State = state;
		}
	}
}
