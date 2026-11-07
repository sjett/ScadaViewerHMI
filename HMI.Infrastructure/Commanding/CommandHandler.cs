// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Commands;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Windows.Input;

namespace HMI.Infrastructure.Commanding
{
    public abstract class CommandHandler : ICommandHandler
    {
        protected abstract ICommand Command
        {
            get;
        }
        public void RegisterCommand(ICommandSink commandSink)
        {
            Contract.Requires<ArgumentNullException>(!(commandSink == null), "commandSink");
            commandSink.RegisterCommand(Command, new Predicate<object>(CanExecute), new Action<object>(Execute));
        }
        protected abstract bool CanExecute(object state);
        protected abstract void Execute(object state);
    }
}
