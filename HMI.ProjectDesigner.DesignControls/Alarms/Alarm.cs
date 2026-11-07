// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using HMI.Common.Presentation.Events;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public class Alarm : PropertyChangedNotifier
	{
		private string acknowledgeComment;
		private string acknowledgeUser;
		private string alarmGroupSymbolicTextName;
		private string alarmID;
		private string alarmLogID;
		private AlarmState alarmType;
		private string alarmTypeSymbolicTextName;
		private string aliasName;
		private System.Globalization.CultureInfo cultureInfo;
		private System.DateTime dateAcknowledged;
		private string dateAcknowledgedString;
		private System.DateTime dateOff;
		private string dateOffString;
		private System.DateTime dateOn;
		private string dateOnString;
		private System.DateTime systemTime;
		private string systemTimeString;
		private string group;
		private string httpLink;
		private string name;
		private int priority;
		private string serverID;
		private readonly bool showDateTimeMilliseconds;
		private string signalName;
		private string text;
		private string type;
		public string AcknowledgeComment
		{
			get
			{
				return this.acknowledgeComment;
			}
			set
			{
				this.acknowledgeComment = value;
				base.RaisePropertyChanged("AcknowledgeComment");
			}
		}
		public bool IsAcknowledgedWithComment
		{
			get
			{
				return !string.IsNullOrEmpty(this.acknowledgeComment);
			}
		}
		public string AcknowledgeUser
		{
			get
			{
				return this.acknowledgeUser;
			}
			set
			{
				this.acknowledgeUser = value;
				base.RaisePropertyChanged("AcknowledgeUser");
			}
		}
		public string AlarmGroupSymbolicTextName
		{
			get
			{
				return this.alarmGroupSymbolicTextName;
			}
			set
			{
				this.alarmGroupSymbolicTextName = value;
				base.RaisePropertyChanged("AlarmGroupSymbolicTextName");
			}
		}
		public string AlarmID
		{
			get
			{
				return this.alarmID;
			}
			set
			{
				this.alarmID = value;
				base.RaisePropertyChanged("AlarmID");
			}
		}
		public string AlarmLogID
		{
			get
			{
				return this.alarmLogID;
			}
			set
			{
				this.alarmLogID = value;
				base.RaisePropertyChanged("AlarmLogID");
			}
		}
		public AlarmState AlarmType
		{
			get
			{
				return this.alarmType;
			}
			private set
			{
				this.alarmType = value;
				base.RaisePropertyChanged("AlarmType");
			}
		}
		public string AlarmTypeSymbolicTextName
		{
			get
			{
				return this.alarmTypeSymbolicTextName;
			}
			set
			{
				this.alarmTypeSymbolicTextName = value;
				base.RaisePropertyChanged("AlarmTypeSymbolicTextName");
			}
		}
		public string AliasName
		{
			get
			{
				return this.aliasName;
			}
			set
			{
				this.aliasName = value;
				base.RaisePropertyChanged("AliasName");
			}
		}
		public System.DateTime DateAcknowledged
		{
			get
			{
				return this.dateAcknowledged;
			}
			set
			{
				this.dateAcknowledged = value;
				this.SetAlarmType();
				this.DateAcknowledgedString = this.FormatDateTime(this.dateAcknowledged);
				base.RaisePropertyChanged("DateAcknowledged");
			}
		}
		public string DateAcknowledgedString
		{
			get
			{
				return this.dateAcknowledgedString;
			}
			private set
			{
				this.dateAcknowledgedString = value;
				base.RaisePropertyChanged("DateAcknowledgedString");
			}
		}
		public System.DateTime DateOff
		{
			get
			{
				return this.dateOff;
			}
			set
			{
				this.dateOff = value;
				this.SetAlarmType();
				this.DateOffString = this.FormatDateTime(this.dateOff);
				base.RaisePropertyChanged("DateOff");
			}
		}
		public string DateOffString
		{
			get
			{
				return this.dateOffString;
			}
			private set
			{
				this.dateOffString = value;
				base.RaisePropertyChanged("DateOffString");
			}
		}
		public System.DateTime DateOn
		{
			get
			{
				return this.dateOn;
			}
			set
			{
				this.dateOn = value;
				this.DateOnString = this.FormatDateTime(this.dateOn);
				base.RaisePropertyChanged("DateOn");
			}
		}
		public string DateOnString
		{
			get
			{
				return this.dateOnString;
			}
			private set
			{
				this.dateOnString = value;
				base.RaisePropertyChanged("DateOnString");
			}
		}
		public System.DateTime SystemTime
		{
			get
			{
				return this.systemTime;
			}
			set
			{
				this.systemTime = value;
				this.SystemTimeString = this.FormatDateTime(this.systemTime);
				base.RaisePropertyChanged("SystemTime");
			}
		}
		public string SystemTimeString
		{
			get
			{
				return this.systemTimeString;
			}
			private set
			{
				this.systemTimeString = value;
				base.RaisePropertyChanged("SystemTimeString");
			}
		}
		public string Group
		{
			get
			{
				return this.group;
			}
			set
			{
				this.group = value;
				base.RaisePropertyChanged("Group");
			}
		}
		public string HttpLink
		{
			get
			{
				return this.httpLink;
			}
			set
			{
				this.httpLink = value;
				base.RaisePropertyChanged("HttpLink");
			}
		}
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				base.RaisePropertyChanged("Name");
			}
		}
		public int Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				this.priority = value;
				base.RaisePropertyChanged("Priority");
			}
		}
		public string ServerID
		{
			get
			{
				return this.serverID;
			}
			set
			{
				this.serverID = value;
				base.RaisePropertyChanged("ServerID");
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
				base.RaisePropertyChanged("SignalName");
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
				base.RaisePropertyChanged("Text");
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
				base.RaisePropertyChanged("Type");
			}
		}
		public Alarm(System.Globalization.CultureInfo cultureInfo, bool showDateTimeMilliseconds)
		{
			this.cultureInfo = cultureInfo;
			this.showDateTimeMilliseconds = showDateTimeMilliseconds;
		}
		public void UpdateFrom(Alarm alarm)
		{
			if (alarm == null)
			{
				throw new System.ArgumentNullException("alarm");
			}
			this.AlarmID = alarm.AlarmID;
			this.SystemTime = alarm.SystemTime;
			this.DateOff = alarm.DateOff;
			this.DateAcknowledged = alarm.DateAcknowledged;
			this.Name = alarm.Name;
			this.SignalName = alarm.SignalName;
			this.AliasName = alarm.AliasName;
			this.Text = alarm.Text;
			this.Group = alarm.Group;
			this.Type = alarm.Type;
			this.ServerID = alarm.ServerID;
			this.Priority = alarm.Priority;
			this.AcknowledgeComment = alarm.AcknowledgeComment;
			this.AcknowledgeUser = alarm.AcknowledgeUser;
			this.HttpLink = alarm.HttpLink;
			this.AlarmTypeSymbolicTextName = alarm.AlarmTypeSymbolicTextName;
			this.AlarmGroupSymbolicTextName = alarm.AlarmGroupSymbolicTextName;
			this.cultureInfo = alarm.cultureInfo;
		}
		public override bool Equals(object obj)
		{
			Alarm alarm = obj as Alarm;
			return alarm != null && alarm.alarmLogID == this.alarmLogID && alarm.dateOn == this.dateOn;
		}
		public override int GetHashCode()
		{
			return this.alarmLogID.GetHashCode() ^ this.dateOn.GetHashCode();
		}
		private string FormatDateTime(System.DateTime dateTime)
		{
			if (dateTime == System.DateTime.MinValue)
			{
				return string.Empty;
			}
			if (this.showDateTimeMilliseconds)
			{
				string text = this.cultureInfo.DateTimeFormat.LongTimePattern;
				text = Regex.Replace(text, "(:ss|:s)", "$1.fff");
				return string.Format("{0} {1}", dateTime.ToString("d", this.cultureInfo), dateTime.ToString(text, this.cultureInfo));
			}
			return dateTime.ToString(this.cultureInfo);
		}
		private void SetAlarmType()
		{
			if (this.dateOff != System.DateTime.MinValue)
			{
				this.AlarmType = AlarmState.Inactive;
				return;
			}
			if (this.dateAcknowledged != System.DateTime.MinValue)
			{
				this.AlarmType = AlarmState.Acknowledged;
				return;
			}
			this.AlarmType = AlarmState.Active;
		}
	}
}
