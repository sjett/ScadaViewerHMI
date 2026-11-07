// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.ProjectDesigner.DesignControls.ViewModels.DataTable
{
	public class TableLine
	{
		public int Index
		{
			get;
			set;
		}
		public System.DateTime Time
		{
			get;
			set;
		}
		public int Signal1
		{
			get;
			set;
		}
		public double Signal2
		{
			get;
			set;
		}
		public string Signal3
		{
			get;
			set;
		}
	}
}
