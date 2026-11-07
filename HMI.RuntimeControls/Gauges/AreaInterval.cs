// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using System.Windows.Media;
using HMI.Runtime.Shared.Common;
namespace HMI.RuntimeControls.Gauges
{
	public class AreaInterval : PropertyChangedNotifier, IAreaInterval
	{
		private string _name;
		private double _oldLengthValue;
		private double _radiusX;
		private double _radiusY;
		private double _leftPadding;
		private double _areaLength;
		private Brush _fill;
		private Brush _strokeColor;
		private double _strokeThickness;

		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
			}
		}
		[Browsable(false)]
		public double OldLengthValue
		{
			get
			{
				return this._oldLengthValue;
			}
			set
			{
				this._oldLengthValue = value;
			}
		}
		[Category("Layout")]
		public double RadiusX
		{
			get
			{
				return this._radiusX;
			}
			set
			{
				if (value < 0.0)
				{
					return;
				}
				this._radiusX = value;
				base.RaisePropertyChanged<double>(() => this.RadiusX);
			}
		}
		[Category("Layout")]
		public double RadiusY
		{
			get
			{
				return this._radiusY;
			}
			set
			{
				if (value < 0.0)
				{
					return;
				}
				this._radiusY = value;
				base.RaisePropertyChanged<double>(() => this.RadiusY);
			}
		}
		[Category("Layout")]
		public double LeftPadding
		{
			get
			{
				return this._leftPadding;
			}
			set
			{
				if (value < 0.0)
				{
					return;
				}
				this._leftPadding = value;
				base.RaisePropertyChanged<double>(() => this.LeftPadding);
			}
		}
		[Category("Layout")]
		public double AreaLength
		{
			get
			{
				return this._areaLength;
			}
			set
			{
				if (value < 0.0)
				{
					return;
				}
				this._areaLength = value;
				if (this._areaLength != this._oldLengthValue)
				{
					base.RaisePropertyChanged<double>(() => this.AreaLength);
					this._oldLengthValue = this._areaLength;
				}
			}
		}
		[Category("Brushes")]
		public Brush Fill
		{
			get
			{
				return this._fill;
			}
			set
			{
				this._fill = value;
				base.RaisePropertyChanged<Brush>(() => this.Fill);
			}
		}
		[Category("Brushes")]
		public Brush StrokeColor
		{
			get
			{
				return this._strokeColor;
			}
			set
			{
				this._strokeColor = value;
				base.RaisePropertyChanged<Brush>(() => this.StrokeColor);
			}
		}
		[Category("Layout")]
		public double StrokeThickness
		{
			get
			{
				return this._strokeThickness;
			}
			set
			{
				if (value < 0.0)
				{
					return;
				}
				this._strokeThickness = value;
				base.RaisePropertyChanged<double>(() => this.StrokeThickness);
			}
		}
	}
}
