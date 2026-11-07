// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Communication;

public interface IHmiApiClient: IAsyncDisposable
{
    event Action<SignalValue>? SignalReceived;
    event Action<string, Exception, int>? CommunicationError;
    event Action<string, Exception, int, int[], string[]>? RegisterError;

    // --- 认证 / 会话 ---

    /// <summary>
    /// 使用已有 Token 进行认证（通常在前端已有 Token 时使用）。
    /// </summary>
    Task AuthenticateAsync(string token, CancellationToken ct = default);

    event Action<UserInfo>? OnUserContextChanged;

    /// <summary>
    /// 登录安全用户（对应原 WF 安全登录），返回用户信息。
    /// </summary>
    Task<UserInfo> LoginAsync(string user, string password,  CancellationToken ct = default);

    Task LogoutAsync(CancellationToken ct = default);

    Task<object?> ChangePasswordAsUserAsync(string username, string oldPassword, string newPassword, CancellationToken ct = default);

    Task<object?> ChangePasswordAsAdminAsync(string username, string newPassword, CancellationToken ct = default);

    Task<int> GetSecurtityStatusAsync(CancellationToken ct = default);

    // --- 信号读写 ---

    Task<object?> ReadSignalAsync(string signal, CancellationToken ct = default);

    Task<object?> WriteSignalAsync(string signal, object value, WriteOptions? options = null, CancellationToken ct = default);

    Task SubscribeSignalsAsync(
        IEnumerable<string> signals,
        CancellationToken ct = default);
    Task UnsubscribeSignalsAsync(
        IEnumerable<string> signals,
        CancellationToken ct = default);

    // --- 报警 ---

    Task<IEnumerable<Alarm>> GetActiveAlarmsAsync(CancellationToken ct = default);

    Task<object?> AcknowledgeAlarmsAsync(
        IEnumerable<Guid> alarmIds,
        string? comment = null,
        WriteOptions? options = null,
        CancellationToken ct = default);
    Task<object?> AcknowledgeAlarmsByGroupAsync(
        string groupName,
        string? comment = null,
        WriteOptions? options = null,
        CancellationToken ct = default);

    Task<object?> AcknowledgeAllAlarmsAsync(
        WriteOptions? options = null,
        CancellationToken ct = default);

    Task<object?> AcknowledgeAllAlarmsWithCommentAsync(
        string? comment = null,
        WriteOptions? options = null,
        CancellationToken ct = default);

    Task<object?> AcknowledgeAllGoneAlarmsAsync(
        string? comment = null,
        WriteOptions? options = null,
        CancellationToken ct = default);

    Task<object?> SetAlarmStateAsync(
        IEnumerable<string> alarmIds,
        int[] states,
        DateTime[] reactivations,
        WriteOptions? options = null,
        CancellationToken ct = default);

    // --- 语言 / 翻译 ---

    Task<IEnumerable<Language>> GetAllLanguages(CancellationToken ct = default);

    Task<IEnumerable<KeyValuePair<string, string>>> GetSymbolicTextTranslations(int languageId, CancellationToken ct = default);



}

