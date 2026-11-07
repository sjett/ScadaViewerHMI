// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;
namespace HMI.ProjectDesigner.DesignControls.Primitives
{
	public class HMIBorder1 : HMIPrimitiveBase
	{
		static HMIBorder1()
		{
            FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(HMIBorder1), new FrameworkPropertyMetadata(typeof(HMIBorder1)));
		}
	}
}
