// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Commands;

namespace HMI.Infrastructure.Commanding
{
    public interface ICommandHandler
    {
        void RegisterCommand(ICommandSink commandSink);
    }
}
