// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
namespace HMI.Editor.Common
{
	public class ZoomTickValueCollection : Freezable, IList<ZoomTickValue>, ICollection<ZoomTickValue>, IEnumerable<ZoomTickValue>, IEnumerable
	{
		private List<ZoomTickValue> ticks = new List<ZoomTickValue>();
		public ZoomTickValue this[int index]
		{
			get
			{
				return ticks[index];
			}
			set
			{
                ticks[index] = value;
			}
		}
		public int Count
		{
			get
			{
				return ticks.Count;
			}
		}
		public bool IsReadOnly
		{
			get
			{
				return ((ICollection<ZoomTickValue>)ticks).IsReadOnly;
			}
		}
		protected override Freezable CreateInstanceCore()
		{
			return new ZoomTickValueCollection();
		}
		public int IndexOf(ZoomTickValue item)
		{
			return ticks.IndexOf(item);
		}
		public void Insert(int index, ZoomTickValue item)
		{
            ticks.Insert(index, item);
		}
		public void RemoveAt(int index)
		{
            ticks.RemoveAt(index);
		}
		public void Add(ZoomTickValue item)
		{
            ticks.Add(item);
		}
		public void Clear()
		{
            ticks.Clear();
		}
		public bool Contains(ZoomTickValue item)
		{
			return ticks.Contains(item);
		}
		public void CopyTo(ZoomTickValue[] array, int arrayIndex)
		{
            ticks.CopyTo(array, arrayIndex);
		}
		public bool Remove(ZoomTickValue item)
		{
			return ticks.Remove(item);
		}
		public IEnumerator<ZoomTickValue> GetEnumerator()
		{
			return ticks.GetEnumerator();
		}
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ticks.GetEnumerator();
		}
	}
}
