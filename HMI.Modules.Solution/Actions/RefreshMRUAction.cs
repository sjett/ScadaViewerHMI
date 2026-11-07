// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Services;

namespace HMI.Modules.Solution.Actions
{
	public class RefreshMRUAction : SolutionAction
	{
		protected Lazy<IMruService> MRUService
		{
			get;
			private set;
		}
		public RefreshMRUAction(IServiceProvider serviceLocator) : base(serviceLocator)
		{
            MRUService = new Lazy<IMruService>(serviceLocator.Resolve<Func<IMruService>>());
		}
		public override bool Execute(IWorkflow workflow)
		{
            Solution.Value.MruItems.Clear();
			IEnumerable<MruEntry> mruItems = MRUService.Value.GetMruItems();
			foreach (MruEntry current in mruItems)
			{
                Solution.Value.MruItems.Add(current);
			}
			return true;
		}
	}
}
