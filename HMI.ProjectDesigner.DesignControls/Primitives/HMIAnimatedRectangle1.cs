// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Primitives
{
	public class HMIAnimatedRectangle1 : HMIPrimitiveBase
	{
		static HMIAnimatedRectangle1()
		{
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIAnimatedRectangle1), new FrameworkPropertyMetadata(typeof(HMIAnimatedRectangle1)));
		}
	}
}
