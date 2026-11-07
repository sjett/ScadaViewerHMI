// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.ViewModels;
using System.Windows;

namespace HMI.Modules.Design.ViewModels
{
	public class CornerRadiusViewModel : ViewModel, ICornerRadiusViewModel
	{
		private CornerRadius _cornerRadius;

		public event EventHandler<ValueChangedEventArgs<CornerRadius>>? CornerRadiusChanged;

		public CornerRadius CornerRadius
		{
			get
			{
				return this._cornerRadius;
			}
			set
			{
				SetCornerRadius(value);
			}
		}
		public double TopLeft
		{
			get
			{
				return this._cornerRadius.TopLeft;
			}
			set
			{
				this.SetTopLeft(value);
			}
		}
		public double TopRight
		{
			get
			{
				return this._cornerRadius.TopRight;
			}
			set
			{
				this.SetTopRight(value);
			}
		}
		public double BottomRight
		{
			get
			{
				return this._cornerRadius.BottomRight;
			}
			set
			{
				this.SetBottomRight(value);
			}
		}
		public double BottomLeft
		{
			get
			{
				return this.CornerRadius.BottomLeft;
			}
			set
			{
				this.SetBottomLeft(value);
			}
		}
		private void SetTopLeft(double value)
		{
            UpdateCornerRadius(this.BottomRight, value, this.TopRight, this.BottomLeft);
		}
		private void SetTopRight(double value)
		{
            UpdateCornerRadius(this.BottomRight, this.TopLeft, value, this.BottomLeft);
		}
		private void SetBottomRight(double value)
		{
            UpdateCornerRadius(value, this.TopLeft, this.TopRight, this.BottomLeft);
		}
		private void SetBottomLeft(double value)
		{
            UpdateCornerRadius(this.BottomRight, this.TopLeft, this.TopRight, value);
		}
        private void UpdateCornerRadius(double bottomRight, double topLeft, double topRight, double bottomLeft)
        {
            SetCornerRadius(new CornerRadius(topLeft, topRight, bottomRight, bottomLeft));
        }
        private void SetCornerRadius(CornerRadius cornerRadius)
        {
            CornerRadius previousCornerRadius = this._cornerRadius;
            if (previousCornerRadius == cornerRadius)
            {
                return;
            }
            _cornerRadius = cornerRadius;
            CornerRadiusChanged?.Invoke(this, new ValueChangedEventArgs<CornerRadius>(previousCornerRadius, cornerRadius));
            RaisePropertyChanged<CornerRadius>(() => CornerRadius);
            RaisePropertyChanged<double>(() => TopLeft);
            RaisePropertyChanged<double>(() => TopRight);
            RaisePropertyChanged<double>(() => BottomRight);
            RaisePropertyChanged<double>(() => BottomLeft);
        }
    }
}
