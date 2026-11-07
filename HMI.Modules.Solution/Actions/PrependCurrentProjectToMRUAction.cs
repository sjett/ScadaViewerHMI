// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Services;
namespace HMI.Modules.Solution.Actions
{
    public class PrependCurrentProjectToMRUAction : SolutionAction
	{
		protected Lazy<IMruService> MRUService
		{
			get;
			private set;
		}
		public PrependCurrentProjectToMRUAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            MRUService = new Lazy<IMruService>(serviceLocator.Resolve<Func<IMruService>>());
		}
		public override bool Execute(IWorkflow workflow)
		{
            MRUService.Value.PrependProjectToMRU(Project.ProjectFullPath);
			return true;
		}
	}
}
