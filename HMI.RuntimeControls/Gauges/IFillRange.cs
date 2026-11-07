// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows.Media;
namespace HMI.RuntimeControls.Gauges
{
	public interface IFillRange : INotifyPropertyChanged
	{
		string Name
		{
			get;
			set;
		}
		Brush Fill
		{
			get;
			set;
		}
		double Begin
		{
			get;
			set;
		}
		double End
		{
			get;
			set;
		}
	}
}
