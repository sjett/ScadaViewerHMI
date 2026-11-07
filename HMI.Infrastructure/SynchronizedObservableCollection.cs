// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace HMI.Infrastructure
{


    /// <summary>
    ///     同步可观察集合
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SynchronizedObservableCollection<T> : ISynchronizedObservableCollection<T>, IList, ICollection
    {
        private readonly IObservableCollection<T> items;
        private readonly object syncRoot;
        /// <summary>
        /// 集合变更事件
        /// </summary>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        /// <summary>
        /// 属性变更事件
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>
        /// 同步根对象
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return this.syncRoot;
            }
        }
        /// <summary>
        /// 集合是否同步线程安全
        /// </summary>
        bool ICollection.IsSynchronized
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 同步根对象
        /// </summary>
        object ICollection.SyncRoot
        {
            get
            {
                return this.syncRoot;
            }
        }
        /// <summary>
        /// 集合是否固定大小
        /// </summary>
        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 集合是否只读
        /// </summary>
        bool IList.IsReadOnly
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                this.EnsureValueIsValid(value);
                this[index] = (T)((object)value);
            }
        }
        /// <summary>
        /// 集合元素数量
        /// </summary>
        public int Count
        {
            get
            {
                int i;
                lock (this.syncRoot)
                {
                    i = this.items.Count;
                }
                return i;
            }
        }
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public T this[int index]
        {
            get
            {
                T result;
                lock (this.syncRoot)
                {
                    result = this.items[index];
                }
                return result;
            }
            set
            {
                lock (this.syncRoot)
                {
                    if (index < 0 || index >= this.items.Count)
                    {
                        throw new ArgumentOutOfRangeException("index", index, "Value must be in range.");
                    }
                    this.SetItem(index, value);
                }
            }
        }
        /// <summary>
        /// 集合是否只读
        /// </summary>
        bool ICollection<T>.IsReadOnly
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="syncRoot"></param>
        /// <param name="list"></param>
        public SynchronizedObservableCollection(object syncRoot = null, IEnumerable<T> list = null)
        {
            this.syncRoot = (syncRoot ?? new object());
            this.items = ((list == null) ? new FullyObservableCollection<T>() : new FullyObservableCollection<T>(new List<T>(list)));
            
            this.items.CollectionChanged += this.onCollectionChanged;
            this.items.PropertyChanged += this.onPropertyChanged;
        }
        /// <summary>
        /// 复制到数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        void ICollection.CopyTo(Array array, int index)
        {
            lock (this.syncRoot)
            {
                int num = 0;
                while (num < this.items.Count)
                {
                    array.SetValue(this.items[num], index + num);
                    num = num + 1;
                }
            }
        }
        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int IList.Add(object value)
        {
            int idx = 0;
            this.EnsureValueIsValid(value);
            lock (this.syncRoot)
            {
                this.Add((T)((object)value));
                idx = this.Count - 1;
            }
            return idx;
        }
        /// <summary>
        /// 是否包含元素
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        bool IList.Contains(object value)
        {
            this.EnsureValueIsValid(value);
            return this.Contains((T)((object)value));
        }
        /// <summary>
        /// 元素索引
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        int IList.IndexOf(object value)
        {
            this.EnsureValueIsValid(value);
            return this.IndexOf((T)((object)value));
        }
        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        void IList.Insert(int index, object value)
        {
            this.EnsureValueIsValid(value);
            this.Insert(index, (T)((object)value));
        }
        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="value"></param>
        void IList.Remove(object value)
        {
            this.EnsureValueIsValid(value);
            this.Remove((T)((object)value));
        }
        /// <summary>
        /// 添加元素
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            lock (this.syncRoot)
            {
                int i = this.items.Count;
                this.InsertItem(i, item);
            }
        }
        /// <summary>
        /// 清空集合
        /// </summary>
        public void Clear()
        {
            lock (this.syncRoot)
            {
                this.ClearItems();
            }
        }
        /// <summary>
        /// 是否包含元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            bool r;
            lock (this.syncRoot)
            {
                r = this.items.Contains(item);
            }
            return r;
        }
        /// <summary>
        /// 复制到数组
        /// </summary>
        /// <param name="array"></param>
        /// <param name="index"></param>
        public void CopyTo(T[] array, int index)
        {
            lock (this.syncRoot)
            {
                this.items.CopyTo(array, index);
            }
        }
        /// <summary>
        /// 获取枚举器
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            IEnumerator<T> enumerator;
            lock (this.syncRoot)
            {
                enumerator = this.items.GetEnumerator();
            }
            return enumerator;
        }
        /// <summary>
        /// 元素索引
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            int i;
            lock (this.syncRoot)
            {
                i = this.getItemIndex(item);
            }
            return i;
        }
        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Insert(int index, T item)
        {
            lock (this.syncRoot)
            {
                if (index < 0 || index > this.items.Count)
                {
                    throw new ArgumentOutOfRangeException("index", index, "Value must be in range.");
                }
                this.InsertItem(index, item);
            }
        }
        /// <summary>
        /// 移除元素
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            bool r;
            lock (this.syncRoot)
            {
                int i = this.getItemIndex(item);
                if (i < 0)
                {
                    r = false;
                }
                else
                {
                    this.RemoveItem(i);
                    r = true;
                }
            }
            return r;
        }
        /// <summary>
        /// 移除指定索引元素
        /// </summary>
        /// <param name="index"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void RemoveAt(int index)
        {
            lock (this.syncRoot)
            {
                if (index < 0 || index >= this.items.Count)
                {
                    throw new ArgumentOutOfRangeException("index", index, "Value must be in range.");
                }
                this.RemoveItem(index);
            }
        }
        /// <summary>
        /// 获取枚举器
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.items.GetEnumerator();
        }
        /// <summary>
        /// 清空集合
        /// </summary>
        protected virtual void ClearItems()
        {
            this.items.Clear();
        }
        /// <summary>
        /// 插入元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected virtual void InsertItem(int index, T item)
        {
            this.items.Insert(index, item);
        }
        /// <summary>
        /// 获取元素索引
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private int getItemIndex(T item)
        {
            int len = this.items.Count;
            int num = 0;
            while (num < len)
            {
                if (object.Equals(this.items[num], item))
                {
                    return num;
                }
                num = num + 1;
            }
            return -1;
        }
        /// <summary>
        /// 移除指定索引元素
        /// </summary>
        /// <param name="index"></param>
        protected virtual void RemoveItem(int index)
        {
            this.items.RemoveAt(index);
        }
        /// <summary>
        /// 设置指定索引元素
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        protected virtual void SetItem(int index, T item)
        {
            this.items[index] = item;
        }
        /// <summary>
        /// 检查值类型
        /// </summary>
        /// <param name="value"></param>
        /// <exception cref="ArgumentException"></exception>
        private void EnsureValueIsValid(object value)
        {
            if (value == null)
            {
                if (typeof(T).IsValueType)
                {
                    throw new ArgumentException("Synchronized collection wrong type null.");
                }
            }
            else
            {
                if (!(value is T))
                {
                    throw new ArgumentException("Synchronized collection wrong type.");
                }
            }
        }
        /// <summary>
        /// 移动元素
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        public void Move(int oldIndex, int newIndex)
        {
            lock (this.syncRoot)
            {
                this.items.Move(oldIndex, newIndex);
            }
        }
        /// <summary>
        /// 开始编辑
        /// </summary>
        public void BeginEdit()
        {
            lock (this.syncRoot)
            {
                this.items.BeginEdit();
            }
        }
        /// <summary>
        /// 结束编辑
        /// </summary>
        public void EndEdit()
        {
            lock (this.syncRoot)
            {
                this.items.EndEdit();
            }
        }
        /// <summary>
        /// 属性变更事件触发
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChanged?.Invoke(this, e);
        }
        /// <summary>
        /// 集合变更事件触发
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
        {
            CollectionChanged?.Invoke(this, e);
        }

        private void onCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            this.OnCollectionChanged(e);
        }

        private void onPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            this.OnPropertyChanged(e);
        }
    }
}
