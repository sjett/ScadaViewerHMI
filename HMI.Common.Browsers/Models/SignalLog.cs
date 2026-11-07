// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
namespace HMI.Common.Browsers.Models
{
	public class SignalLog : Model
	{
		public string LogTag
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
		public SignalLog()
		{
		}
	}
}
