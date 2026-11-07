// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Common.Browsers.Models;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public class TranslatableViewModel : ViewModel, IBrowserItemViewModel, ITranslatableViewModel
	{
		private Guid id;
		private string name;
		private string translation;
		public Guid Id
		{
			get
			{
				return id;
			}
			set
			{
				if (id != value)
				{
					base.RaisePropertyChanging<Guid>(() => Id);
                    id = value;
					base.RaisePropertyChanged<Guid>(() => Id);
				}
			}
		}
		public string Name
		{
			get
			{
				return name;
			}
			set
			{
				if (name != value)
				{
					base.RaisePropertyChanging<string>(() => Name);
                    name = value;
					base.RaisePropertyChanged<string>(() => Name);
				}
			}
		}
		public string Translation
		{
			get
			{
				return translation;
			}
			set
			{
				if (translation != value)
				{
					base.RaisePropertyChanging<string>(() => Translation);
                    translation = value;
					base.RaisePropertyChanged<string>(() => Translation);
				}
			}
		}
		public string ItemText
		{
			get
			{
				return Name;
			}
		}
		public TranslatableViewModel(TranslatedEntity translatedEntity)
		{
			Contract.Requires<ArgumentNullException>(translatedEntity != null, "itemModel");
            Id = translatedEntity.Id;
            Name = translatedEntity.Name;
            Translation = translatedEntity.Translation;
		}
	}
}
