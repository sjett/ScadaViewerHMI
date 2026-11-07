// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.Events;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Globalization;
using System.Reactive.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using ErrorTexts = HMI.Infrastructure.Presentation.Resources.ErrorTexts;
//using System.Diagnostics;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 节点视图模型基类
    /// </summary>
    public abstract class NodeViewModel : DisposableViewModel, IExplorerNodeViewModel
    {
        /// <summary>
        /// 子节点集合
        /// </summary>
        protected readonly ISynchronizedObservableCollection<IExplorerNodeViewModel> children = new SynchronizedObservableCollection<IExplorerNodeViewModel>(null, null);
        private readonly IDisposable disposable;
        private readonly INavigationTargetService navigationTargetService;// 导航目标服务
        private readonly IProjectNameService projectNameService;// 项目名称服务
        private readonly IUiScheduler _uiScheduler;
        private bool _isExpanded;
        private string _id;
        private bool _isInEditMode;
        private bool _isSelected;
        private IExplorerNodeViewModel parentNode;// 父节点

        private Brush _background = (Brush)new SolidColorBrush().GetAsFrozen();
        private Brush _foreground = (Brush)new SolidColorBrush(Colors.Black).GetAsFrozen();
        private bool _isBackgroundVisible;
        private bool _isForegroundVisible;

        /// <summary>
        /// 父节点更改事件
        /// </summary>
        public event EventHandler<ValueChangedEventArgs<IExplorerNodeViewModel>>? ParentChanged;
        /// <summary>
        /// 是否可编辑
        /// </summary>

        [Browsable(false)]
        public abstract bool IsEditable
        {
            get;
        }
        /// <summary>
        /// 事件聚合器
        /// </summary>
        [Browsable(false)]
        protected IEventAggregator EventAggregator
        {
            get;
            private set;
        }
        /// <summary>
        /// 属性监视器
        /// </summary>
        protected IPropertyMonitor PropertyMonitor
        {
            get;
            private set;
        }
        /// <summary>
        /// 子节点集合监视器
        /// </summary>
        protected ICollectionMonitor<IExplorerNodeViewModel> ChildrenMonitor
        {
            get;
            private set;
        }
        /// <summary>
        /// 是否处于编辑模式
        /// </summary>
        [Browsable(false)]
        public bool IsInEditMode
        {
            get
            {
                return this._isInEditMode;
            }
            set
            {
                if (this._isInEditMode != value)
                {
                    this._isInEditMode = value;
                    base.RaisePropertyChanged<bool>(() => this.IsInEditMode);
                }
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
        /// 父项目页面
        /// </summary>
        [Browsable(false)]
        public IProjectPageViewModel ParentPage
        {
            get
            {
                for (IExplorerNodeViewModel explorerNodeViewModel = this; explorerNodeViewModel != null; explorerNodeViewModel = explorerNodeViewModel.Parent)
                {
                    IProjectPageViewModel projectPageViewModel = explorerNodeViewModel.Parent as IProjectPageViewModel;
                    if (projectPageViewModel != null)
                    {
                        return projectPageViewModel;
                    }
                }
                return null;
            }
        }
        /// <summary>
        /// 所属项目
        /// </summary>
        [Browsable(false)]
        public abstract IProjectViewModel Project
        {
            get;
        }
        /// <summary>
        /// 子节点集合
        /// </summary>
        [Browsable(false)]
        public IObservableCollection<IExplorerNodeViewModel> Children
        {
            get
            {
                return this.children;
            }
        }
        /// <summary>
        /// 是否展开
        /// </summary>
        [Browsable(false)]
        public bool IsExpanded
        {
            get
            {
                return this._isExpanded;
            }
            set
            {
                if (value != this._isExpanded)
                {
                    this._isExpanded = value;
                    base.RaisePropertyChanged<bool>(() => this.IsExpanded);
                }
            }
        }
        /// <summary>
        /// 节点唯一标识符
        /// </summary>
        [Browsable(true), DisplayName("Name"), EnabledProperty("IsEditable"), SingleInstanceEditable, CustomMethodValidation("ValidateId")]
        public string ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if (value == null)
                {
                    return;
                }
                string b = this.projectNameService.RegisterAndGetUniqueName(this, value);
                if (this._id == b)
                {
                    return;
                }
                base.RaisePropertyChanging<string>(() => this.ID);
                this._id = b;
                base.RaisePropertyChanged<string>(() => this.ID);
            }
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        [Browsable(false)]
        public virtual bool IsSelected
        {
            get
            {
                return this._isSelected;
            }
            set
            {
                if (this._isSelected == value)
                {
                    return;
                }
                this._isSelected = value;
                //Debug.WriteLine($"IsSelected changed: {this.ID} -> {value}");
                //Debug.WriteLine(Environment.StackTrace);
                // 发布选中状态事件，以便其他组件响应（属性面板，项目树面板）
                this.EventAggregator.Publish<IsNodeSelectedChangedEvent, IExplorerNodeViewModel>(this);

                if (this._isSelected && this.parentNode != null)
                {
                    this.parentNode.IsExpanded = true;
                }
                base.RaisePropertyChanged<bool>(() => this.IsSelected);
            }
        }
        /// <summary>
        /// 节点路径
        /// </summary>
        [Browsable(false)]
        public string Path
        {
            get
            {
                string sep = "/";

                List<string> ids = [this.ID];
                var parent = this.Parent;
                while (parent != null)
                {
                    ids.Add(parent.ID);
                    parent = parent.Parent;
                }
                ids.Reverse();

                return string.Join(sep, ids);
            }
        }
        /// <summary>
        /// 父节点
        /// </summary>
        [Browsable(false)]
        public virtual IExplorerNodeViewModel Parent
        {
            get
            {
                return this.parentNode;
            }
            set
            {
                if (this.parentNode != value)
                {
                    ValueChangedEventArgs<IExplorerNodeViewModel> e = new ValueChangedEventArgs<IExplorerNodeViewModel>(this.parentNode, value);
                    base.RaisePropertyChanging<IExplorerNodeViewModel>(() => this.Parent);
                    this.parentNode = value;
                    this.ParentChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<IExplorerNodeViewModel>(() => this.Parent);
                }
            }
        }
        /// <summary>
        /// 背景色
        /// </summary>
        [EditorOrder(-255.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories)), VisibilityProperty("IsBackgroundVisible")]
        public Brush Background
        {
            get
            {
                return this._background;
            }
            set
            {
                if (this._background != value)
                {
                    base.RaisePropertyChanging<Brush>(() => this.Background);
                    this._background = value;
                    base.RaisePropertyChanged<Brush>(() => this.Background);
                }
            }
        }
        /// <summary>
        /// 前景色
        /// </summary>
        [EditorOrder(-254.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories)), VisibilityProperty("IsForegroundVisible")]
        public Brush Foreground
        {
            get
            {
                return this._foreground;
            }
            set
            {
                if (this._foreground != value)
                {
                    base.RaisePropertyChanging<Brush>(() => this.Foreground);
                    this._foreground = value;
                    base.RaisePropertyChanged<Brush>(() => this.Foreground);
                }
            }
        }
        /// <summary>
        /// 背景色是否可见
        /// </summary>
        [Browsable(false)]
        public bool IsBackgroundVisible
        {
            get
            {
                return this._isBackgroundVisible;
            }
            set
            {
                if (this._isBackgroundVisible != value)
                {
                    this._isBackgroundVisible = value;
                    base.RaisePropertyChanged<bool>(() => this.IsBackgroundVisible);
                }
            }
        }
        /// <summary>
        /// 前景色是否可见
        /// </summary>
        [Browsable(false)]
        public bool IsForegroundVisible
        {
            get
            {
                return this._isForegroundVisible;
            }
            set
            {
                if (this._isForegroundVisible != value)
                {
                    this._isForegroundVisible = value;
                    base.RaisePropertyChanged<bool>(() => this.IsForegroundVisible);
                }
            }
        }
        /// <summary>
        /// 验证节点唯一标识符
        /// </summary>
        /// <param name="id"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public ValidationResult ValidateId(string id, CultureInfo culture)
        {
            if (!this.projectNameService.IsValidName(id))
            {
                string errorContent = string.Format(ErrorTexts.InvalidName, id);
                return new ValidationResult(false, errorContent);
            }
            if (this.projectNameService.IsNameInUse(id, this))
            {
                string errorContent2 = string.Format(ErrorTexts.NameAlreadyExists, id);
                return new ValidationResult(false, errorContent2);
            }
            return ValidationResult.ValidResult;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container"></param>
        /// <param name="parentValue"></param>
        protected NodeViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            this.EventAggregator = container.Resolve<IEventAggregator>();
            this.ChildrenMonitor = container.Resolve<ICollectionMonitor<IExplorerNodeViewModel>>();
            this.projectNameService = container.Resolve<IProjectNameService>();
            this.navigationTargetService = container.Resolve<INavigationTargetService>();
            this._uiScheduler = container.Resolve<IUiScheduler>();
            // 启动子节点集合监视器
            this.ChildrenMonitor.Start(this.Children);
            this.parentNode = parentValue;
            // 启动属性监视器
            this.PropertyMonitor = container.Resolve<IPropertyMonitor>();
            this.PropertyMonitor.Start(this);

            // 子节点集合更改事件处理
            var observableChildren = Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                addHandler => this.children.CollectionChanged += addHandler,
                removeHandler => this.children.CollectionChanged -= removeHandler);

            this.disposable = observableChildren
                                .Select(ep => ep.EventArgs)
                                .Do(this.UpdateChildNames) //1.后台线程处理
                                .ObserveOn(this._uiScheduler)//2.切回 UI 线程
                                .Do(this.UpdateNavigationTargets)
                                .Subscribe<NotifyCollectionChangedEventArgs>();


            if (this.Background != null && this.Background.CanFreeze)
            {
                this.Background.Freeze();// 冻结画刷以提高性能
            }
            if (this.Foreground != null && this.Foreground.CanFreeze)
            {
                this.Foreground.Freeze();// 冻结画刷以提高性能
            }
        }

        /// <summary>
        /// 验证是否为有效的拖放目标
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public abstract bool IsValidDropTarget(IEnumerable<object> payload);
        /// <summary>
        /// 是否为指定节点的后代节点
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public bool IsDescendantOf(IExplorerNodeViewModel parentNode)
        {
            for (IExplorerNodeViewModel explorerNodeViewModel = this; explorerNodeViewModel != null; explorerNodeViewModel = explorerNodeViewModel.Parent)
            {
                if (explorerNodeViewModel == parentNode)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 更新子节点名称
        /// </summary>
        /// <param name="e"></param>
        protected virtual void UpdateChildNames(NotifyCollectionChangedEventArgs e)
        {
            NotifyCollectionChangedAction action = e.Action;
            if (action != NotifyCollectionChangedAction.Remove && action != NotifyCollectionChangedAction.Reset)
            {
                return;
            }
            IExplorerNodeViewModel[] removedItems = (e.OldItems ?? new ArrayList()).OfType<IExplorerNodeViewModel>().ToArray<IExplorerNodeViewModel>();
            this.projectNameService.RemoveNames(removedItems);
        }
        /// <summary>
        /// 更新导航目标
        /// </summary>
        private void UpdateNavigationTargets(NotifyCollectionChangedEventArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.navigationTargetService.UpdateNavigationTargets((eventArgs.NewItems ?? new ArrayList()).OfType<IExplorerNodeViewModel>(), null);
                    return;
                case NotifyCollectionChangedAction.Remove:
                case NotifyCollectionChangedAction.Reset:
                    {
                        IExplorerNodeViewModel[] removedItems = (eventArgs.OldItems ?? new ArrayList()).OfType<IExplorerNodeViewModel>().ToArray<IExplorerNodeViewModel>();
                        this.navigationTargetService.UpdateNavigationTargets(null, removedItems);
                        break;
                    }
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                    break;
                default:
                    return;
            }
        }
        /// <summary>
        /// 释放托管资源
        /// </summary>
        protected override void DisposeManagedResources()
        {
            foreach (IExplorerNodeViewModel current in this.Children)
            {
                current.Dispose();
            }

            this.disposable.Dispose();
            base.DisposeManagedResources();
        }
    }
}
