// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Modules.Application.Models
{
	public class Language
	{
		private string _name;
		private string _culture;
		private int _languageNumber;
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
                _name = value;
			}
		}
		public string Culture
		{
			get
			{
				return _culture;
			}
			set
			{
                _culture = value;
			}
		}
		public int Number
		{
			get
			{
				return _languageNumber;
			}
			set
			{
                _languageNumber = value;
			}
		}
		public Language(string name, string culture, int number)
		{
            Name = name;
            Culture = culture;
            Number = number;
		}
		public override string ToString()
		{
			return string.Format("[Language : {0}]", Name);
		}
	}
}
