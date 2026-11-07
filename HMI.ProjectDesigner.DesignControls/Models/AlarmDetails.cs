// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using HMI.ProjectDesigner.DesignControls.Alarms;
using HMI.Common.Presentation.Events;
namespace HMI.ProjectDesigner.DesignControls.Models
{
	public class AlarmDetails : PropertyChangedNotifier
	{
		private const string carriageReturn = "\r";
		private const string newLine = "\n";
		private const string ellipsis = " (...)";
		private int occurrenceCount;
		private System.TimeSpan duration;
		private string acknowledgeComment;
		private string strippedAcknowledgeComment;
		private string acknowledgeUser;
		private string alarmGroupSymbolicTextName;
		private System.Guid alarmID;
		private System.Guid alarmLogID;
		private AlarmState alarmType;
		private string alarmTypeSymbolicTextName;
		private string aliasName;
		private readonly System.Globalization.CultureInfo cultureInfo;
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
		private bool showMultilineTextValues;
		private string signalName;
		private string text;
		private string type;
		private string statusText;
		private string helpCause;
		private string strippedHelpCause;
		private string helpEffect;
		private string strippedHelpEffect;
		private string helpRepair;
		private string strippedHelpRepair;
		private string generalComment;
		private string strippedGeneralComment;
		private string occurrenceComment;
		private string strippedOccurrenceComment;
		private string navigationSource;
		private string navigationTarget;
		private string extendedProperty1Text;
		private string extendedProperty2Text;
		private string extendedProperty3Text;
		private string extendedProperty4Text;
		private string extendedProperty5Text;
		private string extendedProperty6Text;
		private string extendedProperty7Text;
		private string extendedProperty8Text;
		private string extendedProperty9Text;
		private string extendedProperty10Text;
		private string extendedProperty11Text;
		private string extendedProperty12Text;
		private string extendedProperty13Text;
		private string extendedProperty14Text;
		private string extendedProperty15Text;
		private string extendedProperty16Text;
		private string extendedProperty17Text;
		private string extendedProperty18Text;
		private string extendedProperty19Text;
		private string extendedProperty20Text;
		private string extendedProperty21Text;
		private string extendedProperty22Text;
		private string extendedProperty23Text;
		private string extendedProperty24Text;
		private string extendedProperty25Text;
		private string extendedProperty26Text;
		private string extendedProperty27Text;
		private string extendedProperty28Text;
		private string extendedProperty29Text;
		private string extendedProperty30Text;
		private string extendedProperty31Text;
		private string extendedProperty32Text;
		private bool hasBadQuality;
		public bool HasBadQuality
		{
			get
			{
				return this.hasBadQuality;
			}
			set
			{
				this.hasBadQuality = value;
				base.RaisePropertyChanged<bool>(() => this.HasBadQuality);
			}
		}
		public bool ShowMultilineTextValues
		{
			get
			{
				return this.showMultilineTextValues;
			}
			set
			{
				this.showMultilineTextValues = value;
				base.RaisePropertyChanged<bool>(() => this.ShowMultilineTextValues);
			}
		}
		public string AcknowledgeComment
		{
			get
			{
				return this.acknowledgeComment;
			}
			set
			{
				this.acknowledgeComment = value;
				base.RaisePropertyChanged<string>(() => this.AcknowledgeComment);
				base.RaisePropertyChanged<bool>(() => this.IsAcknowledgedWithComment);
				if (this.acknowledgeComment != null)
				{
					this.StrippedAcknowledgeComment = this.StripMultilineText(this.AcknowledgeComment);
				}
			}
		}
		public string StrippedAcknowledgeComment
		{
			get
			{
				return this.strippedAcknowledgeComment;
			}
			set
			{
				this.strippedAcknowledgeComment = value;
				base.RaisePropertyChanged<string>(() => this.StrippedAcknowledgeComment);
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
				base.RaisePropertyChanged<string>(() => this.AcknowledgeUser);
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
				base.RaisePropertyChanged<string>(() => this.AlarmGroupSymbolicTextName);
			}
		}
		public System.Guid AlarmID
		{
			get
			{
				return this.alarmID;
			}
			set
			{
				this.alarmID = value;
				base.RaisePropertyChanged<System.Guid>(() => this.AlarmID);
			}
		}
		public System.Guid AlarmLogID
		{
			get
			{
				return this.alarmLogID;
			}
			set
			{
				this.alarmLogID = value;
				base.RaisePropertyChanged<System.Guid>(() => this.AlarmLogID);
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
				base.RaisePropertyChanged<AlarmState>(() => this.AlarmType);
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
				base.RaisePropertyChanged<string>(() => this.AlarmTypeSymbolicTextName);
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
				base.RaisePropertyChanged<string>(() => this.AliasName);
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
				base.RaisePropertyChanged<System.DateTime>(() => this.DateAcknowledged);
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
				base.RaisePropertyChanged<string>(() => this.DateAcknowledgedString);
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
				base.RaisePropertyChanged<System.DateTime>(() => this.DateOff);
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
				base.RaisePropertyChanged<string>(() => this.DateOffString);
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
				base.RaisePropertyChanged<System.DateTime>(() => this.DateOn);
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
				base.RaisePropertyChanged<string>(() => this.DateOnString);
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
				base.RaisePropertyChanged<System.DateTime>(() => this.SystemTime);
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
				base.RaisePropertyChanged<string>(() => this.SystemTimeString);
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
				base.RaisePropertyChanged<string>(() => this.Group);
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
				base.RaisePropertyChanged<string>(() => this.HttpLink);
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
				base.RaisePropertyChanged<string>(() => this.Name);
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
				base.RaisePropertyChanged<int>(() => this.Priority);
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
				base.RaisePropertyChanged<string>(() => this.ServerID);
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
		public int OccurrenceCount
		{
			get
			{
				return this.occurrenceCount;
			}
			set
			{
				this.occurrenceCount = value;
				base.RaisePropertyChanged<int>(() => this.OccurrenceCount);
			}
		}
		public System.TimeSpan Duration
		{
			get
			{
				return this.duration;
			}
			set
			{
				this.duration = value;
				base.RaisePropertyChanged<System.TimeSpan>(() => this.Duration);
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
		public string HelpCause
		{
			get
			{
				return this.helpCause;
			}
			set
			{
				this.helpCause = value;
				base.RaisePropertyChanged<string>(() => this.HelpCause);
				if (this.helpCause != null)
				{
					this.StrippedHelpCause = this.StripMultilineText(this.HelpCause);
				}
			}
		}
		public string StrippedHelpCause
		{
			get
			{
				return this.strippedHelpCause;
			}
			set
			{
				this.strippedHelpCause = value;
				base.RaisePropertyChanged<string>(() => this.StrippedHelpCause);
			}
		}
		public string HelpEffect
		{
			get
			{
				return this.helpEffect;
			}
			set
			{
				this.helpEffect = value;
				base.RaisePropertyChanged<string>(() => this.HelpEffect);
				if (this.helpEffect != null)
				{
					this.StrippedHelpEffect = this.StripMultilineText(this.HelpEffect);
				}
			}
		}
		public string StrippedHelpEffect
		{
			get
			{
				return this.strippedHelpEffect;
			}
			set
			{
				this.strippedHelpEffect = value;
				base.RaisePropertyChanged<string>(() => this.StrippedHelpEffect);
			}
		}
		public string HelpRepair
		{
			get
			{
				return this.helpRepair;
			}
			set
			{
				this.helpRepair = value;
				base.RaisePropertyChanged<string>(() => this.HelpRepair);
				if (this.helpRepair != null)
				{
					this.StrippedHelpRepair = this.StripMultilineText(this.HelpRepair);
				}
			}
		}
		public string StrippedHelpRepair
		{
			get
			{
				return this.strippedHelpRepair;
			}
			set
			{
				this.strippedHelpRepair = value;
				base.RaisePropertyChanged<string>(() => this.StrippedHelpRepair);
			}
		}
		public string GeneralComment
		{
			get
			{
				return this.generalComment;
			}
			set
			{
				this.generalComment = value;
				base.RaisePropertyChanged<string>(() => this.GeneralComment);
				if (this.generalComment != null)
				{
					this.StrippedGeneralComment = this.StripMultilineText(this.GeneralComment);
				}
			}
		}
		public string StrippedGeneralComment
		{
			get
			{
				return this.strippedGeneralComment;
			}
			set
			{
				this.strippedGeneralComment = value;
				base.RaisePropertyChanged<string>(() => this.StrippedGeneralComment);
			}
		}
		public string OccurrenceComment
		{
			get
			{
				return this.occurrenceComment;
			}
			set
			{
				this.occurrenceComment = value;
				base.RaisePropertyChanged<string>(() => this.OccurrenceComment);
				if (this.occurrenceComment != null)
				{
					this.StrippedOccurrenceComment = this.StripMultilineText(this.OccurrenceComment);
				}
			}
		}
		public string StrippedOccurrenceComment
		{
			get
			{
				return this.strippedOccurrenceComment;
			}
			set
			{
				this.strippedOccurrenceComment = value;
				base.RaisePropertyChanged<string>(() => this.StrippedOccurrenceComment);
			}
		}
		public string NavigationSource
		{
			get
			{
				return this.navigationSource;
			}
			set
			{
				this.navigationSource = value;
				base.RaisePropertyChanged<string>(() => this.NavigationSource);
			}
		}
		public string NavigationTarget
		{
			get
			{
				return this.navigationTarget;
			}
			set
			{
				this.navigationTarget = value;
				base.RaisePropertyChanged<string>(() => this.NavigationTarget);
			}
		}
		public string ExtendedProperty1Text
		{
			get
			{
				return this.extendedProperty1Text;
			}
			set
			{
				this.extendedProperty1Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty1Text);
			}
		}
		public string ExtendedProperty2Text
		{
			get
			{
				return this.extendedProperty2Text;
			}
			set
			{
				this.extendedProperty2Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty2Text);
			}
		}
		public string ExtendedProperty3Text
		{
			get
			{
				return this.extendedProperty3Text;
			}
			set
			{
				this.extendedProperty3Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty3Text);
			}
		}
		public string ExtendedProperty4Text
		{
			get
			{
				return this.extendedProperty4Text;
			}
			set
			{
				this.extendedProperty4Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty4Text);
			}
		}
		public string ExtendedProperty5Text
		{
			get
			{
				return this.extendedProperty5Text;
			}
			set
			{
				this.extendedProperty5Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty5Text);
			}
		}
		public string ExtendedProperty6Text
		{
			get
			{
				return this.extendedProperty6Text;
			}
			set
			{
				this.extendedProperty6Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty6Text);
			}
		}
		public string ExtendedProperty7Text
		{
			get
			{
				return this.extendedProperty7Text;
			}
			set
			{
				this.extendedProperty7Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty7Text);
			}
		}
		public string ExtendedProperty8Text
		{
			get
			{
				return this.extendedProperty8Text;
			}
			set
			{
				this.extendedProperty8Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty8Text);
			}
		}
		public string ExtendedProperty9Text
		{
			get
			{
				return this.extendedProperty9Text;
			}
			set
			{
				this.extendedProperty9Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty9Text);
			}
		}
		public string ExtendedProperty10Text
		{
			get
			{
				return this.extendedProperty10Text;
			}
			set
			{
				this.extendedProperty10Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty10Text);
			}
		}
		public string ExtendedProperty11Text
		{
			get
			{
				return this.extendedProperty11Text;
			}
			set
			{
				this.extendedProperty11Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty11Text);
			}
		}
		public string ExtendedProperty12Text
		{
			get
			{
				return this.extendedProperty12Text;
			}
			set
			{
				this.extendedProperty12Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty12Text);
			}
		}
		public string ExtendedProperty13Text
		{
			get
			{
				return this.extendedProperty13Text;
			}
			set
			{
				this.extendedProperty13Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty13Text);
			}
		}
		public string ExtendedProperty14Text
		{
			get
			{
				return this.extendedProperty14Text;
			}
			set
			{
				this.extendedProperty14Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty14Text);
			}
		}
		public string ExtendedProperty15Text
		{
			get
			{
				return this.extendedProperty15Text;
			}
			set
			{
				this.extendedProperty15Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty15Text);
			}
		}
		public string ExtendedProperty16Text
		{
			get
			{
				return this.extendedProperty16Text;
			}
			set
			{
				this.extendedProperty16Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty16Text);
			}
		}
		public string ExtendedProperty17Text
		{
			get
			{
				return this.extendedProperty17Text;
			}
			set
			{
				this.extendedProperty17Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty17Text);
			}
		}
		public string ExtendedProperty18Text
		{
			get
			{
				return this.extendedProperty18Text;
			}
			set
			{
				this.extendedProperty18Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty18Text);
			}
		}
		public string ExtendedProperty19Text
		{
			get
			{
				return this.extendedProperty19Text;
			}
			set
			{
				this.extendedProperty19Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty19Text);
			}
		}
		public string ExtendedProperty20Text
		{
			get
			{
				return this.extendedProperty20Text;
			}
			set
			{
				this.extendedProperty20Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty20Text);
			}
		}
		public string ExtendedProperty21Text
		{
			get
			{
				return this.extendedProperty21Text;
			}
			set
			{
				this.extendedProperty21Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty21Text);
			}
		}
		public string ExtendedProperty22Text
		{
			get
			{
				return this.extendedProperty22Text;
			}
			set
			{
				this.extendedProperty22Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty22Text);
			}
		}
		public string ExtendedProperty23Text
		{
			get
			{
				return this.extendedProperty23Text;
			}
			set
			{
				this.extendedProperty23Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty23Text);
			}
		}
		public string ExtendedProperty24Text
		{
			get
			{
				return this.extendedProperty24Text;
			}
			set
			{
				this.extendedProperty24Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty24Text);
			}
		}
		public string ExtendedProperty25Text
		{
			get
			{
				return this.extendedProperty25Text;
			}
			set
			{
				this.extendedProperty25Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty25Text);
			}
		}
		public string ExtendedProperty26Text
		{
			get
			{
				return this.extendedProperty26Text;
			}
			set
			{
				this.extendedProperty26Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty26Text);
			}
		}
		public string ExtendedProperty27Text
		{
			get
			{
				return this.extendedProperty27Text;
			}
			set
			{
				this.extendedProperty27Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty27Text);
			}
		}
		public string ExtendedProperty28Text
		{
			get
			{
				return this.extendedProperty28Text;
			}
			set
			{
				this.extendedProperty28Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty28Text);
			}
		}
		public string ExtendedProperty29Text
		{
			get
			{
				return this.extendedProperty29Text;
			}
			set
			{
				this.extendedProperty29Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty29Text);
			}
		}
		public string ExtendedProperty30Text
		{
			get
			{
				return this.extendedProperty30Text;
			}
			set
			{
				this.extendedProperty30Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty30Text);
			}
		}
		public string ExtendedProperty31Text
		{
			get
			{
				return this.extendedProperty31Text;
			}
			set
			{
				this.extendedProperty31Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty31Text);
			}
		}
		public string ExtendedProperty32Text
		{
			get
			{
				return this.extendedProperty32Text;
			}
			set
			{
				this.extendedProperty32Text = value;
				base.RaisePropertyChanged<string>(() => this.ExtendedProperty32Text);
			}
		}
		public AlarmDetails(System.Globalization.CultureInfo cultureInfo, bool showDateTimeMilliseconds, bool showMultilineTextValues)
		{
			this.cultureInfo = cultureInfo;
			this.showDateTimeMilliseconds = showDateTimeMilliseconds;
			this.showMultilineTextValues = showMultilineTextValues;
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
		public void UpdateDuration()
		{
			if (this.AlarmType != AlarmState.Active)
			{
				if (this.AlarmType != AlarmState.Acknowledged)
				{
					this.Duration = this.DateOff.Subtract(this.DateOn);
					return;
				}
			}
			this.Duration = System.DateTime.Now.Subtract(this.DateOn);
		}
		private void SetAlarmType()
		{
			if (this.dateOff != System.DateTime.MinValue && this.dateAcknowledged != System.DateTime.MinValue)
			{
				this.AlarmType = AlarmState.AcknowledgedAndGone;
			}
			else
			{
				if (this.dateOff != System.DateTime.MinValue)
				{
					this.AlarmType = AlarmState.Inactive;
				}
				else
				{
					if (this.dateAcknowledged != System.DateTime.MinValue)
					{
						this.AlarmType = AlarmState.Acknowledged;
					}
					else
					{
						this.AlarmType = AlarmState.Active;
					}
				}
			}
			this.UpdateDuration();
		}
		private string StripMultilineText(string text)
		{
			int carriageReturnIndex = text.IndexOf("\r");
			int lineFeedIndex = text.IndexOf("\n");
			if (carriageReturnIndex == -1 && lineFeedIndex == -1)
			{
				return text;
			}
			if (carriageReturnIndex == -1 && lineFeedIndex != -1)
			{
				return text.Substring(0, lineFeedIndex) + " (...)";
			}
			if (carriageReturnIndex != -1 && lineFeedIndex == -1)
			{
				return text.Substring(0, carriageReturnIndex) + " (...)";
			}
			return text.Substring(0, System.Math.Min(carriageReturnIndex, lineFeedIndex)) + " (...)";
		}
	}
}
