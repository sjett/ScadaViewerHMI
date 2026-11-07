// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Configuration;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Domain.Models;

namespace HMI.Infrastructure.Services;

/// <summary>使用用户 JSON 设置保存最近打开的项目列表。</summary>
public sealed class MruService : IMruService
{
    private const string RecentProjectsSettingsKey = "Editor.RecentProjects";
    public const int MaxMruItems = 10;

    private readonly HmiSettingsStore settingsStore;
    private readonly IFileSystemService fileSystemService;

    public MruService(HmiSettingsStore settingsStore, IFileSystemService fileSystemService)
    {
        Contract.Requires<ArgumentNullException>(settingsStore != null, nameof(settingsStore));
        Contract.Requires<ArgumentNullException>(fileSystemService != null, nameof(fileSystemService));
        this.settingsStore = settingsStore;
        this.fileSystemService = fileSystemService;
    }

    public void PrependProjectToMRU(string projectFile)
    {
        Contract.Requires<ArgumentNullException>(projectFile != null, nameof(projectFile));
        Contract.Requires<ArgumentException>(projectFile.Length > 0, nameof(projectFile));

        List<string> recentProjects = settingsStore.GetStringList(RecentProjectsSettingsKey).ToList();
        recentProjects.RemoveAll(path => string.Equals(path, projectFile, StringComparison.OrdinalIgnoreCase));
        recentProjects.Insert(0, projectFile);
        recentProjects = recentProjects.Take(MaxMruItems).ToList();
        settingsStore.SetStringList(RecentProjectsSettingsKey, recentProjects);
    }

    public IEnumerable<MruEntry> GetMruItems()
    {
        return settingsStore.GetStringList(RecentProjectsSettingsKey)
            .Where(fileSystemService.ExistsFile)
            .Select(path => new MruEntry(path))
            .ToList();
    }
}
