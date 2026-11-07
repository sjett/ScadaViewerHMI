// SPDX-License-Identifier: GPL-3.0-only
using Scada;
using Scada.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI.Common.Browsers.Services
{
    public interface IClientAccessor: IDisposable
    {
        ScadaClient ScadaClient { get; }
    }
    public class ClientAccessor : IClientAccessor, IDisposable
    {
        private readonly ConnectionOptions connectionOptions;
        private readonly ScadaClientPool clientPool;
        private ScadaClient? scadaClient;
        public ClientAccessor(IServiceConfiguration serviceConfiguration)
        {
            this.connectionOptions = new ConnectionOptions()
            {
                Name = serviceConfiguration.ServerName,
                Host = serviceConfiguration.Host,
                Port = serviceConfiguration.Port,
                Username = serviceConfiguration.Username,
                Password = ScadaUtils.Decrypt(serviceConfiguration.Password),
                Timeout = serviceConfiguration.Timeout,
                SecretKey = ScadaUtils.HexToBytes(serviceConfiguration.SecretKey)
            };

            this.clientPool = new ScadaClientPool();
            this.scadaClient = null;
        }
        public ScadaClient ScadaClient
        {
            get
            {
                scadaClient ??= this.clientPool.GetClient(this.connectionOptions);
                return scadaClient;
            }
        }

        public void Dispose()
        {
            this.clientPool.ReturnClient(scadaClient);
        }
    }
}
