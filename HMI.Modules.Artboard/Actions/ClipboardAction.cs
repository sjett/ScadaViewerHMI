// SPDX-License-Identifier: GPL-3.0-only

using System;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Runtime.CompilerServices;
using HMI.Infrastructure.Commanding;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Artboard.Actions
{
	public abstract class ClipboardAction : IWorkflowAction
	{
		
		private sealed class ServiceProviderResolver
		{
			public IServiceProvider ServiceProvider;
			public IClipboardManagerService ResolveClipboardService()
			{
				return ServiceProvider.Resolve<IClipboardManagerService>();
			}
			public IProjectManagerViewModel ResolveProjectManager()
			{
				return ServiceProvider.Resolve<IProjectManagerViewModel>();
			}
		}
		private readonly Lazy<IClipboardManagerService> _clipboardService;
		private readonly Lazy<IProjectManagerViewModel> _projectManager;
		public IClipboardManagerService ClipboardService
		{
			get
			{
				return _clipboardService.Value;
			}
		}
		public ActionExecutionMode ExecutionMode
		{
			get
			{
				return ActionExecutionMode.Synchronous;
			}
		}
		public IProjectManagerViewModel Solution
		{
			get
			{
				return _projectManager.Value;
			}
		}
		public IProjectPageViewModel ActivePage
		{
			get
			{
				return Solution.ActivePage;
			}
		}
		protected ClipboardAction(IServiceProvider serviceLocator)
		{
            ServiceProviderResolver resolver = new ServiceProviderResolver();
			resolver.ServiceProvider = serviceLocator;
			//base..ctor();
			Contract.Requires<ArgumentNullException>(resolver.ServiceProvider != null, "serviceLocator");
			Func<IClipboardManagerService> clipboardServiceFactory = resolver.ResolveClipboardService;
            _clipboardService = new Lazy<IClipboardManagerService>(clipboardServiceFactory);
			Func<IProjectManagerViewModel> projectManagerFactory = resolver.ResolveProjectManager;
            _projectManager = new Lazy<IProjectManagerViewModel>(projectManagerFactory);
		}
		public abstract bool Execute(IWorkflow workflow);
	}
}
