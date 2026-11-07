// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared
{
	public class HMIStateCondition : HMIStaticStateCondition
	{
		~HMIStateCondition()
		{
            Dispose(false);
		}
		protected override void Dispose(bool disposing)
		{
			base.Dispose(disposing);
		}
		public override string ToString()
		{
			return base.StateName;
		}
	}
}
