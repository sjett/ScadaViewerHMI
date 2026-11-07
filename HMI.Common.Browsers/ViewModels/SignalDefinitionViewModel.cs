// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Browsers.Models;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	internal class SignalDefinitionViewModel : ViewModel, IBrowserItemViewModel, ISignalDefinitionViewModel
	{
		private string name;
		private string aliasName;
		private string description;
		private bool isActive;
		private string unit;
		private bool isOpcEnabled;
		private double factorX1;
		private double factorX2;
		private double factorY1;
		private double factorY2;
		private bool logUserActivity;
		private double hysterese;
		private double hystereseLog;
		private double hystereseAlarm;
		private double maximum;
		private double minimum;
		private bool isVChannel;
		private string server;
		private string serverDescription;
		private string connector;
		private string connectorDescription;
		private string group;
		private string groupDescription;
		private string writeGroup;
		//private string _logTag;
		//private bool _isLogActive;
		//private string _logDescription;
		//private Guid? _logId;

		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (name != value)
				{
					base.RaisePropertyChanging<string>(() => Name);
                    name = value;
					base.RaisePropertyChanged<string>(() => Name);
				}
			}
		}
		public string AliasName
		{
			get
			{
				return aliasName;
			}
			set
			{
				if (aliasName != value)
				{
					base.RaisePropertyChanging<string>(() => AliasName);
                    aliasName = value;
					base.RaisePropertyChanged<string>(() => AliasName);
				}
			}
		}
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				if (description != value)
				{
					base.RaisePropertyChanging<string>(() => Description);
                    description = value;
					base.RaisePropertyChanged<string>(() => Description);
				}
			}
		}
		public bool IsActive
		{
			get
			{
				return isActive;
			}
			set
			{
				if (isActive != value)
				{
					base.RaisePropertyChanging<bool>(() => IsActive);
                    isActive = value;
					base.RaisePropertyChanged<bool>(() => IsActive);
				}
			}
		}
		public string Unit
		{
			get
			{
				return unit;
			}
			set
			{
				if (unit != value)
				{
					base.RaisePropertyChanging<string>(() => Unit);
                    unit = value;
					base.RaisePropertyChanged<string>(() => Unit);
				}
			}
		}
		public bool IsOpcEnabled
		{
			get
			{
				return isOpcEnabled;
			}
			set
			{
				if (isOpcEnabled != value)
				{
					base.RaisePropertyChanging<bool>(() => IsOpcEnabled);
                    isOpcEnabled = value;
					base.RaisePropertyChanged<bool>(() => IsOpcEnabled);
				}
			}
		}
		public double FactorX1
		{
			get
			{
				return factorX1;
			}
			set
			{
				if (factorX1 != value)
				{
					base.RaisePropertyChanging<double>(() => FactorX1);
                    factorX1 = value;
					base.RaisePropertyChanged<double>(() => FactorX1);
				}
			}
		}
		public double FactorX2
		{
			get
			{
				return factorX2;
			}
			set
			{
				if (factorX2 != value)
				{
					base.RaisePropertyChanging<double>(() => FactorX2);
                    factorX2 = value;
					base.RaisePropertyChanged<double>(() => FactorX2);
				}
			}
		}
		public double FactorY1
		{
			get
			{
				return factorY1;
			}
			set
			{
				if (factorY1 != value)
				{
					base.RaisePropertyChanging<double>(() => FactorY1);
                    factorY1 = value;
					base.RaisePropertyChanged<double>(() => FactorY1);
				}
			}
		}
		public double FactorY2
		{
			get
			{
				return factorY2;
			}
			set
			{
				if (factorY2 != value)
				{
					base.RaisePropertyChanging<double>(() => FactorY2);
                    factorY2 = value;
					base.RaisePropertyChanged<double>(() => FactorY2);
				}
			}
		}
		public bool LogUserActivity
		{
			get
			{
				return logUserActivity;
			}
			set
			{
				if (logUserActivity != value)
				{
					base.RaisePropertyChanging<bool>(() => LogUserActivity);
                    logUserActivity = value;
					base.RaisePropertyChanged<bool>(() => LogUserActivity);
				}
			}
		}
		public double Hysterese
		{
			get
			{
				return hysterese;
			}
			set
			{
				if (hysterese != value)
				{
					base.RaisePropertyChanging<double>(() => Hysterese);
                    hysterese = value;
					base.RaisePropertyChanged<double>(() => Hysterese);
				}
			}
		}
		public double HystereseLog
		{
			get
			{
				return hystereseLog;
			}
			set
			{
				if (hystereseLog != value)
				{
					base.RaisePropertyChanging<double>(() => HystereseLog);
                    hystereseLog = value;
					base.RaisePropertyChanged<double>(() => HystereseLog);
				}
			}
		}
		public double HystereseAlarm
		{
			get
			{
				return hystereseAlarm;
			}
			set
			{
				if (hystereseAlarm != value)
				{
					base.RaisePropertyChanging<double>(() => HystereseAlarm);
                    hystereseAlarm = value;
					base.RaisePropertyChanged<double>(() => HystereseAlarm);
				}
			}
		}
		public double Maximum
		{
			get
			{
				return maximum;
			}
			set
			{
				if (maximum != value)
				{
					base.RaisePropertyChanging<double>(() => Maximum);
                    maximum = value;
					base.RaisePropertyChanged<double>(() => Maximum);
				}
			}
		}
		public double Minimum
		{
			get
			{
				return minimum;
			}
			set
			{
				if (minimum != value)
				{
					base.RaisePropertyChanging<double>(() => Minimum);
                    minimum = value;
					base.RaisePropertyChanged<double>(() => Minimum);
				}
			}
		}
		public bool IsVChannel
		{
			get
			{
				return isVChannel;
			}
			set
			{
				if (isVChannel != value)
				{
					base.RaisePropertyChanging<bool>(() => IsVChannel);
                    isVChannel = value;
					base.RaisePropertyChanged<bool>(() => IsVChannel);
				}
			}
		}
		public string Server
		{
			get
			{
				return server;
			}
			set
			{
				if (server != value)
				{
					base.RaisePropertyChanging<string>(() => Server);
                    server = value;
					base.RaisePropertyChanged<string>(() => Server);
				}
			}
		}
		public string ServerDescription
		{
			get
			{
				return serverDescription;
			}
			set
			{
				if (serverDescription == value)
				{
					return;
				}
				base.RaisePropertyChanging<string>(() => ServerDescription);
                serverDescription = value;
				base.RaisePropertyChanged<string>(() => ServerDescription);
			}
		}
		public string Connector
		{
			get
			{
				return connector;
			}
			set
			{
				if (connector != value)
				{
					base.RaisePropertyChanging<string>(() => Connector);
                    connector = value;
					base.RaisePropertyChanged<string>(() => Connector);
				}
			}
		}
		public string ConnectorDescription
		{
			get
			{
				return connectorDescription;
			}
			set
			{
				if (connectorDescription != value)
				{
					base.RaisePropertyChanging<string>(() => ConnectorDescription);
                    connectorDescription = value;
					base.RaisePropertyChanged<string>(() => ConnectorDescription);
				}
			}
		}
		public string Group
		{
			get
			{
				return group;
			}
			set
			{
				if (group != value)
				{
					base.RaisePropertyChanging<string>(() => Group);
                    group = value;
					base.RaisePropertyChanged<string>(() => Group);
				}
			}
		}
		public string GroupDescription
		{
			get
			{
				return groupDescription;
			}
			set
			{
				if (groupDescription == value)
				{
					return;
				}
				base.RaisePropertyChanging<string>(() => GroupDescription);
                groupDescription = value;
				base.RaisePropertyChanged<string>(() => GroupDescription);
			}
		}
		public string WriteGroup
		{
			get
			{
				return writeGroup;
			}
			set
			{
				if (writeGroup != value)
				{
					base.RaisePropertyChanging<string>(() => WriteGroup);
                    writeGroup = value;
					base.RaisePropertyChanged<string>(() => WriteGroup);
				}
			}
		}
		string IBrowserItemViewModel.ItemText
		{
			get
			{
				return AliasName;
			}
		}
		//public string LogTag
		//{
		//	get
		//	{
		//		return _logTag;
		//	}
		//	set
		//	{
		//		if (_logTag == value)
		//		{
		//			return;
		//		}
		//		base.RaisePropertyChanging<string>(() => LogTag);
  //              _logTag = value;
		//		base.RaisePropertyChanged<string>(() => LogTag);
		//	}
		//}
		//public bool IsLogActive
		//{
		//	get
		//	{
		//		return _isLogActive;
		//	}
		//	set
		//	{
		//		if (_isLogActive == value)
		//		{
		//			return;
		//		}
		//		base.RaisePropertyChanging<bool>(() => IsLogActive);
  //              _isLogActive = value;
		//		base.RaisePropertyChanged<bool>(() => IsLogActive);
		//	}
		//}
		//public string LogDescription
		//{
		//	get
		//	{
		//		return _logDescription;
		//	}
		//	set
		//	{
		//		if (_logDescription == value)
		//		{
		//			return;
		//		}
		//		base.RaisePropertyChanging<string>(() => LogDescription);
  //              _logDescription = value;
		//		base.RaisePropertyChanged<string>(() => LogDescription);
		//	}
		//}
		//public Guid? LogId
		//{
		//	get
		//	{
		//		return _logId;
		//	}
		//	set
		//	{
		//		if (_logId == value)
		//		{
		//			return;
		//		}
		//		base.RaisePropertyChanging<Guid?>(() => LogId);
  //              _logId = value;
		//		base.RaisePropertyChanged<Guid?>(() => LogId);
		//	}
		//}
		public SignalDefinitionViewModel(
			//SignalLog log, 
			SignalDefinition definition)
		{
			if (definition == null)
			{
				return;
			}
            Name = definition.Name;
            AliasName = definition.AliasName;
            Description = definition.Description;
            IsActive = definition.Active;
            Unit = definition.Unit;
            IsOpcEnabled = definition.OPCEnabled;
            FactorX1 = definition.FactorX1;
            FactorX2 = definition.FactorX2;
            FactorY1 = definition.FactorY1;
            FactorY2 = definition.FactorY2;
            LogUserActivity = definition.LogUserActivity;
            Hysterese = definition.Hysterese;
            HystereseLog = definition.HystereseLog;
            HystereseAlarm = definition.HystereseAlarm;
            Maximum = definition.Maximum;
            Minimum = definition.Minimum;
            IsVChannel = definition.VChannel;
			if (definition.Server != null)
			{
                Server = definition.Server.Name;
                ServerDescription = definition.Server.Description;
			}
			if (definition.Connector != null)
			{
                Connector = definition.Connector.Name;
                ConnectorDescription = definition.Connector.Description;
			}
			if (definition.Group != null)
			{
                Group = definition.Group.Name;
                GroupDescription = definition.Group.Description;
			}
			if (definition.WriteGroup != null)
			{
                WriteGroup = definition.WriteGroup.Name;
			}
			//if (log != null)
			//{
   //             LogTag = log.LogTag;
   //             IsLogActive = log.Active;
   //             LogDescription = log.Description;
   //             LogId = new Guid?(log.Id);
			//}
		}
	}
}
