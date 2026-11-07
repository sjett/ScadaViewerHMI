// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Threading;
namespace HMI.RuntimeControls.Gauges
{
    /// <summary>
    /// 根据输入信号值计算仪表指针对应的角度，并提供线性和正弦两种映射方式。
    /// </summary>
    public class CalculateAngle : ICalculateAngle
	{
		private double angle;
		private bool isSinusoidal;
		private double maximumAngle;
		private double maximumValue;
		private double minimunAngle;
		private double minimumValue;

		private int fillProcentage;

		public event EventHandler? FillProcentageChanged;
        /// <summary>
        /// 返回当前值在量程中的百分比，范围通常为 0 到 100。
        /// </summary>
        public int FillProcentage
		{
			get
			{
				return this.fillProcentage;
			}
		}
        /// <summary>
        /// 指定是否采用正弦映射计算角度。
        /// </summary>
        public bool IsSinusoidal
		{
			set
			{
				this.isSinusoidal = value;
			}
		}
		public double MaximumAngle
		{
			set
			{
				this.maximumAngle = value;
			}
		}
		public double MaximumValue
		{
			set
			{
				this.maximumValue = value;
			}
		}
		public double MinimunAngle
		{
			set
			{
				this.minimunAngle = value;
			}
		}
		public double MinimumValue
		{
			set
			{
				this.minimumValue = value;
			}
		}
		public double GetAngleValue(double signalValue)
		{
			this.CalculateAngleForValue(signalValue);
			return this.angle;
		}
		private void CalculateAngleForValue(double signalValue)
		{
			if (double.IsNaN(signalValue))
			{
				this.ResetAngle();
			}
			this.CalculateAngleForClampedValue(signalValue);
		}
		private void ResetAngle()
		{
			this.angle = 0.0;
		}
		private void CalculateAngleForClampedValue(double signalValue)
		{
			if (signalValue < this.minimumValue)
			{
				if (this.isSinusoidal)
				{
					this.CalculateSinusoidalAngle(this.minimumValue);
					return;
				}
				this.CalculateLinearAngle(this.minimumValue);
				return;
			}
			else
			{
				if (signalValue <= this.maximumValue)
				{
					this.CalculateAngleForInRangeValue(signalValue);
					return;
				}
				if (this.isSinusoidal)
				{
					this.CalculateSinusoidalAngle(this.maximumValue);
					return;
				}
				this.CalculateLinearAngle(this.maximumValue);
				return;
			}
		}
		private void CalculateAngleForInRangeValue(double signalValue)
		{
			if (this.minimumValue >= 0.0 && this.maximumValue > 0.0)
			{
				this.CalculateLinearAngle(signalValue);
			}
			if (this.isSinusoidal)
			{
				this.CalculateSinusoidalAngle(signalValue);
			}
		}
		private void CalculateLinearAngle(double signalValue)
		{
			this.UpdateAngle(this.GetValueRange(), this.GetValueOffset(signalValue));
		}
		private void CalculateSinusoidalAngle(double signalValue)
		{
			this.UpdateAngle(this.GetCombinedAbsoluteRange(
				this.GetAbsoluteValue(this.minimumValue),
				this.GetAbsoluteValue(this.maximumValue)), signalValue);
		}
		private double GetValueOffset(double signalValue)
		{
			return signalValue - this.minimumValue;
		}
		private double GetCombinedAbsoluteRange(double minimum, double maximum)
		{
			return maximum + minimum;
		}
		private double GetValueRange()
		{
			return this.maximumValue - this.minimumValue;
		}
		private void UpdateAngle(double totalRange, double valueOffset)
		{
			double percentage = Convert.ToDouble(valueOffset / totalRange * 100.0);
			double anglePerPercent = this.maximumAngle / 100.0;

			if (this.isSinusoidal)
			{
				this.angle = anglePerPercent * percentage;
				return;
			}

			this.angle = anglePerPercent * percentage + this.minimunAngle;
		}
		private double GetAbsoluteValue(double value)
		{
			if (value < 0.0)
			{
				return value *= -1.0;
			}
			return value;
		}
	}
}
