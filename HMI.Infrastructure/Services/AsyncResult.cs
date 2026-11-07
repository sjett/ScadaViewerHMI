// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HMI.Infrastructure.Services
{
    internal class AsyncResult<TResult> : AsyncResultNoResult
    {
        private TResult result = default;
        public AsyncResult(AsyncCallback asyncCallback, object state)
            : base(asyncCallback, state)
        {
        }
        public void SetAsCompleted(TResult result, bool completedSynchronously)
        {
            this.result = result;
            SetAsCompleted(null, completedSynchronously);
        }
        public new TResult EndInvoke()
        {
            base.EndInvoke();
            return result;
        }
    }
}
