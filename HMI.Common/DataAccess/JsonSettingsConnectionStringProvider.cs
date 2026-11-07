// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Configuration;

namespace HMI.Common.DataAccess;

/// <summary>从跨平台 JSON 用户设置中读取数据库连接字符串。</summary>
public sealed class JsonSettingsConnectionStringProvider : IConnectionStringProvider
{
    private readonly HmiSettingsStore settingsStore;

    public JsonSettingsConnectionStringProvider(HmiSettingsStore settingsStore)
    {
        this.settingsStore = settingsStore ?? throw new ArgumentNullException(nameof(settingsStore));
    }

    public string GetConnectionString()
    {
        return settingsStore.GetString("ConnectionStrings.DataEntities")
            ?? throw new InvalidOperationException("未配置 DataEntities 数据连接字符串。");
    }
}
