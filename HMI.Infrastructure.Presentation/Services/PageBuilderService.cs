// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Common.Instrumentation.ErrorHandling;
namespace HMI.Infrastructure.Presentation.Services
{
    public class PageBuilderService : IPageBuilderService
	{
		private readonly IProjectNameService _projectNameService;
		private readonly IServiceProvider services;
		public PageBuilderService(IServiceProvider services, IProjectNameService projectNameService)
		{
			Contract.Requires<ArgumentNullException>(services != null, "services");
			Contract.Requires<ArgumentNullException>(projectNameService != null, "projectNameService");

            _projectNameService = projectNameService;
            this.services = services;
		}
		public TPage AddPage<TPage>(IExplorerNodeViewModel parent, string name) where TPage : IProjectPageViewModel
		{
			Contract.Requires<ArgumentNullException>(parent != null, "parent");
			Contract.Requires<ArgumentNullException>(name != null, "name");
			IProjectViewModel project = parent.Project;
            var pageType = typeof(TPage) == typeof(ITemplatePageViewModel)
                ? typeof(TemplatePageViewModel)
                : typeof(ProjectPageViewModel);
            TPage tPage = (TPage)ActivatorUtilities.CreateInstance(services, pageType, parent, name);
			tPage.Background = project.PageDefaultBackground;
			parent.Children.Add(tPage);
			project.Pages.Add(tPage);
			return tPage;
		}
	}
}
