// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
namespace HMI.Runtime.Core
{
	public class HMIEntity
	{
		public Guid ID
		{
			get;
			set;
		}
		public string Name
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
	}
}
