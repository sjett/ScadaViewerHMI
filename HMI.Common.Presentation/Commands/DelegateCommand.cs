// SPDX-License-Identifier: GPL-3.0-only
using System.Windows.Input;

namespace HMI.Common.Presentation.Commands;

public abstract class DelegateCommandBase : ICommand
{
    public event EventHandler? CanExecuteChanged;

    public abstract bool CanExecute(object? parameter);

    public abstract void Execute(object? parameter);

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}

public sealed class DelegateCommand : DelegateCommandBase
{
    private readonly Action execute;
    private readonly Func<bool> canExecute;

    public DelegateCommand(Action execute, Func<bool>? canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute ?? (() => true);
    }

    public override bool CanExecute(object? parameter) => canExecute();

    public override void Execute(object? parameter) => execute();
}

public sealed class DelegateCommand<T> : DelegateCommandBase
{
    private readonly Action<T?> execute;
    private readonly Func<T?, bool> canExecute;

    public DelegateCommand(Action<T?> execute, Func<T?, bool>? canExecute = null)
    {
        this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
        this.canExecute = canExecute ?? (_ => true);
    }

    public override bool CanExecute(object? parameter) => canExecute(ConvertParameter(parameter));

    public override void Execute(object? parameter) => execute(ConvertParameter(parameter));

    private static T? ConvertParameter(object? parameter)
    {
        if (parameter is null)
        {
            return default;
        }

        if (parameter is T value)
        {
            return value;
        }

        throw new ArgumentException($"Command parameter must be assignable to {typeof(T)}.", nameof(parameter));
    }
}
