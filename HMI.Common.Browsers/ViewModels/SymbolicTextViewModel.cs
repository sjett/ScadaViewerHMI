// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Common.Browsers.Models;
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public class SymbolicTextViewModel : ViewModel, IBrowserItemViewModel, ISymbolicTextViewModel
	{
		private Guid id;
		private string symbolicTextName;


		public event EventHandler<ValueChangedEventArgs<Guid>>? IDChanged;

		public event EventHandler<ValueChangedEventArgs<string>>? SymbolicTextNameChanged;

		public Guid ID
		{
			get
			{
				return id;
			}
			set
			{
				if (id != value)
				{
					base.RaisePropertyChanging<Guid>(() => ID);
					ValueChangedEventArgs<Guid> e = new ValueChangedEventArgs<Guid>(id, value);
                    id = value;
                    IDChanged?.Raise(this, e);
					base.RaisePropertyChanged<Guid>(() => ID);
				}
			}
		}
		public string SymbolicTextName
		{
			get
			{
				return symbolicTextName;
			}
			set
			{
				if (symbolicTextName != value)
				{
					base.RaisePropertyChanging<string>(() => SymbolicTextName);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(symbolicTextName, value);
                    symbolicTextName = value;
                    SymbolicTextNameChanged?.Raise(this, e);
					base.RaisePropertyChanged<string>(() => SymbolicTextName);
				}
			}
		}
		public IDictionary<int, string> Translations
		{
			get;
			private set;
		}
		public string ItemText
		{
			get
			{
				return SymbolicTextName;
			}
		}
		public SymbolicTextViewModel(SymbolicText symbolicText) : this()
		{
			Contract.Requires<ArgumentNullException>(symbolicText != null, "symbolicText");

            ID = symbolicText!.Id;
            SymbolicTextName = symbolicText.SymbolicTextName;

            Translations = symbolicText.Translations.ToDictionary(ks => ks.LanguageID, ele => ele.TranslationText);
		}
		public SymbolicTextViewModel()
		{
            Translations = new Dictionary<int, string>();
		}
		
	}
}
