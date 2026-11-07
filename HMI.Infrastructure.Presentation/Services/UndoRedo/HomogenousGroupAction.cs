// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Services;
using NLog;
using System.Diagnostics;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{
    /// <summary>
    /// 同类组合操作
    /// </summary>
    [DebuggerDisplay("HomogenousGroup")]
	internal class HomogenousGroupAction : IAction
	{
		private static readonly Logger logger = LogManager.GetLogger("HomogenousGroupAction");
        private readonly IAction[] action;
        /// <summary>
        /// 获取操作列表
        /// </summary>
        public IAction[] Actions
		{
			get
			{
				return this.action;
			}
		}
        /// <summary>
        /// 同类组合操作构造函数
        /// </summary>
        /// <param name="actions"></param>
        public HomogenousGroupAction(params IAction[] actions)
		{
			Contract.Requires<ArgumentNullException>(actions != null, "actions");
			Contract.Requires<ArgumentException>(actions.Length > 1, "actions");
			for (int i = 0; i < actions.Length; i++)
			{
				Contract.Requires<ArgumentNullException>(actions[i] != null, "actions[" + i + "]");
			}
			this.action = actions;
		}
        /// <summary>
        /// 将此操作与下一个操作组合
        /// </summary>
        /// <param name="nextAction"></param>
        /// <returns></returns>
        public IAction GroupWith(IAction nextAction)
		{
			IEnumerable<IAction> source = this.action.Concat(Enumerable.Repeat<IAction>(nextAction, 1));
			return new HomogenousGroupAction(source.ToArray<IAction>());
		}
        /// <summary>
        /// 确定此操作是否可以与另一个操作组合
        /// </summary>
        /// <param name="nextAction"></param>
        /// <returns></returns>
        public bool CanGroup(IAction nextAction)
		{
			return this.action.Last<IAction>().CanGroup(nextAction);
		}
        /// <summary>
        /// 撤销操作
        /// </summary>
        public void Undo()
		{
			foreach (IAction current in this.action.Reverse<IAction>())
			{
				HomogenousGroupAction.logger.Trace<IAction>("Undo ({0})", current);
				current.Undo();
			}
		}
        /// <summary>
        /// 重做操作
        /// </summary>
        public void Redo()
		{
			IAction[] array = this.action;
			for (int i = 0; i < array.Length; i++)
			{
				IAction action = array[i];
				HomogenousGroupAction.logger.Trace<IAction>("Redo ({0})", action);
				action.Redo();
			}
		}
        /// <summary>
        /// 返回操作的字符串表示形式
        /// </summary>
        /// <returns></returns>
        public override string ToString()
		{
			return string.Format(CultureInfo.InvariantCulture, "Homogenous group ({0} actions)", new object[]
			{
				this.action.Length
			});
		}
	}
}
