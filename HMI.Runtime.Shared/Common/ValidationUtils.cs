// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
namespace HMI.Runtime.Shared.Common
{
	public static class ValidationUtils
	{
		[Obsolete("Use CheckIsNull<T> instead")]
		public static void CheckNullParam(object param, string paramName)
		{
			if (param == null)
			{
				throw new ArgumentNullException(paramName);
			}
		}
		public static void CheckIsNull<T>(Expression<Func<T>> param)
		{
			MemberExpression memberExpression = param.Body as MemberExpression;
			T t = param.Compile().Invoke();
			if (t == null)
			{
				string name = memberExpression.Member.Name;
				throw new ArgumentNullException(name);
			}
		}
		public static void CheckNullOrEmptyString(string param, string paramName)
		{
			if (string.IsNullOrEmpty(param))
			{
				throw new ArgumentNullException(paramName);
			}
		}
		[Obsolete("Use CheckIsNull<T> instead")]
		public static void CheckIsNull(object argument)
		{
			if (argument == null)
			{
				throw new ArgumentNullException();
			}
		}
		public static void CheckIsLessThan(int argument, int value)
		{
			if (argument < value)
			{
				throw new ArgumentException();
			}
		}
		public static void CheckIsNullOrEmpty(string argument)
		{
			if (string.IsNullOrEmpty(argument))
			{
				throw new ArgumentException();
			}
		}
		public static void CheckIsNullOrEmpty<T>(IEnumerable<T> argument)
		{
			if (argument == null || Enumerable.Count<T>(argument) == 0)
			{
				throw new ArgumentException();
			}
		}
	}
}
