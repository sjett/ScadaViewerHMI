// SPDX-License-Identifier: GPL-3.0-only
using System;
using HMI.Infrastructure.Presentation.Views;
namespace HMI.ProjectDesigner.DesignControls.Charts
{
	public abstract class HMITrendingBase : CommonControl
	{
		public enum TrendingTimeZone
		{
			ClientTime,
			ServerTime,
			UTCTime
		}
	}
}
