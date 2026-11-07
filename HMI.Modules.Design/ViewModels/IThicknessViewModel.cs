// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using System.Windows;
namespace HMI.Modules.Design.ViewModels
{
	public interface IThicknessViewModel
	{
		event EventHandler<ValueChangedEventArgs<Thickness>>? ThicknessChanged;
		Thickness Thickness
		{
			get;
			set;
		}
		double Left
		{
			get;
			set;
		}
		double Top
		{
			get;
			set;
		}
		double Right
		{
			get;
			set;
		}
		double Bottom
		{
			get;
			set;
		}
	}
}
