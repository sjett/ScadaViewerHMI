// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Configuration;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;

namespace HMI.Infrastructure.Services;

/// <summary>管理应用程序语言偏好。</summary>
public sealed class LanguageService : ILanguageService
{
    private const string LanguageSettingsKey = "Editor.Language";

    private readonly HmiSettingsStore settingsStore;

    public LanguageService(HmiSettingsStore settingsStore)
    {
        Contract.Requires<ArgumentNullException>(settingsStore != null, nameof(settingsStore));
        this.settingsStore = settingsStore;

        if (string.IsNullOrWhiteSpace(GetLanguage()))
        {
            ChangeLanguage(CultureInfo.CurrentCulture.Name);
        }
    }

    public void SetApplicationLanguage()
    {
        CultureInfo culture = new(GetLanguage());
        Thread.CurrentThread.CurrentCulture = culture;
        Thread.CurrentThread.CurrentUICulture = culture;
    }

    public void ChangeLanguage(string language)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(language);
        _ = CultureInfo.GetCultureInfo(language);
        settingsStore.SetString(LanguageSettingsKey, language);
    }

    public string GetLanguage()
    {
        return settingsStore.GetString(LanguageSettingsKey) ?? CultureInfo.CurrentCulture.Name;
    }
}
