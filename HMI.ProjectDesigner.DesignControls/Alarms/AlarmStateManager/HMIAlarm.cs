// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Presentation.Events;
namespace HMI.ProjectDesigner.DesignControls.Alarms.AlarmStateManager
{
	public class HMIAlarm : PropertyChangedNotifier
	{
		private string conditionName;
		private readonly HMIAlarmGroup group;
		private readonly System.Guid id;
		private string opcItemName;
		private System.DateTime reactivationDate;
		private string reactivationDateText;
		private string signalDescription;
		private string signalName;
		private int status;
		private string statusText;
		private string tag;
		private string text;
		private string type;
		public string ConditionName
		{
			get
			{
				return this.conditionName;
			}
			set
			{
				this.conditionName = value;
				base.RaisePropertyChanged<string>(() => this.ConditionName);
			}
		}
		public HMIAlarmGroup Group
		{
			get
			{
				return this.group;
			}
		}
		public System.Guid ID
		{
			get
			{
				return this.id;
			}
		}
		public string OPCItemName
		{
			get
			{
				return this.opcItemName;
			}
			set
			{
				this.opcItemName = value;
				base.RaisePropertyChanged<string>(() => this.OPCItemName);
			}
		}
		public System.DateTime ReactivationDate
		{
			get
			{
				return this.reactivationDate;
			}
			set
			{
				this.reactivationDate = value;
				base.RaisePropertyChanged<System.DateTime>(() => this.ReactivationDate);
			}
		}
		public string ReactivationDateText
		{
			get
			{
				return this.reactivationDateText;
			}
			set
			{
				this.reactivationDateText = value;
				base.RaisePropertyChanged<string>(() => this.ReactivationDateText);
			}
		}
		public string SignalDescription
		{
			get
			{
				return this.signalDescription;
			}
			set
			{
				this.signalDescription = value;
				base.RaisePropertyChanged<string>(() => this.SignalDescription);
			}
		}
		public string SignalName
		{
			get
			{
				return this.signalName;
			}
			set
			{
				this.signalName = value;
				base.RaisePropertyChanged<string>(() => this.SignalName);
			}
		}
		public int Status
		{
			get
			{
				return this.status;
			}
			set
			{
				this.status = value;
				base.RaisePropertyChanged<int>(() => this.Status);
			}
		}
		public string StatusText
		{
			get
			{
				return this.statusText;
			}
			set
			{
				this.statusText = value;
				base.RaisePropertyChanged<string>(() => this.StatusText);
			}
		}
		public string Tag
		{
			get
			{
				return this.tag;
			}
			set
			{
				this.tag = value;
				base.RaisePropertyChanged<string>(() => this.Tag);
			}
		}
		public string Text
		{
			get
			{
				return this.text;
			}
			set
			{
				this.text = value;
				base.RaisePropertyChanged<string>(() => this.Text);
			}
		}
		public string Type
		{
			get
			{
				return this.type;
			}
			set
			{
				this.type = value;
				base.RaisePropertyChanged<string>(() => this.Type);
			}
		}
		public HMIAlarm(HMIAlarmGroup group, System.Guid id)
		{
			this.group = group;
			this.id = id;
		}
	}
}
