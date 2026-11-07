// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Common;

namespace HMI.Infrastructure.Services
{
    internal class AsyncResultNoResult : IAsyncResult
    {
        private const int StatePending = 0;
        private const int StateCompletedSynchronously = 1;
        private const int StateCompletedAsynchronously = 2;
        private readonly AsyncCallback asyncCallback;
        private readonly object asyncState;
        private int completedState;
        private ManualResetEvent asyncWaitHandle;
        private Exception exception;
        public object AsyncState
        {
            get
            {
                return asyncState;
            }
        }
        public bool CompletedSynchronously
        {
            get
            {
                return Thread.VolatileRead(ref completedState) == 1;
            }
        }
        public WaitHandle AsyncWaitHandle
        {
            get
            {
                if (asyncWaitHandle == null)
                {
                    bool isCompleted;
                    ManualResetEvent manualResetEvent = new ManualResetEvent(isCompleted = IsCompleted);
                    if (Interlocked.CompareExchange<ManualResetEvent>(ref asyncWaitHandle, manualResetEvent, null) != null)
                    {
                        manualResetEvent.Close();
                    }
                    else
                    {
                        if (!isCompleted && IsCompleted)
                        {
                            asyncWaitHandle.Set();
                        }
                    }
                }
                return asyncWaitHandle;
            }
        }
        public bool IsCompleted
        {
            get
            {
                return Thread.VolatileRead(ref completedState) != 0;
            }
        }
        public AsyncResultNoResult(AsyncCallback asyncCallback, object state)
        {
            this.asyncCallback = asyncCallback;
            asyncState = state;
        }
        public void SetAsCompleted(Exception exception, bool completedSynchronously)
        {
            this.exception = exception;
            SetCompletionStatus(completedSynchronously);
            SetCompletionEvent();
            ExecuteCallback();
        }
        private void ExecuteCallback()
        {
            if (asyncCallback != null)
            {
                asyncCallback(this);
            }
        }
        private void SetCompletionEvent()
        {
            if (asyncWaitHandle != null)
            {
                asyncWaitHandle.Set();
            }
        }
        private void SetCompletionStatus(bool completedSynchronously)
        {
            int num = Interlocked.Exchange(ref completedState, completedSynchronously ? 1 : 2);
            if (num != 0)
            {
                throw new InvalidOperationException("You can set a result only once");
            }
        }
        public void EndInvoke()
        {
            if (!IsCompleted)
            {
                AsyncWaitHandle.WaitOne();
                AsyncWaitHandle.Close();
                asyncWaitHandle = null;
            }
            if (exception != null)
            {
                exception.RethrowWithNoStackTraceLoss();
            }
        }
    }
}
