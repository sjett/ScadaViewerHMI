// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Models;
using Scada;
using Scada.Data.Const;
using Scada.Data.Entities;
using Scada.Data.Models;
using Scada.Data.Tables;
using Scada.Lang;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HMI.Common.Browsers.Services
{
    public interface ISignalsService
    {
        IConnectableObservable<SignalDefinition> GetSignalDefinitions(int languageId, bool invalidateCache);
    }
    public class CachedSignalsService: ISignalsService
    {
        private readonly IClientAccessor clientAccessor;
        public CachedSignalsService(IClientAccessor clientAccessor)
        {
            this.clientAccessor = clientAccessor;
        }

        public IConnectableObservable<SignalDefinition> GetSignalDefinitions(int languageId, bool invalidateCache)
        {

            try
            {
                this.clientAccessor.ScadaClient.GetStatus(out bool serverIsReady, out bool userIsLoggedIn);

                if (serverIsReady)
                {
                    string tableTitle = CommonPhrases.UndefinedTable;
                    var configDatabase = new ConfigDataset();
                    //下载全部表
                    foreach (IBaseTable baseTable in configDatabase.AllTables)
                    {
                        tableTitle = baseTable.Title;
                        this.clientAccessor.ScadaClient.DownloadBaseTable(baseTable);
                    }

                    tableTitle = CommonPhrases.UndefinedTable;
                    PostprocessConfigDatabase(configDatabase);

                    //Log.WriteAction(Locale.IsRussian ?
                    //    "База конфигурации получена успешно" :
                    //    "The configuration database has been received successfully");
                    //return true;
                    List<SignalDefinition> result = new List<SignalDefinition>();
                    foreach (var cnl in configDatabase.CnlTable)
                    {
                        result.Add( new SignalDefinition()
                        {
                            Name = cnl.Name,
                            AliasName = cnl.Name,
                            //Description = cnl.Description,
                            //IsArchived = cnl.IsArchivable(),
                            //DataType = cnl.GetDataType(),
                            //Server = serviceConfiguration.ServerName,
                            //Connector = tableTitle
                            //Logs =  new[] { new SignalLog() { Active = cnl.Active } } ,
                            Server = new Server() { Name = configDatabase.ObjTable.GetItem(cnl.ObjNum.Value).Name },
                            Connector = new Connector() { 
                                Name = configDatabase.CommLineTable.GetItem(configDatabase.DeviceTable.GetItem(cnl.DeviceNum.Value).CommLineNum.Value).Name,
                                Description = configDatabase.CommLineTable.GetItem(configDatabase.DeviceTable.GetItem(cnl.DeviceNum.Value).CommLineNum.Value).Name
                            },
                            Group = new SignalGroup() { Name = configDatabase.DeviceTable.GetItem(cnl.DeviceNum.Value).Name },
                            WriteGroup = new WriteGroup() { Name = configDatabase.DeviceTable.GetItem(cnl.DeviceNum.Value).Name },

                            Active = cnl.Active,
                            Unit = cnl.UnitID.HasValue ? configDatabase.UnitTable.GetItem(cnl.UnitID.Value).Name : ""
                        });
                    }

                   return result.ToObservable().Publish();
                }
                else
                {
                    throw new Exception("Server is not ready");
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error checking server connection", ex);
            }
            finally
            {
                this.clientAccessor.Dispose();
            }

            return Enumerable.Empty<SignalDefinition>().ToObservable().Publish();
        }
        private void PostprocessConfigDatabase(ConfigDataset configDatabase)
        {
            // duplicate channels for arrays and strings
            List<Cnl> duplicatedCnls = [];

            foreach (Cnl cnl in configDatabase.CnlTable)
            {
                if (cnl.IsArray() && cnl.IsArchivable())
                {
                    string name = cnl.Name;
                    int dataLen = cnl.DataLen.Value;
                    int cnlTypeID = CnlTypeID.UnsetOutput(cnl.CnlTypeID);

                    if (cnl.IsNumeric())
                    {
                        cnl.Name += "[0]";
                        cnl.DataLen = null;
                    }

                    for (int i = 1; i < dataLen; i++)
                    {
                        Cnl newCnl = cnl.ShallowCopy();
                        newCnl.CnlNum = cnl.CnlNum + i;
                        newCnl.Name = name + "[" + i + "]";
                        newCnl.DataLen = null;
                        newCnl.CnlTypeID = cnlTypeID;
                        duplicatedCnls.Add(newCnl);
                    }
                }
            }

            duplicatedCnls.ForEach(configDatabase.CnlTable.AddItem);

            // initialize data objects, 初始化枚举
            //configDatabase.Init();
        }
    }
}
