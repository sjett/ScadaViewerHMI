// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Common.Browsers.Models;
using HMI.Common.Presentation.ViewModels;
namespace HMI.Common.Browsers.ViewModels
{
	public class ProjectAuthorizationViewModel : ViewModel, IBrowserItemViewModel, IProjectAuthorizationViewModel
	{
		private Guid _id;
		private string _name;
		private string _description;
		public Guid ID
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
					base.RaisePropertyChanged<Guid>(() => ID);
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
		public string Description
		{
			get
			{
				return _description;
			}
			set
			{
				if (_description == value)
				{
					return;
				}
                _description = value;
				base.RaisePropertyChanged<string>(() => Description);
			}
		}
		public string ItemText
		{
			get
			{
				return Name;
			}
		}
		public ProjectAuthorizationViewModel(ProjectAuthorization itemModel)
		{
			Contract.Requires<ArgumentNullException>(itemModel != null, "itemModel");
            ID = itemModel.Id;
            Name = itemModel.Name;
            Description = itemModel.Description;
		}
	}
}
