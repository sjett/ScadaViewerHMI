// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI.Runtime.Communication;

public sealed class SignalValue
{
    public string Name { get; init; } = default!;
    public object? Value { get; init; }
    public DateTime Timestamp { get; init; }
}


public sealed class Alarm
{
    public Guid Id { get; init; }
    public string Name { get; init; } = default!;
    public string Message { get; init; } = default!;
    public string Severity { get; init; } = default!;
    public DateTime Timestamp { get; init; }
    public bool Active { get; init; }
}


public sealed class UserInfo
{
    public string UserName { get; init; } = default!;
    public int UserLevel { get; init; }
    public bool IsDomainUser { get; init; }

    public IReadOnlyList<string> ProjectAuthorizations { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> SystemAuthorizations { get; init; } = Array.Empty<string>();
    public IReadOnlyList<string> AuthorizationGroups { get; init; } = Array.Empty<string>();
}


public sealed class WriteOptions
{
    public string? User { get; init; }
    public string? Password { get; init; }
}


public record SubscriptionRequest( string[] Signals);

public sealed class Language
{
    public int LanguageId { get; init; }
    public string Description { get; init; } = default!;
    public bool IsActive { get; init; }
    public bool IsDefault { get; init; }
}
public enum SecurityStatus
{
    SecurityUnknown,
    SecurityOn,
    SecurityOff
}