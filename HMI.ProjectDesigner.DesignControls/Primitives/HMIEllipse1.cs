// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Primitives
{
	public class HMIEllipse1 : HMIPrimitiveBase
	{
		static HMIEllipse1()
		{
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIEllipse1), new FrameworkPropertyMetadata(typeof(HMIEllipse1)));
		}
	}
}
