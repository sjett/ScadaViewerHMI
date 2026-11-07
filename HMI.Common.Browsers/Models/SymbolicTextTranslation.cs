// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.CompilerServices;
namespace HMI.Common.Browsers.Models
{
	public class SymbolicTextTranslation
	{
		public int LanguageID
		{
			get;
			set;
		}
		public string TranslationText
		{
			get;
			set;
		}
		public SymbolicTextTranslation()
		{
		}
	}
}
