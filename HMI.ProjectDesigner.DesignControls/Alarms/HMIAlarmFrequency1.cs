// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Alarms
{
	public class HMIAlarmFrequency1 : HMIHighCountAlarmStatisticBase
	{
		public override string YTitle
		{
			get
			{
				return "数量";
			}
		}
		static HMIAlarmFrequency1()
		{
			System.Windows.FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAlarmFrequency1), new System.Windows.FrameworkPropertyMetadata(typeof(HMIAlarmFrequency1)));
		}
	}
}
