// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.Presentation;
namespace HMI.Modules.Design.ViewModels
{
	public class DateRangeViewModel : ViewModel
	{
		
		public string Text
		{
			get;
			set;
		}
		public DateRange Range
		{
			get;
			set;
		}
	}
}
