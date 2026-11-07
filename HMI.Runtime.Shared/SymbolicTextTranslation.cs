// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Runtime.Shared.Common;
namespace HMI.Runtime.Shared
{
	public class SymbolicTextTranslation : PropertyChangedNotifier
	{
		private string symbolicText;
		private string translation;
		public string SymbolicText
		{
			get
			{
				return this.symbolicText;
			}
		}
		public string Translation
		{
			get
			{
				return this.translation;
			}
			set
			{
				this.translation = value;
				base.RaisePropertyChanged<string>(() => this.Translation);
			}
		}
		public SymbolicTextTranslation(string symbolicText) : this(symbolicText, string.Empty)
		{
		}
		public SymbolicTextTranslation(string symbolicText, string translation)
		{
			this.symbolicText = symbolicText;
			this.translation = translation;
		}
	}
}
