// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Services
{
    /// <summary>
    /// 任务运行器，提供异步执行任务的功能。
    /// </summary>
    public class TaskRunner : ITaskRunner
    {
        public IAsyncResult BeginAction(Action<object> action, AsyncCallback callback, object asyncState)
        {

            AsyncResultNoResult asyncResultNoResult = new AsyncResultNoResult(callback, asyncState);

            ThreadPool.QueueUserWorkItem(new WaitCallback(state =>
            {
                AsyncResultNoResult asyncResultNoResult = (AsyncResultNoResult)state!;
                try
                {
                    action(asyncResultNoResult.AsyncState);
                    asyncResultNoResult.SetAsCompleted(null, false);
                }
                catch (Exception exception)
                {
                    asyncResultNoResult.SetAsCompleted(exception, false);
                }
            }), asyncResultNoResult);

            return asyncResultNoResult;
        }
        public void EndAction(IAsyncResult asyncResult)
        {
            AsyncResultNoResult result = (AsyncResultNoResult)asyncResult;
            result.EndInvoke();
        }
        public IAsyncResult BeginTask<T>(Func<object, T> task, AsyncCallback callback, object asyncState)
        {
            AsyncResult<T> asyncResult = new AsyncResult<T>(callback, asyncState);

            ThreadPool.QueueUserWorkItem(new WaitCallback(state => this.ExecuteTask(task, state!)), asyncResult);

            return asyncResult;
        }
        public T EndTask<T>(IAsyncResult asyncResult)
        {
            AsyncResult<T> result = (AsyncResult<T>)asyncResult;
            return result.EndInvoke();
        }
        private void ExecuteTask<T>(Func<object, T> task, object state)
        {
            AsyncResult<T> asyncResult = (AsyncResult<T>)state;
            try
            {
                T result = task(asyncResult.AsyncState);
                asyncResult.SetAsCompleted(result, false);
            }
            catch (Exception exception)
            {
                asyncResult.SetAsCompleted(exception, false);
            }
        }
    }
}
