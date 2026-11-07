// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Editor.Surface
{
	public class CoordinatesUpdatedEventArgs : EventArgs
    {
		public double Top
		{
			get;
			set;
		}
		public double Left
		{
			get;
			set;
		}
		public double Width
		{
			get;
			set;
		}
		public double Height
		{
			get;
			set;
		}
	}
}
