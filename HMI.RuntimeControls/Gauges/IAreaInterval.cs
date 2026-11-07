// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows.Media;
namespace HMI.RuntimeControls.Gauges
{
	public interface IAreaInterval : INotifyPropertyChanged
	{
		string Name
		{
			get;
			set;
		}
		double OldLengthValue
		{
			get;
			set;
		}
		double RadiusX
		{
			get;
			set;
		}
		double RadiusY
		{
			get;
			set;
		}
		double LeftPadding
		{
			get;
			set;
		}
		double AreaLength
		{
			get;
			set;
		}
		Brush Fill
		{
			get;
			set;
		}
		Brush StrokeColor
		{
			get;
			set;
		}
		double StrokeThickness
		{
			get;
			set;
		}
	}
}
