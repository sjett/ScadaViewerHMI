// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Runtime.Serialization;

namespace HMI.Common.Instrumentation.Metadata
{
    /// <summary>
    /// 反射异常
    /// </summary>
    [Serializable]
    public class ReflectionException : InvalidOperationException
    {
        public ReflectionException()
        {
        }
        public ReflectionException(string message)
            : base(message)
        {
        }
        public ReflectionException(string message, Exception inner)
            : base(message, inner)
        {
        }
        protected ReflectionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
