// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public class HMIAlarmDuration1 : HMIHighCountAlarmStatisticBase
	{
		public override string YTitle
		{
			get
			{
				return "分钟";
			}
		}
		public override double PieLabelOffset
		{
			get
			{
				return 0.7;
			}
		}
		static HMIAlarmDuration1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAlarmDuration1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIAlarmDuration1)));
		}
	}
}
