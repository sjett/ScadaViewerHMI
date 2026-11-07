// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics;
using HMI.Infrastructure.Presentation.ViewModels;
using System.IO;

namespace HMI.Modules.Build.Services.Builder
{
	internal class ProjectRunnerService : IProjectRunnerService
	{
		private readonly IProjectManagerViewModel projectManagerViewModel;
		private Process? process;

		public ProjectRunnerService(IProjectManagerViewModel projectManagerViewModel)
		{
			this.projectManagerViewModel = projectManagerViewModel ?? throw new ArgumentNullException(nameof(projectManagerViewModel));
		}

		public void Run()
		{
			Stop();

			string publishFolder = Path.Combine(projectManagerViewModel.Project.AppProjectFolder, "Publish");
			string executablePath = Path.Combine(publishFolder, projectManagerViewModel.Project.ProjectName + ".exe");
			if (!File.Exists(executablePath))
			{
				throw new FileNotFoundException("The published runtime application was not found.", executablePath);
			}

			process = Process.Start(new ProcessStartInfo
			{
				FileName = executablePath,
				WorkingDirectory = publishFolder,
				UseShellExecute = false
			}) ?? throw new InvalidOperationException($"Failed to start runtime application '{executablePath}'.");
		}

		public void Stop()
		{
			Process? currentProcess = process;
			process = null;
			if (currentProcess is null)
			{
				return;
			}

			using (currentProcess)
			{
				if (currentProcess.HasExited)
				{
					return;
				}

				currentProcess.CloseMainWindow();
				if (!currentProcess.WaitForExit(3000))
				{
					currentProcess.Kill(entireProcessTree: true);
					currentProcess.WaitForExit();
				}
			}
		}
	}
}
