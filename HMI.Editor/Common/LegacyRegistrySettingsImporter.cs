// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Configuration;
using HMI.Common.Cryptography;
using Microsoft.Win32;

namespace HMI.Editor.Common;

/// <summary>
/// 一次性将旧版 Windows 注册表设置导入跨平台 JSON 设置文件。
/// </summary>
internal sealed class LegacyRegistrySettingsImporter
{
    private const string ImportCompletedKey = "Migration.LegacyRegistryImported";
    private const string EditorSettingsPath = @"Software\HMI\Editor\2.00\Settings";
    private const string HmiSettingsPath = @"Software\HMI\2.00\Settings";
    private const string CommonSettingsPath = @"Software\HMI\Common\2.00\Settings";
    private const string DataSourceSettingsPath = @"SOFTWARE\ScadaViewer\Server\2.00\Settings";
    private const int MaximumRecentProjects = 10;

    private readonly HmiSettingsStore settingsStore;

    public LegacyRegistrySettingsImporter(HmiSettingsStore settingsStore)
    {
        this.settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
    }

    public void ImportOnce()
    {
        if (settingsStore.GetString(ImportCompletedKey) == "true")
        {
            return;
        }

        if (!OperatingSystem.IsWindows())
        {
            return;
        }

        using RegistryKey? editorSettings = Registry.CurrentUser.OpenSubKey(EditorSettingsPath);
        using RegistryKey? hmiSettings = Registry.LocalMachine.OpenSubKey(HmiSettingsPath);
        using RegistryKey? commonSettings = Registry.LocalMachine.OpenSubKey(CommonSettingsPath);
        using RegistryKey? dataSourceSettings = Registry.LocalMachine.OpenSubKey(DataSourceSettingsPath);

        if (editorSettings != null)
        {
            ImportString(editorSettings, "LastProjectDirectory", "Editor.LastProjectDirectory");
            ImportString(editorSettings, "Language", "Editor.Language");

            if (settingsStore.GetStringList("Editor.RecentProjects").Count == 0)
            {
                var recentProjects = new List<string>();
                for (int index = 0; index < MaximumRecentProjects; index++)
                {
                    string? projectPath = editorSettings.GetValue($"MRU{index}")?.ToString();
                    if (string.IsNullOrWhiteSpace(projectPath))
                    {
                        break;
                    }

                    recentProjects.Add(projectPath);
                }

                settingsStore.SetStringList("Editor.RecentProjects", recentProjects);
            }
        }

        ImportString(hmiSettings, "Silverlight4ControlsDirectory", "Build.Silverlight4ControlsDirectory");
        ImportString(commonSettings, "InstallationPath", "Build.InstallationPath");

        string? encodedConnectionString = dataSourceSettings?.GetValue("DSN")?.ToString();
        if (string.IsNullOrWhiteSpace(settingsStore.GetString("ConnectionStrings.DataEntities")) &&
            !string.IsNullOrWhiteSpace(encodedConnectionString))
        {
            settingsStore.SetString(
                "ConnectionStrings.DataEntities",
                new LegacyConnectionStringCodec().Decode(encodedConnectionString));
        }

        settingsStore.SetString(ImportCompletedKey, "true");
    }

    private void ImportString(RegistryKey? registryKey, string valueName, string settingsKey)
    {
        string? value = registryKey?.GetValue(valueName)?.ToString();
        if (string.IsNullOrWhiteSpace(settingsStore.GetString(settingsKey)) &&
            !string.IsNullOrWhiteSpace(value))
        {
            settingsStore.SetString(settingsKey, value);
        }
    }
}
