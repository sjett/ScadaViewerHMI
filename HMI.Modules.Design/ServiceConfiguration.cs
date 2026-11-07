// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Services;
using HMI.Domain.Models;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Infrastructure.Presentation.ViewModels;
namespace HMI.Modules.Design
{
	public class ServiceConfiguration : IServiceConfiguration
	{
		private readonly ISolution solution;
		//public Uri BaseUri
		//{
		//	get
		//	{
		//		return this.solution.Project.WebServicesBaseUri;
		//	}
		//}
		public string ServerName
		{
			get
			{
				return this.solution.Project.ServerName;
			}
		}

        public string Host => (this.solution.Project as IProjectViewModel).Host;

        public int Port => (this.solution.Project as IProjectViewModel).Port;

        public int Timeout => (this.solution.Project as IProjectViewModel).Timeout;

        public string Username => (this.solution.Project as IProjectViewModel).Username;

        public string Password => (this.solution.Project as IProjectViewModel).Password;

        public string SecretKey => (this.solution.Project as IProjectViewModel).SecretKey;

		//public string Instance { get; }

        public ServiceConfiguration(ISolution solution)
		{
			Contract.Requires<ArgumentNullException>(solution != null, "solution");
			this.solution = solution;
		}
	}
}
