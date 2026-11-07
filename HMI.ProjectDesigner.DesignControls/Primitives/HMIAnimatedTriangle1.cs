// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Primitives
{
	public class HMIAnimatedTriangle1 : HMIPrimitiveBase
	{
		static HMIAnimatedTriangle1()
		{
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAnimatedTriangle1), new FrameworkPropertyMetadata(typeof(HMIAnimatedTriangle1)));
		}
	}
}
