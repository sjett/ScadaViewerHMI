// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.Models;
using Scada;
using Scada.Data.Models;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace HMI.Common.Browsers.Services
{
    public enum IncludedSymbolicTexts
    {
        SymbolicTexts = 1,
        InternallyUsed = 2,
        AlarmGroups = 3,
        AlarmTypes = 4,
        Alarms = 5,
        AlarmHelpTexts = 6,
        ExtendedAlarmProperties = 7,
        Signals = 8,
        SignalGroups = 9,
        Logs = 10,
        DiscreteValues = 11,
        All = 12
    }

    public interface ISymbolicTextService
    {
        IObservable<int> GetAvailableLanguageIds();
        IConnectableObservable<SymbolicText> GetSymbolicTexts(int[] languages, IncludedSymbolicTexts includedSymbolicTexts, bool invalidateCache);
    }


    public class CachedSymbolicTextService : ISymbolicTextService
    {
        private readonly IClientAccessor clientAccessor;
        public CachedSymbolicTextService(IClientAccessor clientAccessor)
        {
            this.clientAccessor = clientAccessor;
        }
        public IObservable<int> GetAvailableLanguageIds()
        {
            try
            {
                this.clientAccessor.ScadaClient.GetStatus(out bool serverIsReady, out bool userIsLoggedIn);

                if (!serverIsReady)
                    throw new Exception("Server is not ready");

                var configDatabase = new ConfigDataset();
                this.clientAccessor.ScadaClient.DownloadBaseTable(configDatabase.LanguageTable);

                var result = new List<int>();
                foreach (var lang in configDatabase.LanguageTable)
                    result.Add(lang.LanguageID);

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
        public IConnectableObservable<SymbolicText> GetSymbolicTexts(
            int[] languages,
            IncludedSymbolicTexts included,
            bool invalidateCache)
        {

            try
            {
                this.clientAccessor.ScadaClient.GetStatus(out bool serverIsReady, out bool userIsLoggedIn);

                if (!serverIsReady)
                    throw new Exception("Server is not ready");

                var configDatabase = new ConfigDataset();

                // 下载所有相关表
                this.clientAccessor.ScadaClient.DownloadBaseTable(configDatabase.TranslationTable);

                // 根据 included 参数过滤
                List<SymbolicText> list = new List<SymbolicText>();
                foreach (var row in configDatabase.TranslationTable)
                {
                    if (!languages.Contains(row.LanguageID))
                        continue;

                    if (row.TranslationTypeID > (int)included)
                        continue;

                    var symbolic = list.FirstOrDefault(t => t.SymbolicTextName == row.Code);
                    if (symbolic == null)
                    {
                        symbolic = new SymbolicText
                        {
                            SymbolicTextName = row.Code
                        };
                        list.Add(symbolic);
                    }

                    var translation = symbolic.Translations.FirstOrDefault(t => t.LanguageID == row.LanguageID);
                    if (translation != null)
                    {
                        translation.TranslationText = row.Text;
                    }
                    else
                    {
                        symbolic.Translations.Add(new SymbolicTextTranslation
                        {
                            LanguageID = row.LanguageID,
                            TranslationText = row.Text
                        });
                    }
                }

                return list.ToObservable().Publish();
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
