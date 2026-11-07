// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Models;
using Scada.Data.Models;
using Scada;
using System.Reactive.Linq;

namespace HMI.Common.Browsers.Services
{
    public interface IProjectAuthorizationService
    {
        IObservable<ProjectAuthorization> GetProjectAuthorizations();
    }

    public class ProjectAuthorizationService : IProjectAuthorizationService
    {
        private readonly IClientAccessor clientAccessor;
        public ProjectAuthorizationService(IClientAccessor clientAccessor)
        {
            this.clientAccessor = clientAccessor;
        }

        public IObservable<ProjectAuthorization> GetProjectAuthorizations()
        {
            try
            {
                this.clientAccessor.ScadaClient.GetStatus(out bool serverIsReady, out bool userIsLoggedIn);

                if (!serverIsReady)
                    throw new Exception("Server is not ready");

                var configDatabase = new ConfigDataset();
                this.clientAccessor.ScadaClient.DownloadBaseTable(configDatabase.RoleTable);

                var result = new List<ProjectAuthorization>();
                foreach (var role in configDatabase.RoleTable)
                    result.Add(new ProjectAuthorization() { Name = role.Name, Description = role.Descr });

                return result.ToObservable();
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking server connection", ex);
            }
            finally
            {
                this.clientAccessor.Dispose();
            }
        }

    }
}
