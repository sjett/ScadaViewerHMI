// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Runtime.Shared
{
	public interface IViewModel : IDisposable
	{
		bool IsInitialized
		{
			get;
		}
		void Initialize();
	}
}
