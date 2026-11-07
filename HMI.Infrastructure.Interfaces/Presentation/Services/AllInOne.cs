// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.ViewModels;
using System.Runtime.Serialization;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 剪贴板对象
    /// </summary>
    [Serializable]
    public class ClipboardObject : ISerializable
    {
        /// <summary>
        /// 类型
        /// </summary>
        public Type Type
        {
            get;
            set;
        }
        /// <summary>
        /// 序列化对象
        /// </summary>
        public string SerializedObject
        {
            get;
            set;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public ClipboardObject(SerializationInfo info, StreamingContext context)
        {
            Type = (Type)info.GetValue("Type", typeof(Type));
            SerializedObject = info.GetString("SerializedObject");
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <param name="serializedObject"></param>
        public ClipboardObject(Type type, string serializedObject)
        {
            Type = type;
            SerializedObject = serializedObject;
        }
        /// <summary>
        /// 序列化对象数据
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Type", Type);
            info.AddValue("SerializedObject", SerializedObject);
        }
    }

    /// <summary>
    /// 缓存接口
    /// </summary>
    /// <typeparam name="TKey">缓存键类型</typeparam>
    /// <typeparam name="TValue">缓存值类型</typeparam>
    public interface ICache<TKey, TValue>
    {
        /// <summary>
        /// 获取或缓存条目
        /// </summary>
        /// <param name="key">缓存键</param>
        /// <param name="resolveUncachedEntryCallback">未缓存条目解析回调</param>
        /// <returns>缓存值</returns>
        TValue GetOrCacheEntry(TKey key, Func<TKey, TValue> resolveUncachedEntryCallback);
        /// <summary>
        /// 移除条目
        /// </summary>
        /// <param name="key">缓存键</param>
        void RemoveEntry(TKey key);
    }

    /// <summary>
    /// 剪贴板管理服务接口
    /// </summary>
    public interface IClipboardManagerService
    {
        /// <summary>
        /// 设置数据
        /// </summary>
        /// <param name="controls">控件视图模型</param>
        void SetData(IEnumerable<ICommonControlViewModel> controls);
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="parentPage">父级项目页面</param>
        /// <returns>控件视图模型</returns>
        ICollection<ICommonControlViewModel> GetData(IProjectPageViewModel parentPage);
        /// <summary>
        /// 是否有数据
        /// </summary>
        /// <returns></returns>
        bool HasData();
    }


    /// <summary>
    /// 页面构建服务接口
    /// </summary>
    public interface IPageBuilderService
    {
        TPage AddPage<TPage>(IExplorerNodeViewModel parent, string name) where TPage : IProjectPageViewModel;
    }
    /// <summary>
    /// 页面子控件服务接口
    /// </summary>
    public interface IPageChildrenService : IPageService
    {
        void AddControlFromDragDropOperation(System.Windows.IDataObject dragDropData, System.Windows.Point dropPosition);
    }
    /// <summary>
    /// 页面选择服务接口
    /// </summary>
    public interface IPageSelectionService
    {
        bool HasMultipleSelection
        {
            get;
        }
        void Attach(IProjectPageViewModel page);
        void SelectSingleChild(ICommonControlViewModel child);
        void DeselectAllChildrenAndSelectPage();
        void UpdateContainerSelectionStatus();
        void AddControlToSelection(ICommonControlViewModel control);
        void RemoveControlFromSelection(ICommonControlViewModel control);
        void ToggleItemSelection(ICommonControlViewModel item, bool isControlPressed);
        ISelectionControlViewModel GetSelectionViewModel();
        void RemoveSelectionViewModel();
        ICommonControlViewModel CreateGroupFromSelection();
        void SelectItems(ICollection<ICommonControlViewModel> selectedItems);
    }

    /// <summary>
    /// 页面服务接口
    /// </summary>
    public interface IPageService
    {
        /// <summary>
        /// 附加到页面
        /// </summary>
        /// <param name="page"></param>
        void AttachToPage(IProjectPageViewModel page);
    }
    /// <summary>
    /// 页面撤销重做服务接口
    /// </summary>
    public interface IPageUndoRedoService
    {
        bool AutoSelectPageAfterUndoRedo
        {
            get;
            set;
        }
        void AttachToPageSelectionService(IPageSelectionService pageSelectionService);
    }

    /// <summary>
    /// 模板参数服务接口
    /// </summary>
    public interface ITemplateParametersService
    {
        bool IsActivePageTemplatePage
        {
            get;
        }
        IObservable<T> GetActivePageTemplateParameters<T>();
    }
    /// <summary>
    /// Z顺序服务接口
    /// </summary>
    public interface IZOrderService
    {
        void BringForward(IProjectPageViewModel page, IEnumerable<ICommonControlViewModel> selectedItems);
        void BringToFront(IProjectPageViewModel page, IEnumerable<ICommonControlViewModel> selectedItems);
        void SendBackward(IProjectPageViewModel page, IEnumerable<ICommonControlViewModel> selectedItems);
        void SendToBack(IProjectPageViewModel page, IEnumerable<ICommonControlViewModel> selectedItems);
    }
}
