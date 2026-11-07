// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
namespace HMI.RuntimeControls.Indicators.Localization
{
	public interface ILanguageTranslator : IDisposable
	{
		event EventHandler LanguageChanged;
		string North
		{
			get;
		}
		string NorthWest
		{
			get;
		}
		string West
		{
			get;
		}
		string SouthWest
		{
			get;
		}
		string South
		{
			get;
		}
		string SouthEast
		{
			get;
		}
		string East
		{
			get;
		}
		string NorthEast
		{
			get;
		}
		int CurrentLanguageId
		{
			get;
		}
		CultureInfo Culture
		{
			get;
		}
		void RegisterSymbolicTexts(string[] symbolicTexts);
		string GetTranslation(string symbolicText);
	}
}
