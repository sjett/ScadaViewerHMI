// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Services;
using NLog;
using System.Diagnostics;
using System.Globalization;
namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{
    /// <summary>
    /// 异构组操作
    /// </summary>
    [DebuggerDisplay("HeterogenousGroup")]
	internal class HeterogenousGroupAction : IAction
	{
		private static readonly Logger logger = LogManager.GetLogger("HeterogenousGroupAction");
		private readonly IObservableStack<IAction> actions = new ObservableStack<IAction>();
        /// <summary>
        /// 操作集合
        /// </summary>
        public IObservableStack<IAction> Actions
		{
			get
			{
				return this.actions;
			}
		}
        /// <summary>
        /// 与下一个操作分组
        /// </summary>
        /// <param name="nextAction"></param>
        /// <returns></returns>
        public IAction GroupWith(IAction nextAction)
		{
			this.Actions.Push(nextAction);
			return this;
		}
        /// <summary>
        /// 是否可以与下一个操作分组
        /// </summary>
        /// <param name="nextAction"></param>
        /// <returns></returns>
        public bool CanGroup(IAction nextAction)
		{
			return true;
		}
        /// <summary>
        /// 撤销操作
        /// </summary>
        public void Undo()
		{
			foreach (IAction current in this.actions)
			{
				HeterogenousGroupAction.logger.Trace<IAction>("Undo ({0})", current);
				current.Undo();
			}
		}
        /// <summary>
        /// 重做操作
        /// </summary>
        public void Redo()
		{
			foreach (IAction current in this.actions.Reverse<IAction>())
			{
				HeterogenousGroupAction.logger.Trace<IAction>("Redo ({0})", current);
				current.Redo();
			}
		}
        /// <summary>
        /// 字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "Heterogenous group ({0} actions)", new object[]
			{
				this.Actions.Count
			});
		}
	}
}
