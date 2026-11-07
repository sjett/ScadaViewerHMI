// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
using HMI.Common.Browsers.Models;
namespace HMI.Common.Browsers.ViewModels
{
	public class LogSignalDefinition
	{
		public SignalLog Log
		{
			get;
			set;
		}
		public SignalDefinition SignalDefinition
		{
			get;
			set;
		}
		public LogSignalDefinition(SignalLog log, SignalDefinition signalDefinition)
		{
            Log = log;
            SignalDefinition = signalDefinition;
		}
	}
}
