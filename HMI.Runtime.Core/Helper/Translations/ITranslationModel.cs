// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
namespace HMI.Runtime.Core.Helper.Translations
{
	public interface ITranslationModel : IDisposable
	{
		string SymbolicText
		{
			get;
			set;
		}
		Dictionary<int, string> Translations
		{
			get;
			set;
		}
		IDictionary<OnLanguageChangedDelegate, int> Delegates
		{
			get;
			set;
		}
	}
}
