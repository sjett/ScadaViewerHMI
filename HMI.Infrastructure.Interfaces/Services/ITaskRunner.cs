// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Services
{
    public interface ITaskRunner
    {
        IAsyncResult BeginAction(Action<object> task, AsyncCallback callback, object state);
        void EndAction(IAsyncResult asyncResult);
        IAsyncResult BeginTask<T>(Func<object, T> task, AsyncCallback callback, object state);
        T EndTask<T>(IAsyncResult asyncResult);
    }
}
