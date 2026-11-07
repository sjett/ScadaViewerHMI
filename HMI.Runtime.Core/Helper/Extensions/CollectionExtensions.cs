// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
namespace HMI.Runtime.Core.Helper.Extensions
{
	public static class CollectionExtensions
	{
		public static void ReplaceWithRange<T>(this ICollection<T> target, IEnumerable<T> range)
		{
			target.Clear();
			using (IEnumerator<T> enumerator = range.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T current = enumerator.Current;
					target.Add(current);
				}
			}
		}
	}
}
