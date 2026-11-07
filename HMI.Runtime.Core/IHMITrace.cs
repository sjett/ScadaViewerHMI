// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Core
{
	public interface IHMITrace : IDisposable
	{
		void OutputDebugText(HMIDebugLevel level, string textFormat, params object[] textFormatParams);
	}
}
