// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Services;
using System.ComponentModel;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 集合项抽象类
    /// </summary>
    public abstract class CollectionItem : DisposableViewModel, ICollectionItem
    {
        private readonly IParameterReferenceMonitor parameterReferenceMonitor;

        private IExplorerNodeViewModel explorerNodeViewModel;
        /// <summary>
        /// 编辑器显示文本更改事件
        /// </summary>
        public event EventHandler<EventArgs>? EditorDisplayTextChanged;
        /// <summary>
        /// 父节点改变事件
        /// </summary>

        public event EventHandler<ValueChangedEventArgs<IExplorerNodeViewModel>>? ParentChanged;

        /// <summary>
        /// 父项目页面
        /// </summary>
        [Browsable(false)]
        public IProjectPageViewModel? ParentPage
        {
            get
            {
                for (IHierarchicalNode hierarchicalNode = this; hierarchicalNode != null; hierarchicalNode = hierarchicalNode.Parent)
                {
                    IProjectPageViewModel? projectPageViewModel = hierarchicalNode.Parent as IProjectPageViewModel;
                    if (projectPageViewModel != null)
                    {
                        return projectPageViewModel;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 父模板页面
        /// </summary>
        [Browsable(false)]
        public ITemplatePageViewModel? ParentTemplatePage
        {
            get
            {
                return this.ParentPage as ITemplatePageViewModel;
            }
        }
        /// <summary>
        /// 子节点集合
        /// </summary>
        [Browsable(false)]
        public IObservableCollection<IExplorerNodeViewModel>? Children
        {
            get
            {
                return null;
            }
        }
        /// <summary>
        /// 属性监视器
        /// </summary>
        [Browsable(false)]
        public IPropertyMonitor PropertyMonitor
        {
            get;
            set;
        }
        /// <summary>
        /// 显示文本
        /// </summary>
        [Browsable(false)]
        public string DisplayText
        {
            get
            {
                return this.ToString();
            }
        }
        /// <summary>
        /// 父节点
        /// </summary>
        [Browsable(false)]
        public IExplorerNodeViewModel Parent
        {
            get
            {
                return this.explorerNodeViewModel;
            }
            set
            {
                if (this.explorerNodeViewModel != value)
                {
                    ValueChangedEventArgs<IExplorerNodeViewModel> e = new ValueChangedEventArgs<IExplorerNodeViewModel>(this.explorerNodeViewModel, value);
                    this.explorerNodeViewModel = value;
                    this.ParentChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<IExplorerNodeViewModel>(() => this.Parent);
                }
            }
        }
        /// <summary>
        /// DI容器
        /// </summary>
        [Browsable(false)]
        public IServiceProvider Services
        {
            get;
            private set;
        }
        /// <summary>
        /// 父集合
        /// </summary>
        [Browsable(false)]
        public IEnumerable<ICollectionItem> ParentCollection
        {
            get;
            set;
        }
        /// <summary>
        /// 唯一标识符
        /// </summary>
        [Browsable(false)]
        public Guid Id
        {
            get;
            set;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="services"></param>
        /// <param name="parentNode"></param>
        public CollectionItem(IServiceProvider services, IExplorerNodeViewModel parentNode)
        {
            this.Parent = parentNode;
            this.Id = Guid.NewGuid();
            this.Services = services;
            // 创建属性监视器
            this.PropertyMonitor = services.Resolve<IPropertyMonitor>();
            this.PropertyMonitor.Start(this);
            // 创建参数引用监视器
            IParameterReferenceMonitorFactory parameterReferenceMonitorFactory = services.Resolve<IParameterReferenceMonitorFactory>();
            this.parameterReferenceMonitor = parameterReferenceMonitorFactory.CreateMonitor(this);
        }
        /// <summary>
        /// 是否为指定节点的后代节点
        /// </summary>
        /// <param name="expectedParent"></param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public bool IsDescendantOf(IExplorerNodeViewModel expectedParent)
        {
            throw new NotSupportedException();
        }
        /// <summary>
        /// 深度复制字段
        /// </summary>
        /// <param name="target"></param>
        public abstract void DeepCopyFields(ICollectionItem target);
        /// <summary>
        /// 克隆集合项
        /// </summary>
        /// <returns></returns>
        public virtual ICollectionItem Clone()
        {
            ICollectionItem collectionItem = this.CreateClone();
            collectionItem.PropertyMonitor.Stop();
            collectionItem.Id = this.Id;
            this.DeepCopyFields(collectionItem);
            return collectionItem;
        }
        /// <summary>
        /// 创建克隆
        /// </summary>
        /// <returns></returns>
        public abstract ICollectionItem CreateClone();
        /// <summary>
        /// 更新编辑器显示文本
        /// </summary>
        public void UpdateEditorDisplayText()
        {
            this.EditorDisplayTextChanged?.Invoke(this, new EventArgs());
            base.RaisePropertyChanged<string>(() => this.DisplayText);
        }
        /// <summary>
        /// 获取XAML表示
        /// </summary>
        /// <returns></returns>
        public abstract string GetXaml();

        /// <summary>
        /// 字符串表示
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return base.GetType().Name;
        }
        /// <summary>
        /// 是否相等
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (object.Equals(obj, null))
            {
                return false;
            }
            CollectionItem collectionItem = obj as CollectionItem;
            return collectionItem != null && collectionItem.Id == this.Id;
        }
        /// <summary>
        /// HashCode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }
        /// <summary>
        /// 释放托管资源
        /// </summary>
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            this.parameterReferenceMonitor.Dispose();
        }
    }
}
