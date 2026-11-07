// SPDX-License-Identifier: GPL-3.0-only
using System.IO;
using System.Text.Json;

namespace HMI.Common.Configuration;

/// <summary>
/// 在当前用户的应用程序数据目录中持久化 HMI 设置。
/// </summary>
public sealed class HmiSettingsStore
{
    private readonly object syncRoot = new();
    private readonly string filePath;
    private Dictionary<string, JsonElement>? settings;

    public HmiSettingsStore()
        : this(GetDefaultFilePath())
    {
    }

    public HmiSettingsStore(string filePath)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(filePath);
        this.filePath = Path.GetFullPath(filePath);
    }

    public string? GetString(string key, string? defaultValue = null)
    {
        lock (syncRoot)
        {
            return GetValue(key)?.Deserialize<string>() ?? defaultValue;
        }
    }

    public void SetString(string key, string? value)
    {
        lock (syncRoot)
        {
            EnsureLoaded();
            settings![key] = JsonSerializer.SerializeToElement(value);
            Save();
        }
    }

    public IReadOnlyList<string> GetStringList(string key)
    {
        lock (syncRoot)
        {
            return GetValue(key)?.Deserialize<List<string>>() ?? [];
        }
    }

    public void SetStringList(string key, IEnumerable<string> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        lock (syncRoot)
        {
            EnsureLoaded();
            settings![key] = JsonSerializer.SerializeToElement(values.ToArray());
            Save();
        }
    }

    private JsonElement? GetValue(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key);
        EnsureLoaded();
        return settings!.TryGetValue(key, out JsonElement value) ? value : null;
    }

    private void EnsureLoaded()
    {
        if (settings != null)
        {
            return;
        }

        settings = File.Exists(filePath)
            ? JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(File.ReadAllText(filePath))
                ?? new Dictionary<string, JsonElement>(StringComparer.Ordinal)
            : new Dictionary<string, JsonElement>(StringComparer.Ordinal);
    }

    private void Save()
    {
        string? directory = Path.GetDirectoryName(filePath);
        if (string.IsNullOrEmpty(directory))
        {
            throw new InvalidOperationException($"无法确定设置文件目录：{filePath}");
        }

        Directory.CreateDirectory(directory);
        string temporaryPath = $"{filePath}.{Guid.NewGuid():N}.tmp";
        File.WriteAllText(temporaryPath, JsonSerializer.Serialize(settings, new JsonSerializerOptions
        {
            WriteIndented = true
        }));
        File.Move(temporaryPath, filePath, overwrite: true);
    }

    private static string GetDefaultFilePath()
    {
        string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        if (string.IsNullOrWhiteSpace(appDataPath))
        {
            throw new InvalidOperationException("无法确定当前用户的应用程序数据目录。");
        }

        return Path.Combine(appDataPath, "HMI", "SmartDesigner", "settings.json");
    }
}
