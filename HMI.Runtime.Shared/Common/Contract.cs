// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics;
using System.Reflection;
namespace HMI.Runtime.Shared.Common
{
	public static class Contract
	{
		public static void CheckIsNull(object argument, string argumentName)
		{
			if (argument == null)
			{
				throw new ArgumentNullException(argumentName);
			}
		}
		public static void CheckIsLessThan(int argument, string argumentName, int value)
		{
			if (argument < value)
			{
				throw new ArgumentException(argumentName);
			}
		}
		public static void CheckIsNullOrEmpty(string argument, string argumentName)
		{
			if (string.IsNullOrEmpty(argument))
			{
				throw new ArgumentException(argumentName);
			}
		}
		[DebuggerStepThrough]
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
