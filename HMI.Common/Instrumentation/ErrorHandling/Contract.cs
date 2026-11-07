// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics;
using System.Reflection;

namespace HMI.Common.Instrumentation.ErrorHandling
{
    public static class Contract
    {
        public static void Requires<TException>(bool condition) where TException : Exception, new()
        {
            if (!condition)
            {
                Type typeFromHandle = typeof(TException);
                ConstructorInfo constructor = typeFromHandle.GetConstructor(Type.EmptyTypes);
                Exception ex = constructor.Invoke(null) as Exception;
                throw ex;
            }
        }
        [DebuggerStepThrough]
        public static void Requires<TException>(bool condition, string userMessage) where TException : Exception, new()
        {
            if (!condition)
            {
                Type typeFromHandle = typeof(TException);
                ConstructorInfo constructor = typeFromHandle.GetConstructor(new Type[]
                {
                    typeof(string)
                });
                Exception ex;
                if (constructor == null)
                {
                    constructor = typeFromHandle.GetConstructor(Type.EmptyTypes);
                    ex = (constructor.Invoke(null) as Exception);
                }
                else
                {
                    ex = (constructor.Invoke(new object[]
                    {
                        userMessage
                    }) as Exception);
                }
                throw ex;
            }
        }
        public static void Assert(bool condition)
        {
            if (!condition)
            {
                throw new InvalidOperationException();
            }
        }
    }
}
