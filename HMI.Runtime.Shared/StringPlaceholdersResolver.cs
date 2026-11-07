// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Controls;
namespace HMI.Runtime.Shared
{
	public class StringPlaceholdersResolver
	{
		private StringPlaceholdersResolver()
		{
		}
		public static string Resolve(string stringWithPlaceholders, Control ctrl)
		{
			if (stringWithPlaceholders == null)
			{
				throw new ArgumentNullException("stringWithPlaceholders");
			}
			if (ctrl == null)
			{
				throw new ArgumentNullException("ctrl");
			}
			return SignalNameBuilder.BuildSignalName(stringWithPlaceholders, ctrl);
		}
	}
}
