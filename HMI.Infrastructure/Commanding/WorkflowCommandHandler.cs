// SPDX-License-Identifier: GPL-3.0-only

namespace HMI.Infrastructure.Commanding
{
    public abstract class WorkflowCommandHandler : CommandHandler
    {
       
        protected internal IServiceProvider ServiceLocator
        {
            get;
            private set;
        }
        public WorkflowCommandHandler(IServiceProvider serviceLocator)
        {
            ServiceLocator = serviceLocator;
        }
        protected void AddActions(IWorkflow workflow, IEnumerable<string> actionNames)
        {
            foreach (string actionName in actionNames)
            {
                AppendAction(actionName, workflow);
            }
        }
        private void AppendAction(string actionName, IWorkflow workflow)
        {
            IWorkflowAction action = this.ServiceLocator.Resolve<IWorkflowAction>(actionName);
            workflow.AppendAction(action);
        }
        protected sealed override void Execute(object state)
        {
            IWorkflow workflow = ServiceLocator.Resolve<IWorkflow>();
            ConfigureWorkflow(workflow, state);
            List<string> actionNames = new List<string>();
            SetActions(actionNames);
            AddActions(workflow, actionNames);
            workflow.Run();
        }
        protected abstract void SetActions(ICollection<string> actionNames);
        protected virtual void ConfigureWorkflow(IWorkflow workflow, object state)
        {
        }
    }
}
