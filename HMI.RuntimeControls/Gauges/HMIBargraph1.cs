// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Shapes;
namespace HMI.RuntimeControls.Gauges
{
	[TemplatePart(Name = "Bar", Type = typeof(Shape))]
	public class HMIBargraph1 : HMIBargraphBase
	{
		public HMIBargraph1()
		{
			base.DefaultStyleKey=(typeof(HMIBargraph1));
		}
	}
}
