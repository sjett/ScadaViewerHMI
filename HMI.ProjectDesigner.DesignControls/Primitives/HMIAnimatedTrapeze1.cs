// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Primitives
{
	public class HMIAnimatedTrapeze1 : HMIPrimitiveBase
	{
		static HMIAnimatedTrapeze1()
		{
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAnimatedTrapeze1), new FrameworkPropertyMetadata(typeof(HMIAnimatedTrapeze1)));
		}
	}
}
