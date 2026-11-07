// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Primitives
{
	public class HMITriangle1 : HMIPrimitiveBase
	{
		static HMITriangle1()
		{
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMITriangle1), new FrameworkPropertyMetadata(typeof(HMITriangle1)));
		}
	}
}
