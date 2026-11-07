// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Services;
using System.Diagnostics;
namespace HMI.Modules.PropertyEditor.Services
{
	[DebuggerDisplay("MultiboundUndo {memberName}")]
	public class MultiboundUndoAction : IAction
	{
        private readonly IList<IAction> _actions = new List<IAction>();
        private readonly string _memberName;
        private readonly object _source;
		
		public bool IsCollectingActions
		{
			get;
			set;
		}
		public MultiboundUndoAction(object source, string memberName)
		{
            this._memberName = memberName;
            this._source = source;
		}
		public IAction GroupWith(IAction nextAction)
		{
			MultiboundUndoAction multiboundUndoAction = nextAction as MultiboundUndoAction;
			if (multiboundUndoAction != null)
			{
				AppendActionsFrom(multiboundUndoAction);
				return multiboundUndoAction;
			}
			AddAction(nextAction);
			return this;
		}
        private void AppendActionsFrom(MultiboundUndoAction otherActionGroup)
        {
            IAction[] groupedActions = this._actions.Concat(otherActionGroup._actions).ToArray<IAction>();
            otherActionGroup._actions.Clear();
            otherActionGroup._actions.AddRange(groupedActions);
        }
        private void AddAction(IAction action)
        {
            this._actions.Add(action);
        }
        public bool CanGroup(IAction nextAction)
		{
			if (IsCollectingActions)
			{
				return true;
			}
			MultiboundUndoAction multiboundUndoAction = nextAction as MultiboundUndoAction;
			return multiboundUndoAction != null && HasSameUndoTarget(multiboundUndoAction);
		}
        private bool HasSameUndoTarget(MultiboundUndoAction otherActionGroup)
        {
            return object.ReferenceEquals(otherActionGroup._source, this._source) && otherActionGroup._memberName == this._memberName;
        }
        public void Undo()
		{
			foreach (IAction action in this._actions.Reverse())
			{
				action.Undo();
			}
		}
		public void Redo()
		{
			foreach (IAction action in this._actions)
			{
				action.Redo();
			}
		}
	}
}
