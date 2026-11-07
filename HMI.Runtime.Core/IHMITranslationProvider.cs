// SPDX-License-Identifier: GPL-3.0-only
using HMI.Runtime.Communication;
using HMI.Runtime.Core.Helper;
using HMI.Runtime.Core.Helper.Extensions;
using HMI.Runtime.Core.Helper.Filter;
using HMI.Runtime.Core.Helper.Translations;
using System;
using System.Globalization;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Transactions;

namespace HMI.Runtime.Core;

public delegate void OnLanguageChangedDelegate(string[] symbolicNames, string[] translationTexts);

public interface IHMITranslationProvider : IDisposable
{
    HMILanguage[] ReadLanguages();
    void RegisterLanguageChangedHandler(string[] symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate);
    void UnregisterLanguageChangedHandler(string[] symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate);
    int GetLanguageId();
    void SetLanguageId(int languageId);
}


/// <summary>

/// 


/// 


/// 

/// 

/// </summary>
public class HMITranslationProvider : IHMITranslationProvider
{
    private const string LANGUAGE_ID_NAME = "HMI_LANGUAGE_ID";

    private IHmiApiClient hmiApiClient;

    private HMILanguage[] languages = new HMILanguage[0];

    private IFilterBuilder _filterBuilder;




    private int languageId = -1;
    private readonly object lockObject = new object();
    private bool translationLoaded;
    private bool useMatch;

    public IList<ITranslationModel> Translations
    {
        get;
        set;
    }
    public HMITranslationProvider(IHmiApiClient apiClient, IFilterBuilder filterBuilder = null)
    {
        this.hmiApiClient = apiClient;

        this.initTranslations();

        this.ensureFilterBuilder(filterBuilder);



        _ = this.StartGetAllLanguages();

        var userData = Preferences.GetPreferences(this);
        this.languageId = this.tryGetLanguageId((string?)userData?[LANGUAGE_ID_NAME] ?? "");
    }
    private void loadAllLanguagesData(IEnumerable<HMILanguage> languages)
    {
        lock (this.lockObject)
        {
            this.saveLanguages(languages);
        }
    }
    private void loadAllTranslationsData(IEnumerable<KeyValuePair<string, string>> translations, int languageId)
    {
        lock (this.lockObject)
        {
            this.loadAllTranslations(translations, languageId);
        }
    }
    public HMILanguage[] ReadLanguages()
    {
        return this.languages;
    }
    public void RegisterLanguageChangedHandler(string[] symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        if (!this.checkRegisterLanguageChangedHandler(symbolicNames, onLanguageChangedDelegate))
        {
            return;
        }
        lock (this.lockObject)
        {
            this.addLanguageChangedHandler(symbolicNames, onLanguageChangedDelegate);
        }
    }
    public void UnregisterLanguageChangedHandler(string[] symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        if (!this.checkUnregisterLanguageChangedHandler(symbolicNames, onLanguageChangedDelegate))
        {
            return;
        }
        lock (this.lockObject)
        {
            this.removeLanguageChangedHandler(symbolicNames, onLanguageChangedDelegate);
        }
    }
    public int GetLanguageId()
    {
        int result;
        lock (this.lockObject)
        {
            result = this.languageId;
        }
        return result;
    }
    public void SetLanguageId(int languageId)
    {
        lock (this.lockObject)
        {
            this.setLanguage(languageId);
        }
    }

    private void ensureFilterBuilder(IFilterBuilder builder)
    {
        this._filterBuilder = builder ?? new FilterBuilder();
    }
    private void addLanguageChangedHandler(IEnumerable<string> symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        this.addLanguageChangedDelegate(symbolicNames, onLanguageChangedDelegate);

        if (this.useMatch)
        {
            this.addMatchTranslationModels();
        }
        
        if (this.hasCurrentLanguageTranslationModels())
        {
            this.invorkeLanguageChangedDelegate(onLanguageChangedDelegate);
            return;
        }

        _ = this.getAllSymbolicTextTranslations();
    }
    private void setLanguage(int languageId)
    {
        if (this.languageId == languageId)
        {
            return;
        }
        this.languageId = languageId;

        var userData = new JsonObject
        {
            { LANGUAGE_ID_NAME, this.languageId.ToString() }
        };
        Preferences.SetPreferences(this, userData);

        this.setCurrentCulture();

        if (this.hasCurrentLanguageTranslationModels())
        {
            this.invorkeAllLanguageChangedDelegate();
            return;
        }

        _ = this.getAllSymbolicTextTranslations();
    }
    private void removeLanguageChangedHandler(IEnumerable<string> symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        if (symbolicNames == null)
        {
            this.removeLanguageChangedDelegate(onLanguageChangedDelegate);
            return;
        }
        this.removeLanguageChangedDelegate(symbolicNames, onLanguageChangedDelegate);
    }
    private void addMatchTranslationModels()
    {
        IEnumerable<ITranslationModel> translationModels = this.findUseMatchTranslationModels();
        foreach (var translationModel in translationModels)
        {
            var filter = this._filterBuilder.CreateFilter(translationModel.SymbolicText);
            if (filter != null)
            {

                IEnumerable<ITranslationModel> translations = this.Translations.Where(new Func<ITranslationModel, bool>(filter));

                this.addAllLanguageChangedDelegate(translations, translationModel.Delegates.Keys);

                this.removeTranslationModelBySymbolicText(translationModel.SymbolicText); 
            }
        }
    }
    private IEnumerable<ITranslationModel> findUseMatchTranslationModels()
    {
        return this.Translations.Where(t => !string.IsNullOrEmpty(t.SymbolicText) && t.SymbolicText.Contains("*")).ToList();
    }
    private void loadAllTranslations(IEnumerable<KeyValuePair<string, string>> translations, int languageId)
    {
        this.translationLoaded = false;
        if (translations == null)
        {
            return;
        }

        this.useMatch = true;
        this.addSymbolicTextTranslation(translations, languageId);

        this.addDefaultLanguageTranslation(languageId);

        this.addMatchTranslationModels();

        this.invorkeAllLanguageChangedDelegate();
    }
    private void saveLanguages(IEnumerable<HMILanguage> languages)
    {
        this.languages = Enumerable.ToArray<HMILanguage>(languages);
        int languageId = this.getLanguageId();
        this.SetLanguageId(languageId);
    }
    private int getLanguageId()
    {
        if (this.languageId == -1 && this.getDefaultLanguageId())
        {
            return this.languages.FirstOrDefault(t => t.IsDefault).LanguageId;
        }
        return this.languageId;
    }
    private bool getDefaultLanguageId()
    {
        if (this.languages != null)
        {
            return this.languages.Any(t => t.IsDefault);
        }
        return false;
    }
    private void initTranslations()
    {
        lock (this.lockObject)
        {
            this.Translations = new List<ITranslationModel>();
        }
    }
    private async Task StartGetAllLanguages()
    {
        if (DesignerPropertiesExt.IsInDesignTool)
        {
            return;
        }

        var languageDto = await this.hmiApiClient.GetAllLanguages();

        this.loadAllLanguagesData(languageDto.Select(dto => new HMILanguage
        {
            LanguageId = dto.LanguageId,
            Description = dto.Description,
            IsActive = dto.IsActive,
            IsDefault = dto.IsDefault
        }));
    }
    private int tryGetLanguageId(string newCookieValue)
    {
        int num = 0;
        int.TryParse(newCookieValue, out num);
        if (num == 0)
        {
            return this.languageId;
        }
        return num;
    }
    private async Task getAllSymbolicTextTranslations()
    {
        if (this.languageId == -1 || this.translationLoaded)
        {
            return;
        }
        this.translationLoaded = true;
        if (!DesignerPropertiesExt.IsInDesignTool)
        {
            var translations = await this.hmiApiClient.GetSymbolicTextTranslations(this.languageId);
            loadAllTranslationsData(translations, this.languageId);
            return;
        }
        this.setDesignerTimeTranslationsData(this.languageId);
    }
    private void setDesignerTimeTranslationsData(int languageId)
    {
        this.loadAllTranslationsData(new List<KeyValuePair<string, string>>(), languageId);
    }
    private void addSymbolicTextTranslation(IEnumerable<KeyValuePair<string, string>> translations, int languageId)
    {
        foreach (var translationEntry in translations)
        {
            ITranslationModel translation = this.Translations.FirstOrDefault(t => t.SymbolicText == translationEntry.Key);
            this.addSymbolicTextTranslation(translation, translationEntry, languageId);
        }
    }
    private void addSymbolicTextTranslation(ITranslationModel translation, KeyValuePair<string, string> newSymbolicTextTranslation, int languageId)
    {
        ITranslationModel translationModel = translation;
        if (translationModel == null)
        {
            translationModel = new TranslationModel();
            this.Translations.Add(translationModel);
        }
        this.addTranslationModel(translationModel, languageId);
        translationModel.SymbolicText = newSymbolicTextTranslation.Key;
        translationModel.Translations[languageId] = newSymbolicTextTranslation.Value;
    }


    private void addDefaultLanguageTranslation(int languageId)
    {
        IEnumerable<ITranslationModel> enumerable = this.Translations.Where(t => !t.Translations.ContainsKey(languageId));
        foreach (var current in enumerable)
        {
            current.Translations.Add(languageId, current.SymbolicText);
        }
    }
    private bool hasCurrentLanguageTranslationModels()
    {
        return this.Translations.Count != 0 && this.Translations.Any(t => t.Translations.ContainsKey(this.languageId));
    }
    private void invorkeAllLanguageChangedDelegate()
    {
        IEnumerable<OnLanguageChangedDelegate> delegates = this.getAllLanguageChangedDelegate();
        foreach (var current in delegates)
        {
            this.invorkeLanguageChangedDelegate(current);
        }
    }
    private void invorkeLanguageChangedDelegate(OnLanguageChangedDelegate @delegate)
    {
        if (@delegate == null)
        {
            return;
        }
        ITranslationModel[] translations = this.Translations.Where(t => t.Delegates.ContainsKey(@delegate)).ToArray();
        this.invorkeLanguageChangedDelegate(@delegate, translations);
    }
    private void invorkeLanguageChangedDelegate(OnLanguageChangedDelegate @delegate, IEnumerable<ITranslationModel> translations)
    {
        if (@delegate == null)
        {
            return;
        }
        List<int> order = this.findDelegateOrder(@delegate, translations).ToList();
        string[] symbolicNames = this.reorderList(translations.Select(t => t.SymbolicText), order).ToArray();
        string[] translationTexts = this.reorderList(translations.Select(t => t.Translations[this.languageId]), order).ToArray();
        @delegate(symbolicNames, translationTexts);
    }
    private IEnumerable<string> reorderList(IEnumerable<string> elements, IEnumerable<int> order)
    {
        IEnumerable<int> enumerable = this.orderList(order);
        string[] array = new string[elements.Count()];
        for (int i = 0; i < elements.Count(); i++)
        {
            string text = elements.ElementAt(i);
            int num = enumerable.ElementAtOrDefault(i);
            array[num] = text;
        }
        return array;
    }
    private IEnumerable<int> orderList(IEnumerable<int> order)
    {
        IEnumerable<KeyValuePair<int, int>> arg_40_0 = order.Select((value, i) => new KeyValuePair<int, int>(i, value));

        IOrderedEnumerable<KeyValuePair<int, int>> orderedEnumerable = arg_40_0.OrderBy(x => x.Value);
        int num = 0;
        int[] array = new int[orderedEnumerable.Count()];
        foreach (KeyValuePair<int, int> current in orderedEnumerable)
        {
            array[current.Key] = num++;
        }
        return array;
    }
    private IEnumerable<int> findDelegateOrder(OnLanguageChangedDelegate @delegate, IEnumerable<ITranslationModel> translations)
    {
        return translations.Select(t => t.Delegates.First(kv => kv.Key == @delegate).Value);
    }
    private IEnumerable<OnLanguageChangedDelegate> getAllLanguageChangedDelegate()
    {
        var delegates = new List<OnLanguageChangedDelegate>();
        foreach (var current in this.Translations)
        {
            delegates.AddRange(current.Delegates.Where(p => !delegates.Contains(p.Key)).Select(p => p.Key));
        }
        return delegates;
    }
    private void addLanguageChangedDelegate(IEnumerable<string> symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        int i = 0;
        foreach (var symbolicName in symbolicNames)
        {
            if (!string.IsNullOrWhiteSpace(symbolicName))
            {
                ITranslationModel translation = this.getOrCreateTranslationModel(symbolicName);

                this.addLanguageChangedDelegate(translation, onLanguageChangedDelegate, i);

                if (this.hasCurrentLanguageTranslationModels())
                {
                    this.addTranslationModel(translation, this.languageId);
                }
            }
            i++;
        }
    }
    private void addTranslationModel(ITranslationModel translation, int languageId)
    {
        if (!translation.Translations.ContainsKey(languageId))
        {
            translation.Translations.Add(languageId, translation.SymbolicText);
        }
    }
    private void addLanguageChangedDelegate(ITranslationModel translation, OnLanguageChangedDelegate onLanguageChangedDelegate, int orderIndex)
    {
        if (!translation.Delegates.ContainsKey(onLanguageChangedDelegate))
        {
            translation.Delegates.Add(onLanguageChangedDelegate, orderIndex);
        }
    }
    private void addLanguageChangedDelegate(IEnumerable<ITranslationModel> translations, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        for (int i = 0; i < translations.Count(); i++)
        {
            ITranslationModel translation = translations.ElementAt(i);
            this.addLanguageChangedDelegate(translation, onLanguageChangedDelegate, i);
        }
    }
    private void addAllLanguageChangedDelegate(IEnumerable<ITranslationModel> translations, IEnumerable<OnLanguageChangedDelegate> onLanguageChangedDelegates)
    {
        foreach (var current in onLanguageChangedDelegates)
        {
            this.addLanguageChangedDelegate(translations, current);
        }
    }
    private void removeLanguageChangedDelegate(IEnumerable<string> symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        foreach (var symbolicName in symbolicNames)
        {
            IEnumerable<ITranslationModel> translations = this.getTranslationModels(symbolicName);
            this.removeLanguageChangedDelegate(translations, onLanguageChangedDelegate);
        }
    }
    private void removeLanguageChangedDelegate(OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        IEnumerable<ITranslationModel> translations = this.Translations.Where(t => t.Delegates.ContainsKey(onLanguageChangedDelegate));
        this.removeLanguageChangedDelegate(translations, onLanguageChangedDelegate);
    }

    private void removeLanguageChangedDelegate(IEnumerable<ITranslationModel> translations, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        foreach (var translation in translations)
        {
            if (translation != null && translation.Delegates.ContainsKey(onLanguageChangedDelegate))
            {
                translation.Delegates.Remove(onLanguageChangedDelegate);
            }
        }
    }

    private void removeTranslationModelBySymbolicText(string symbolicText)
    {
        ITranslationModel[] array = this.Translations.Where(t => t.SymbolicText == symbolicText).ToArray();

        for (int i = 0; i < array.Length; i++)
        {
            ITranslationModel translationModel = array[i];
            this.Translations.Remove(translationModel);
        }
    }
    private ITranslationModel getOrCreateTranslationModel(string symbolicName)
    {
        ITranslationModel? translationModel = this.getTranslationModel(symbolicName);
        if (translationModel == null)
        {
            translationModel = new TranslationModel
            {
                SymbolicText = symbolicName
            };
            this.Translations.Add(translationModel);
        }
        return translationModel;
    }
    private ITranslationModel? getTranslationModel(string symbolicName)
    {
        return this.Translations.FirstOrDefault(t => t.SymbolicText == symbolicName);
    }
    private IEnumerable<ITranslationModel> getTranslationModels(string symbolicName)
    {
        var filter = this._filterBuilder.CreateFilter(symbolicName);
        if (filter == null)
        {
            return Enumerable.Empty<ITranslationModel>();
        }
        return this.Translations.Where(t => filter(t.SymbolicText));
    }
    private bool checkRegisterLanguageChangedHandler(string[] symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        return symbolicNames != null && symbolicNames.Length > 0 && onLanguageChangedDelegate != null;
    }
    private bool checkUnregisterLanguageChangedHandler(string[] symbolicNames, OnLanguageChangedDelegate onLanguageChangedDelegate)
    {
        return onLanguageChangedDelegate != null && (symbolicNames == null || symbolicNames.Length > 0);
    }
    private void setCurrentCulture()
    {
        CultureInfo cultureInfo = CultureInfoCache.GetCultureInfo(this.languageId);
        Thread.CurrentThread.CurrentCulture = (cultureInfo);
        Thread.CurrentThread.CurrentUICulture = (cultureInfo);
    }
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }
    private void Dispose(bool disposing)
    {
        if (disposing)
        {
            this.DisposeManagedResources();
        }
    }
    private void DisposeManagedResources()
    {
        if (this.hmiApiClient != null)
        {
          //this.hmiConnector.DisposeAsync();
        }
        if (this.Translations != null)
        {
            foreach (var model in this.Translations)
            {
                model.Dispose();
            }
            this.Translations.Clear();
            this.Translations = null;
        }

    }


}
