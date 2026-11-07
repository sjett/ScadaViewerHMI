// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Threading;
namespace HMI.Modules.Build.Services.Builder
{
	public interface IProjectBuilderService
	{
		bool Prepare(SynchronizationContext? synchronizationContext);
		bool Build();
		bool Publish();
		bool CompleteBuild();
	}
}
