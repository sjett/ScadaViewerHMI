// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using HMI.Modules.Application.Models;
namespace HMI.Modules.Application.Viewmodels
{
	public interface ISelectLanguageViewModel
	{
		ObservableCollection<Language> Languages
		{
			get;
		}
		string SelectedLanguage
		{
			get;
			set;
		}
	}
}
