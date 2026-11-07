// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows.Media;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.Gauges
{
	public class FillRange : PropertyChangedNotifier, IFillRange
	{
		private string name;
		private Brush fill;
		private double begin;
		private double end;

		[Category("Default")]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
				base.RaisePropertyChanged<string>(() => this.Name);
			}
		}
		[Category("Brushes")]
		public Brush Fill
		{
			get
			{
				return this.fill;
			}
			set
			{
				this.fill = value;
				base.RaisePropertyChanged<Brush>(() => this.Fill);
			}
		}
		[Category("Range")]
		public double Begin
		{
			get
			{
				return this.begin;
			}
			set
			{
				this.begin = value;
				this.OnPropertyChanged("LeftPadding");
			}
		}
		[Category("Range")]
		public double End
		{
			get
			{
				return this.end;
			}
			set
			{
				this.end = value;
				this.OnPropertyChanged("AreaLength");
			}
		}
	}
}
