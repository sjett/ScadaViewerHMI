// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Primitives
{
	public class HMIAnimatedEllipse1 : HMIPrimitiveBase
	{
		static HMIAnimatedEllipse1()
		{
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAnimatedEllipse1), new FrameworkPropertyMetadata(typeof(HMIAnimatedEllipse1)));
		}
	}
}
