// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMI.Runtime.Core.Helper.Models;

public class ServerSettings
{
    public string? DefaultServerName { get; set; }
    public int SubscriptionPollingIntervalMilliseconds { get; set; } = 500;
    public List<ServerConfig> Servers { get; set; } = new();
}

public class ServerConfig
{
    public string Name { get; set; } = default!;
    public string Host { get; set; } = default!;
    public int Port { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Instance { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int Timeout { get; set; } = 10000;
}
