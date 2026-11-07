// SPDX-License-Identifier: GPL-3.0-only
using System;
namespace HMI.Common.DataAccess
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString();
    }
}
