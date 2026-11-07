// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Common.Presentation.Events;
namespace HMI.Modules.ControlTree
{
	public class PrimitiveModel : PropertyChangedNotifier
	{
		private string _name;
		private string _image;
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
		public string Image
		{
			get
			{
				return _image;
			}
			set
			{
				if (_image != value)
				{
                    _image = value;
					base.RaisePropertyChanged<string>(() => Image);
				}
			}
		}
	}
}
