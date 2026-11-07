// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Primitives
{
	public class HMITrapeze1 : HMIPrimitiveBase
	{
		static HMITrapeze1()
		{
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITrapeze1), new FrameworkPropertyMetadata(typeof(HMITrapeze1)));
		}
	}
}
