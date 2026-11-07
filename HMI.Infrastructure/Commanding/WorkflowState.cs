// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Commanding
{
    internal class WorkflowState
    {
        public IWorkflowAction Action
        {
            get;
            set;
        }
        public bool ContinueExecution
        {
            get;
            set;
        }
        public SynchronizationContext SynchronizationContext
        {
            get;
            private set;
        }
        public WorkflowState(SynchronizationContext synchronizationContext)
        {
            SynchronizationContext = synchronizationContext;
        }
    }
}
