// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Commands;
using System.Windows.Input;

namespace HMI.Infrastructure.Commanding
{
    public class CommandSink : ICommandSink
    {
        private struct CommandHandlers
        {
            public readonly Predicate<object> CanExecute;
            public readonly Action<object> Execute;
            public CommandHandlers(Predicate<object> canExecute, Action<object> execute)
            {
                CanExecute = canExecute;
                Execute = execute;
            }
        }
        private readonly Dictionary<ICommand, CommandHandlers> _handlersByCommand = new Dictionary<ICommand, CommandHandlers>();
        public void RegisterCommand(ICommand command, Predicate<object> canExecute, Action<object> execute)
        {
            ThrowIfNull(command, "command");
            ThrowIfNull(canExecute, "canExecute");
            ThrowIfNull(execute, "execute");
            _handlersByCommand[command] = new CommandHandlers(canExecute, execute);
        }
        public void UnregisterCommand(ICommand command)
        {
            ThrowIfNull(command, "command");
            if (_handlersByCommand.ContainsKey(command))
            {
                _handlersByCommand.Remove(command);
            }
        }
        public virtual bool CanExecuteCommand(ICommand command, object parameter, out bool handled)
        {
            ThrowIfNull(command, "command");
            if (_handlersByCommand.ContainsKey(command))
            {
                handled = true;
                return _handlersByCommand[command].CanExecute(parameter);
            }
            handled = false;
            return false;
        }
        public virtual void ExecuteCommand(ICommand command, object parameter, out bool handled)
        {
            ThrowIfNull(command, "command");
            if (_handlersByCommand.ContainsKey(command))
            {
                handled = true;
                _handlersByCommand[command].Execute(parameter);
                return;
            }
            handled = false;
        }

        private static void ThrowIfNull(object value, string parameterName)
        {
            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }
        }
    }
}
