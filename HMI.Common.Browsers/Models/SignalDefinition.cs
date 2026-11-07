// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Browsers.Models
{
	/// <summary>
	/// 描述信号的名称、量程、报警及连接配置。
	/// </summary>
	public class SignalDefinition : Model
	{
		public string Name
		{
			get;
			set;
		}
		public string AliasName
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public bool Active
		{
			get;
			set;
		}
		public string Unit
		{
			get;
			set;
		}
		public bool OPCEnabled
		{
			get;
			set;
		}
		public double FactorX1
		{
			get;
			set;
		}
		public double FactorX2
		{
			get;
			set;
		}
		public double FactorY1
		{
			get;
			set;
		}
		public double FactorY2
		{
			get;
			set;
		}
		public bool LogUserActivity
		{
			get;
			set;
		}
		public double Hysterese
		{
			get;
			set;
		}
		public double HystereseLog
		{
			get;
			set;
		}
		public double HystereseAlarm
		{
			get;
			set;
		}
		public double Maximum
		{
			get;
			set;
		}
		public double Minimum
		{
			get;
			set;
		}
		public bool VChannel
		{
			get;
			set;
		}
		public Server Server
		{
			get;
			set;
		}
		public Connector Connector
		{
			get;
			set;
		}
		public SignalGroup Group
		{
			get;
			set;
		}
		public WriteGroup WriteGroup
		{
			get;
			set;
		}
		public SignalDefinition()
		{
		}
	}
}
