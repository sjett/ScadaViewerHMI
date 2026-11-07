// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Editor.Surface
{
	public class ZoomFactorChangedEventArgs : EventArgs
    {
		public double zoomFactor;
		public double InverseZoomFactor
		{
			get
			{
				return 1.0 / zoomFactor;
			}
		}
	}
}
