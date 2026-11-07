// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Modules.Build.Services.Builder
{
	public interface IProjectRunnerService
	{
		void Run();
		void Stop();
	}
}
