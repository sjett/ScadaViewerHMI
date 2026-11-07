// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.ObjectModel;
using HMI.Common.Presentation.ViewModels;
using HMI.Modules.Application.Models;
namespace HMI.Modules.Application.Viewmodels
{
	public class SelectLanguageViewModel : DisposableViewModel, ISelectLanguageViewModel
	{
		private ObservableCollection<Language> _languages;
		private string _selectedLanguage;
		public ObservableCollection<Language> Languages
		{
			get
			{
				if (_languages == null)
				{
					_languages = new ObservableCollection<Language>
					{
						new Language("English", "en-US", 9),
						new Language("中文", "zh-CN", 2052)//微软官方建议vista之后的系统用zh-Hans 编号 4
					};
				}
				return _languages;
			}
		}
		public string SelectedLanguage
		{
			get
			{
				return _selectedLanguage;
			}
			set
			{
                _selectedLanguage = value;
				base.RaisePropertyChanged<string>(() => SelectedLanguage);
			}
		}
	}
}
