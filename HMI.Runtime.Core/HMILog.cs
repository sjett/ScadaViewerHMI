// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
namespace HMI.Runtime.Core
{
	public class HMILog : HMIEntity
	{
		public new string Name
		{
			get
			{
				return LogTag;
			}
			set
			{
                LogTag = value;
			}
		}
		public Guid SignalId
		{
			get;
			set;
		}
		public string LogTag
		{
			get;
			set;
		}
		public bool Active
		{
			get;
			set;
		}
	}
}
