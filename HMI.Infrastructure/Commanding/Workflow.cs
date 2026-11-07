// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Services;

namespace HMI.Infrastructure.Commanding
{
    internal class Workflow : IWorkflow
    {
        internal readonly List<IWorkflowAction> actions = new();
        private readonly ITaskRunner taskRunner;
        
        public IDictionary<string, object> Data
        {
            get;
            private set;
        }
        public SynchronizationContext SynchronizationContext
        {
            get;
            private set;
        }
        public Workflow(ITaskRunner taskRunner)
        {
            this.taskRunner = taskRunner;
            SynchronizationContext = SynchronizationContext.Current ?? new SynchronizationContext();
            Data = new Dictionary<string, object>();
        }
        public void InsertAction(IWorkflowAction action)
        {
            actions.Insert(0, action);
        }
        public void AppendAction(IWorkflowAction action)
        {
            actions.Add(action);
        }
        public void Run()
        {
            while (actions.Count > 0)
            {
                IWorkflowAction action = DequeueNextAction();
                switch (action.ExecutionMode)
                {
                    case ActionExecutionMode.Synchronous:
                        if (!ExecuteAction(action))
                        {
                            return;
                        }
                        break;
                    case ActionExecutionMode.Asynchronous:
                        {
                            WorkflowState workflowState = new WorkflowState(SynchronizationContext);
                            workflowState.Action = action;
                            taskRunner.BeginAction(ExecuteAsyncAction, OnAsyncActionCompleted, workflowState);
                            return;
                        }
                }
            }
        }
        private void OnAsyncActionCompleted(IAsyncResult asyncResult)
        {
            taskRunner.EndAction(asyncResult);
            WorkflowState workflowState = (WorkflowState)asyncResult.AsyncState;
            if (workflowState.ContinueExecution)
            {
                workflowState.SynchronizationContext.Post(ContinueWorkflow, null);
                Thread.Yield();
            }
        }
        private void ContinueWorkflow(object state)
        {
            Run();
        }
        private void ExecuteAsyncAction(object state)
        {
            WorkflowState workflowState = (WorkflowState)state;
            workflowState.ContinueExecution = ExecuteAction(workflowState.Action);
        }
        private IWorkflowAction DequeueNextAction()
        {
            IWorkflowAction result = actions.First();
            actions.RemoveAt(0);
            return result;
        }
        private bool ExecuteAction(IWorkflowAction action)
        {
            return action.Execute(this);
        }
    }
}
