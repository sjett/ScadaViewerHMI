// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using System.Windows;
namespace HMI.Modules.Design.ViewModels
{
	public interface ICornerRadiusViewModel
	{
		event EventHandler<ValueChangedEventArgs<CornerRadius>>? CornerRadiusChanged;
		CornerRadius CornerRadius
		{
			get;
			set;
		}
		double TopLeft
		{
			get;
			set;
		}
		double TopRight
		{
			get;
			set;
		}
		double BottomRight
		{
			get;
			set;
		}
		double BottomLeft
		{
			get;
			set;
		}
	}
}
