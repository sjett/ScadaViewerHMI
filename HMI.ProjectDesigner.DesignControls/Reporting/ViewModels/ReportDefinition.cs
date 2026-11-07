// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.ViewModels;
namespace HMI.ProjectDesigner.DesignControls.Reporting.ViewModels
{
	public class ReportDefinition : ViewModel
	{
		private string name;
		private string description;
		public event EventHandler<ValueChangedEventArgs<string>> NameChanged;
		public event EventHandler<ValueChangedEventArgs<string>> DescriptionChanged;
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
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(name, value);
					RaisePropertyChanging(() => Name);
					name = value;
					NameChanged.Raise(this, e);
					RaisePropertyChanged(() => Name);
				}
			}
		}
		public string Description
		{
			get
			{
				return description;
			}
			set
			{
				if (description != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(description, value);
					RaisePropertyChanging(() => Description);
					description = value;
					DescriptionChanged.Raise(this, e);
					RaisePropertyChanged(() => Description);
				}
			}
		}
	}
}
