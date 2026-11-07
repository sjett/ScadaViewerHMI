// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Services;

namespace HMI.Modules.Solution.Actions
{
    public class ClearProjectAction : SolutionAction
	{
		public ClearProjectAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
		}
		public override bool Execute(IWorkflow workflow)
		{
			IActionMonitor actionMonitor = ServiceLocator.Resolve<IActionMonitor>();
			actionMonitor.Disable();
			IProjectNameService projectNameService = ServiceLocator.Resolve<IProjectNameService>();
			projectNameService.ResetNameDatabase();
            Project = null;
			return true;
		}
	}
}
