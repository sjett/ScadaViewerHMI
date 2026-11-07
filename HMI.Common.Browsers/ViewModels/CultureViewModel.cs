// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using HMI.Common.Presentation.ViewModels;

namespace HMI.Common.Browsers.ViewModels
{
	public class CultureViewModel : ViewModel, ICultureViewModel
	{
		private int _id = -1;
		private string _name;
		protected bool isVisible = true;
		public int ID
		{
			get
			{
				return _id;
			}
			set
			{
				if (_id != value)
				{
                    _id = value;
					base.RaisePropertyChanged<int>(() => ID);
				}
			}
		}
		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				if (_name != value)
				{
                    _name = value;
					base.RaisePropertyChanged<string>(() => Name);
				}
			}
		}
		public virtual bool IsVisible
		{
			get
			{
				return isVisible;
			}
			set
			{
				if (isVisible != value)
				{
                    isVisible = value;
					base.RaisePropertyChanged<bool>(() => IsVisible);
				}
			}
		}
		public CultureViewModel(int languageId)
		{
            ID = languageId;
			CultureInfo cultureInfo = new CultureInfo(languageId);
            Name = cultureInfo.DisplayName;
		}
	}
}
