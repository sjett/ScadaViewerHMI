// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Browsers.Models
{
	public class SymbolicText : Model
	{
		public string SymbolicTextName
		{
			get;
			set;
		}
		public List<SymbolicTextTranslation> Translations
		{
			get;
			internal set;
		}
		public SymbolicText()
		{
			this.Translations = new List<SymbolicTextTranslation>();
		}
	}
}
