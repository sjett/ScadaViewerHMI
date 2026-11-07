// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Services;
using NLog;
using HMI.Common.Instrumentation.ErrorHandling;
namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{
	/// <summary>
	/// 操作监视器类
	/// </summary>
	public class ActionMonitor : IActionMonitor
	{
        private readonly IEventAggregator eventAggregator;
        private readonly IObservableStack<IAction> redoStack = new ObservableStack<IAction>();
        private readonly IObservableStack<IAction> undoStack = new ObservableStack<IAction>();
		private bool startGrouping = true;
        private bool disabled;
        private readonly Logger logger = LogManager.GetLogger("ActionMonitor");

        /// <summary>
        /// 重做栈
        /// </summary>
        public IObservableStack<IAction> RedoStack
		{
			get
			{
				return this.redoStack;
			}
		}
        /// <summary>
        /// 撤销栈
        /// </summary>
        public IObservableStack<IAction> UndoStack
		{
			get
			{
				return this.undoStack;
			}
		}
        /// <summary>
        /// 是否有记录操作
        /// </summary>
        public bool AreActionsRecorded
		{
			get
			{
				return this.undoStack.Count > 0;
			}
		}
        /// <summary>
        /// 是否可以撤销
        /// </summary>
        public bool CanUndo
		{
			get
			{
				return this.undoStack.Count > 0;
			}
		}
        /// <summary>
        /// 是否可以重做
        /// </summary>
        public bool CanRedo
		{
			get
			{
				return this.redoStack.Count > 0;
			}
		}
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="eventAggregator"></param>
        /// <param name="logger"></param>
        public ActionMonitor(IEventAggregator eventAggregator)
		{
			Contract.Requires<ArgumentNullException>(eventAggregator != null, "eventAggregator");
			this.eventAggregator = eventAggregator;
		}
        /// <summary>
        /// 取消当前操作
        /// </summary>
        public void CancelAction()
		{
			this.logger.Trace("Cancelling current action ({0})",this.undoStack.Peek());
			if (this.startGrouping)
			{
				this.EndGrouping();
			}
			this.undoStack.Pop();
		}
        /// <summary>
        /// 结束组合操作
        /// </summary>
        public void EndGrouping()
		{
			this.logger.Trace("Ending grouping");
			this.startGrouping = false;
		}
        /// <summary>
        /// 禁用操作监视器
        /// </summary>
        public void Disable()
		{
            this.logger.Trace("Action recording OFF");
            this.disabled = true;
        }
        /// <summary>
        /// 开始组合操作
        /// </summary>
        public void StartGrouping()
		{
			this.logger.Trace("Starting grouping");
			HeterogenousGroupAction value = new HeterogenousGroupAction();
			this.undoStack.Push(value);
			this.startGrouping = true;
		}
        /// <summary>
        /// 撤销操作
        /// </summary>
        public void Undo()
		{
			this.logger.Trace("Undo ({0})", this.undoStack.Peek());

            this.logger.Trace("Publishing Undoing event");
            this.eventAggregator
                .GetEvent<UndoingEvent>()
				?.Publish(null);

            this.startGrouping = false;

            this.logger.Trace("Action recording OFF");
            this.disabled = true;

            IAction action = this.undoStack.Pop();
			action.Undo();
			this.redoStack.Push(action);

            this.logger.Trace("Action recording ON");
            this.disabled = false;

            this.logger.Trace("Publishing Undo event");
            this.eventAggregator
                .GetEvent<UndoEvent>()
                ?.Publish(null);
        }
        /// <summary>
        /// 重做操作
        /// </summary>
		public void Redo()
		{
			this.logger.Trace("Redo ({0})", this.redoStack.Peek());
			
            this.logger.Trace("Publishing Redoing event");
            this.eventAggregator
                .GetEvent<RedoingEvent>()
                ?.Publish(null);

            this.startGrouping = false;

            this.logger.Trace("Action recording OFF");
            this.disabled = true;

            IAction action = this.redoStack.Pop();
			action.Redo();
			this.undoStack.Push(action);

            this.logger.Trace("Action recording ON");
            this.disabled = false;

            this.logger.Trace("Publishing Redo event");
            this.eventAggregator
                .GetEvent<RedoEvent>()
                ?.Publish(null);
        }
        /// <summary>
        /// 重置操作监视器
        /// </summary>
		public void Reset()
		{
			this.logger.Trace("Resetting actions (undo count: {0} redo count: {1})", this.undoStack.Count, this.redoStack.Count);
			this.undoStack.Clear();
			this.redoStack.Clear();
			this.startGrouping = true;
			
            this.logger.Trace("Action recording ON");
            this.disabled = false;
        }
        /// <summary>
        /// 记录操作
        /// </summary>
        /// <param name="action"></param>
		public void RecordAction(IAction action)
		{
			if (this.disabled)
			{
				return;
			}
			this.logger.Trace("Recording action ({0})", action);
			if (this.startGrouping && this.AreActionsRecorded)
			{
                IAction _action = this.undoStack.Peek();
                if (_action.CanGroup(action))
                {
                    this.undoStack.Pop();
                    IAction __action = _action.GroupWith(action);
                    this.logger.Trace("Creating action group ({0})", __action);
                    this.undoStack.Push(__action);
                    return;
                }
                this.undoStack.Push(action);
            }
			else
			{
				this.undoStack.Push(action);
			}
			this.redoStack.Clear();
			this.startGrouping = true;
		}
	}
}
