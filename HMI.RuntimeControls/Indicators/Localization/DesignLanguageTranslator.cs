// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Core.Helper;
using System.Globalization;
namespace HMI.RuntimeControls.Indicators.Localization
{
	public class DesignLanguageTranslator : IDisposable, ILanguageTranslator
	{
		private int currentLanguageId;
		public event EventHandler? LanguageChanged;

		public string North
		{
			get
			{
				return "N";
			}
		}
		public string NorthWest
		{
			get
			{
				return "NW";
			}
		}
		public string West
		{
			get
			{
				return "W";
			}
		}
		public string SouthWest
		{
			get
			{
				return "SW";
			}
		}
		public string South
		{
			get
			{
				return "S";
			}
		}
		public string SouthEast
		{
			get
			{
				return "SE";
			}
		}
		public string East
		{
			get
			{
				return "E";
			}
		}
		public string NorthEast
		{
			get
			{
				return "NE";
			}
		}
		public int CurrentLanguageId
		{
			
			get
			{
				return this.currentLanguageId;
			}
		}
		public CultureInfo Culture
		{
			get
			{
				return CultureInfoCache.GetCultureInfo(this.CurrentLanguageId);
			}
		}
		public void RegisterSymbolicTexts(string[] symbolicTexts)
		{
		}
		public string GetTranslation(string symbolicText)
		{
			return symbolicText;
		}
		public void Dispose()
		{
		}
	}
}
