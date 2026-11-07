// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
namespace HMI.Runtime.Core.Helper.Translations
{
	public class TranslationModel : IDisposable, ITranslationModel
	{

		public string SymbolicText
		{
			get;
			set;
		}
		public Dictionary<int, string> Translations
		{
			get;
			set;
		}
		public IDictionary<OnLanguageChangedDelegate, int> Delegates
		{
			get;
			set;
		}
		public TranslationModel()
		{
            Delegates = new Dictionary<OnLanguageChangedDelegate, int>();
            Translations = new Dictionary<int, string>();
		}
		public void Dispose()
		{
			if (Translations != null)
			{
                Translations.Clear();
                Translations = null;
			}
			if (Delegates != null)
			{
                Delegates.Clear();
                Delegates = null;
			}
		}
	}
}
