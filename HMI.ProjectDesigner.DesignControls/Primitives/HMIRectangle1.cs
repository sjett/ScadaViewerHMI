// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Primitives
{
	public class HMIRectangle1 : HMIPrimitiveBase
	{
		static HMIRectangle1()
		{
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIRectangle1), new FrameworkPropertyMetadata(typeof(HMIRectangle1)));
		}
	}
}
