// SPDX-License-Identifier: GPL-3.0-only
using System.Configuration;
namespace HMI.Common.DataAccess
{
    public class ConfigFileConnectionStringProvider : IConnectionStringProvider
    {
        public string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["DataEntities"].ConnectionString;
        }
    }
}
