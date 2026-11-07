// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public class AlarmValue : System.IComparable<AlarmValue>
	{
		public string Name
		{
			get;
			private set;
		}
		public int Value
		{
			get;
			private set;
		}
		public AlarmValue(string name, int value)
		{
			this.Name = name;
			this.Value = value;
		}
		public int CompareTo(AlarmValue other)
		{
			if (this.Value > other.Value)
			{
				return 1;
			}
			if (this.Value < other.Value)
			{
				return -1;
			}
			return 0;
		}
	}
}
