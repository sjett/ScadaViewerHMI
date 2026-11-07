// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
namespace HMI.Runtime.Core
{
	public class HMILanguage
	{
		public int LanguageId
		{
			get;
			set;
		}
		public string Description
		{
			get;
			set;
		}
		public bool IsActive
		{
			get;
			set;
		}
		public bool IsDefault
		{
			get;
			set;
		}
	}
}
