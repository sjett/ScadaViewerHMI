// SPDX-License-Identifier: GPL-3.0-only
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;


namespace HMI.Common.Collections;

/// <summary>
/// 可完全观测的集合
/// </summary>
/// <typeparam name="T"></typeparam>
public class FullyObservableCollection<T> : ObservableCollection<T>, IObservableCollection<T>
{
    protected bool isEditing;
    /// <summary>
    /// 默认构造函数
    /// </summary>
    public FullyObservableCollection()
    {
    }
    /// <summary>
    /// 使用初始项集合初始化
    /// </summary>
    /// <param name="items"></param>
    public FullyObservableCollection(IEnumerable<T> items)
        : base(items)
    {
    }
    /// <summary>
    /// 清除集合项
    /// </summary>
    protected override void ClearItems()
    {
        List<T> oldItems = this.ToList<T>();
        this.BeginEdit();
        base.ClearItems();
        this.EndEdit();
        this.OnCollectionChanged(new FullyObservableNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset)
        {
            OldItems = oldItems
        });
    }
    /// <summary>
    /// 开始编辑
    /// </summary>
    public void BeginEdit()
    {
        if (this.isEditing)
        {
            return;
        }
        this.isEditing = true;
    }
    /// <summary>
    /// 结束编辑
    /// </summary>
    public void EndEdit()
    {
        if (!this.isEditing)
        {
            return;
        }
        this.isEditing = false;
    }
    /// <summary>
    /// 集合变更事件重写
    /// </summary>
    /// <param name="e"></param>
    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
    {
        if (this.isEditing)
        {
            return;
        }
        base.OnCollectionChanged(e);
    }
    /// <summary>
    /// 移动元素
    /// </summary>
    /// <param name="oldIndex"></param>
    /// <param name="newIndex"></param>
    void IObservableCollection<T>.Move(int oldIndex, int newIndex)
    {
        base.Move(oldIndex, newIndex);
    }
}

public class FullyObservableNotifyCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
{
    private static readonly Lazy<Action<NotifyCollectionChangedEventArgs, IList>> oldItemsSetter =
        new(CreateOldItemsSetter);
    private IList oldItems;

    public new IList OldItems
    {
        get => oldItems;
        set
        {
            oldItems = value;
            oldItemsSetter.Value(this, value);
        }
    }

    public FullyObservableNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
        : base(action)
    {
    }
    public FullyObservableNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList items)
        : base(action, items)
    {
        oldItems = base.OldItems;
    }

    private static Action<NotifyCollectionChangedEventArgs, IList> CreateOldItemsSetter()
    {
        // 基类没有公开设置 OldItems 的 API；同步其私有字段，保证基类属性也能读到新集合。
        var oldItemsField = typeof(NotifyCollectionChangedEventArgs)
            .GetField("_oldItems", BindingFlags.Instance | BindingFlags.NonPublic)
            ?? throw new MissingFieldException(typeof(NotifyCollectionChangedEventArgs).FullName, "_oldItems");

        var setterMethod = new DynamicMethod(
            "SetOldItems",
            null,
            new[] { typeof(NotifyCollectionChangedEventArgs), typeof(IList) },
            typeof(NotifyCollectionChangedEventArgs),
            true);

        ILGenerator setterIl = setterMethod.GetILGenerator();
        setterIl.Emit(OpCodes.Ldarg_0);
        setterIl.Emit(OpCodes.Ldarg_1);
        setterIl.Emit(OpCodes.Stfld, oldItemsField);
        setterIl.Emit(OpCodes.Ret);

        return (Action<NotifyCollectionChangedEventArgs, IList>)
            setterMethod.CreateDelegate(typeof(Action<NotifyCollectionChangedEventArgs, IList>));
    }
}

//public interface IFullyObservableCollection<T> : IList<T>
//{
//    event EventHandler<FullyCollectionChangedEventArgs> FullyCollectionChanged;
//    void BeginEdit();
//    void EndEdit();
//}

//public class FullyObservableCollection<T> : ObservableCollection<T>, IObservableCollection<T>
//{
//    private bool isEditing;

//    public event EventHandler<FullyCollectionChangedEventArgs> FullyCollectionChanged;

//    public FullyObservableCollection()
//    {
//    }

//    public FullyObservableCollection(IEnumerable<T> items)
//        : base(items)
//    {
//    }

//    public void BeginEdit()
//    {
//        isEditing = true;
//    }

//    public void EndEdit()
//    {
//        isEditing = false;
//    }

//    protected override void ClearItems()
//    {
//        var oldItems = this.ToList();

//        BeginEdit();
//        base.ClearItems();
//        EndEdit();

//        this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(
//                NotifyCollectionChangedAction.Reset,
//                oldItems,
//                null
//                ));

//        RaiseFullyChanged(
//            new FullyCollectionChangedEventArgs(
//                NotifyCollectionChangedAction.Reset,
//                oldItems,
//                null));
//    }

//    protected override void InsertItem(int index, T item)
//    {
//        base.InsertItem(index, item);

//        if (!isEditing)
//        {
//            RaiseFullyChanged(
//                new FullyCollectionChangedEventArgs(
//                    NotifyCollectionChangedAction.Add,
//                    null,
//                    new[] { item },
//                    -1,
//                    index));
//        }
//    }

//    protected override void RemoveItem(int index)
//    {
//        var oldItem = this[index];

//        base.RemoveItem(index);

//        if (!isEditing)
//        {
//            RaiseFullyChanged(
//                new FullyCollectionChangedEventArgs(
//                    NotifyCollectionChangedAction.Remove,
//                    new[] { oldItem },
//                    null,
//                    index,
//                    -1));
//        }
//    }

//    protected override void SetItem(int index, T item)
//    {
//        var oldItem = this[index];

//        base.SetItem(index, item);

//        if (!isEditing)
//        {
//            RaiseFullyChanged(
//                new FullyCollectionChangedEventArgs(
//                    NotifyCollectionChangedAction.Replace,
//                    new[] { oldItem },
//                    new[] { item },
//                    index,
//                    index));
//        }
//    }

//    protected override void MoveItem(int oldIndex, int newIndex)
//    {
//        var item = this[oldIndex];

//        base.MoveItem(oldIndex, newIndex);

//        if (!isEditing)
//        {
//            RaiseFullyChanged(
//                new FullyCollectionChangedEventArgs(
//                    NotifyCollectionChangedAction.Move,
//                    new[] { item },
//                    new[] { item },
//                    oldIndex,
//                    newIndex));
//        }
//    }
//    protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
//    {
//        // 批量编辑期间禁止触发系统事件
//        if (isEditing)
//            return;

//        base.OnCollectionChanged(e);
//    }


//    private void RaiseFullyChanged(FullyCollectionChangedEventArgs args)
//    {
//        FullyCollectionChanged?.Invoke(this, args);
//    }
//}

//public class FullyCollectionChangedEventArgs : EventArgs
//{
//    public NotifyCollectionChangedAction Action { get; }
//    public IList OldItems { get; }
//    public IList NewItems { get; }
//    public int OldIndex { get; }
//    public int NewIndex { get; }

//    public FullyCollectionChangedEventArgs(
//        NotifyCollectionChangedAction action,
//        IList oldItems,
//        IList newItems,
//        int oldIndex = -1,
//        int newIndex = -1)
//    {
//        Action = action;
//        OldItems = oldItems;
//        NewItems = newItems;
//        OldIndex = oldIndex;
//        NewIndex = newIndex;
//    }
//}
