// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using System.ComponentModel;
namespace HMI.Modules.ControlTree.ViewModels
{
	public class StaticSymbolDefinitionViewModel : IStaticSymbolViewModel, INotifyPropertyChanged
	{
		private string _contentString;

		public event EventHandler<ValueChangedEventArgs<string>>? ContentStringChanged;

		public event PropertyChangedEventHandler? PropertyChanged;
		public string ContentString
		{
			get
			{
				return _contentString;
			}
			set
			{
				if (_contentString != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(_contentString, value);
                    _contentString = value;
                    ContentStringChanged?.Invoke(this, e);
                    PropertyChanged?.Raise(this, () => ContentString);
				}
			}
		}
	}
}
