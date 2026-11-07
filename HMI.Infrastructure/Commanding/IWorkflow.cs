// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Commanding
{
    public interface IWorkflow
    {
        SynchronizationContext SynchronizationContext
        {
            get;
        }
        IDictionary<string, object> Data
        {
            get;
        }
        void InsertAction(IWorkflowAction action);
        void AppendAction(IWorkflowAction action);
        void Run();
    }
}
