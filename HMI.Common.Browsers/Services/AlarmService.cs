// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Models;
using Scada;
using Scada.Data.Models;
using System.IO;
using System.Reactive.Linq;
using System.Xml;

namespace HMI.Common.Browsers.Services
{
	public class AlarmService : IAlarmService
	{
        private readonly IClientAccessor clientAccessor;
        public AlarmService(IClientAccessor clientAccessor) 
		{
            this.clientAccessor = clientAccessor;
        }
		public IObservable<TranslatedEntity> GetAlarmGroups()
		{
            try
            {
                this.clientAccessor.ScadaClient.GetStatus(out bool serverIsReady, out bool userIsLoggedIn);

                if (!serverIsReady)
                    throw new Exception("Server is not ready");

                var configDatabase = new ConfigDataset();
                this.clientAccessor.ScadaClient.DownloadBaseTable(configDatabase.AlarmGroupTable);

                var result = new List<TranslatedEntity>();
                foreach (var alarmGroup in configDatabase.AlarmGroupTable)
                    result.Add(new TranslatedEntity() { Name = alarmGroup.Name });

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
		public IObservable<TranslatedEntity> GetAlarmTypes()
		{
            try
            {
                this.clientAccessor.ScadaClient.GetStatus(out bool serverIsReady, out bool userIsLoggedIn);

                if (!serverIsReady)
                    throw new Exception("Server is not ready");

                var configDatabase = new ConfigDataset();
                this.clientAccessor.ScadaClient.DownloadBaseTable(configDatabase.AlarmTypeTable);

                var result = new List<TranslatedEntity>();
                foreach (var alarmType in configDatabase.AlarmTypeTable)
                    result.Add(new TranslatedEntity() { Name = alarmType.Name });

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
		public IObservable<ExtendedAlarmProperty> GetExtendedAlarmProperties()
		{
            return Observable.Empty<ExtendedAlarmProperty>();
        }
		
		
	}
}
