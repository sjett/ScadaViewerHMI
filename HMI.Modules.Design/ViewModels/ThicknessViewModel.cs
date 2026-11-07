// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.ViewModels;
using System.Windows;

namespace HMI.Modules.Design.ViewModels
{
	public class ThicknessViewModel : ViewModel, IThicknessViewModel
	{
		Thickness _thickness;
		public event EventHandler<ValueChangedEventArgs<Thickness>>? ThicknessChanged;
		public Thickness Thickness
		{
			get
			{
				return this._thickness;
			}
			set
			{
				SetThickness(value);
			}
		}
		public double Left
		{
			get
			{
				return this._thickness.Left;
			}
			set
			{
				this.SetLeft(value);
			}
		}
		public double Top
		{
			get
			{
				return this._thickness.Top;
			}
			set
			{
				this.SetTop(value);
			}
		}
		public double Right
		{
			get
			{
				return this._thickness.Right;
			}
			set
			{
				this.SetRight(value);
			}
		}
		public double Bottom
		{
			get
			{
				return this._thickness.Bottom;
			}
			set
			{
				this.SetBottom(value);
			}
		}
		private void SetLeft(double value)
		{
			UpdateThickness(this.Bottom, this.Top, value, this.Right);
		}
		private void SetTop(double value)
		{
			UpdateThickness(this.Bottom, value, this.Left, this.Right);
		}
		private void SetRight(double value)
		{
			UpdateThickness(this.Bottom, this.Top, this.Left, value);
		}
		private void SetBottom(double value)
		{
			UpdateThickness(value, this.Top, this.Left, this.Right);
		}
        private void UpdateThickness(double bottom, double top, double left, double right)
        {
            SetThickness(new Thickness(left, top, right, bottom));
        }
        private void SetThickness(Thickness thickness)
        {
            Thickness previousThickness = this._thickness;
            if (previousThickness == thickness)
            {
                return;
            }
			_thickness = thickness;
            ThicknessChanged?.Invoke(this, new ValueChangedEventArgs<Thickness>(previousThickness, thickness));
            RaisePropertyChanged<Thickness>(() => Thickness);
            RaisePropertyChanged<double>(() => Left);
            RaisePropertyChanged<double>(() => Top);
            RaisePropertyChanged<double>(() => Right);
            RaisePropertyChanged<double>(() => Bottom);
        }
    }
}
