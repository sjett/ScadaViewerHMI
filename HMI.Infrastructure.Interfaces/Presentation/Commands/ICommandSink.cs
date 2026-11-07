// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows.Input;

namespace HMI.Infrastructure.Presentation.Commands
{
    /// <summary>
    /// 命令接收器接口
    /// </summary>
    public interface ICommandSink
    {
        /// <summary>
        /// 注册命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="canExecute"></param>
        /// <param name="execute"></param>
        void RegisterCommand(ICommand command, Predicate<object> canExecute, Action<object> execute);
        /// <summary>
        /// 注销命令
        /// </summary>
        /// <param name="command"></param>
        void UnregisterCommand(ICommand command);
        /// <summary>
        /// 能否执行命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        /// <param name="handled"></param>
        /// <returns></returns>
        bool CanExecuteCommand(ICommand command, object parameter, out bool handled);
        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        /// <param name="handled"></param>
        void ExecuteCommand(ICommand command, object parameter, out bool handled);
    }
}
