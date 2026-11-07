// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Common.Presentation;

public interface IHmiModule
{
    void RegisterTypes(IServiceCollection services);
    void OnInitialized(IServiceProvider services);
}
