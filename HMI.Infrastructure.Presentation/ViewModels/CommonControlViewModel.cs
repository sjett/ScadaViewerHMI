// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Linq.Expressions;
using System.Reactive.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Globalization;
using System.Net;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    /// <summary>
    /// 公共控件视图模型基类
    /// </summary>
    public class CommonControlViewModel : NodeViewModel, ICommonControlViewModel
    {
        private readonly List<IDisposable> disposeSignalWatchers = new List<IDisposable>();
        private readonly IParameterReferenceMonitorFactory parameterReferenceMonitorFactory;
        private readonly ISignalResolverService signalResolverService;
        private bool initControlHeight;
        private bool initControlWidth;
        private StringBuilder _xamlInlinePropertiesBuilder = new StringBuilder();
        private readonly ObservableCollection<ITemplatePageViewModel> _templatePages = new ObservableCollection<ITemplatePageViewModel>();
        private IParameterReferenceMonitor parameterReferenceMonitor;
        private Guid _controlID = Guid.NewGuid();
        private double _xTranslation;
        private double _yTranslation;
        private double _angle;
        private double _opacity = 1.0;
        private string _toolTip;
        private bool _showTemplate;
        private DialogModality dialogModality;
        private ITemplatePageViewModel _templateSource;
        private string _templateTitle;
        private readonly ObservableCollection<IParameterValue> _parameterValues = new ObservableCollection<IParameterValue>();
        private int _zIndex;
        private double _controlWidth;
        private double _controlHeight;
        private bool _isTemplateEnabled;
        private bool _isShowTemplateEnabled = true;
        private double _width;
        private double _height;
        private bool _showSelectionPreview;
        private bool _showRotationDecorator;
        private bool _showDragDecorator = true;
        private bool _showResizeDecorator;
        private bool _showSelectionDecorator;
        private IDisposable projectChildrenChangedSubscription;

        public event EventHandler<ValueChangedEventArgs<Guid>>? IdChanged;

        public event EventHandler<ValueChangedEventArgs<double>>? XTranslationChanged;

        public event EventHandler<ValueChangedEventArgs<double>>? YTranslationChanged;

        public event EventHandler<ValueChangedEventArgs<double>> AngleChanged;

        public event EventHandler<ValueChangedEventArgs<double>> OpacityChanged;

        public event EventHandler<ValueChangedEventArgs<string>> ToolTipChanged;

        public event EventHandler<ValueChangedEventArgs<bool>> ShowTemplateChanged;

        public event EventHandler<ValueChangedEventArgs<DialogModality>> TemplateModalityChanged;

        public event EventHandler<ValueChangedEventArgs<ITemplatePageViewModel>> TemplateSourceChanged;

        public event EventHandler<ValueChangedEventArgs<string>> TemplateTitleChanged;

        public event EventHandler<ValueChangedEventArgs<int>> ZIndexChanged;

        public event EventHandler<ValueChangedEventArgs<bool>> IsTemplateEnabledChanged;

        public event EventHandler<ValueChangedEventArgs<bool>> IsShowTemplateEnabledChanged;

        public event EventHandler<ValueChangedEventArgs<bool>> ShowSelectionPreviewChanged;

        public event EventHandler<ValueChangedEventArgs<bool>> ShowRotationDecoratorChanged;

        public event EventHandler<ValueChangedEventArgs<bool>> ShowDragDecoratorChanged;

        public event EventHandler<ValueChangedEventArgs<bool>> ShowResizeDecoratorChanged;

        public event EventHandler FocusRequested;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="container">IServiceProvider </param>
        /// <param name="parentValue">IExplorerNodeViewModel</param>
        /// <param name="controlType">Type</param>
        public CommonControlViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue)
        {
            Contract.Requires<ArgumentNullException>(container != null);

            this.Services = container;
            this.ControlType = controlType;
            this.signalResolverService = container.Resolve<ISignalResolverService>();
            this.ActionMonitor = container.Resolve<IActionMonitor>();

            base.ParentChanged += this.onParentChanged;

            addCollectionChangedEvent();
            resetPropertyChangedEvent(null, this.Parent);
            initTemplatePages();

            this.ShowHideProperties();

            this.TemplateSourceChanged += this.onTemplateSourceChanged;

            this.parameterReferenceMonitorFactory = container.Resolve<IParameterReferenceMonitorFactory>();
            this.parameterReferenceMonitor = this.parameterReferenceMonitorFactory.CreateMonitor(this);
        }
        void addCollectionChangedEvent()
        {
            if (Project == null)
            {
                return;
            }

            var children = Project.Children; // 缓存引用，避免闭包访问 this.Project
            var projectChildrenChanges = Observable
                .FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
                addHandler => children!.CollectionChanged += addHandler, 
                removeHandler => children!.CollectionChanged -= removeHandler);
            
            this.projectChildrenChangedSubscription = projectChildrenChanges
                                .Select(change => change.EventArgs)
                                .Do(new Action<NotifyCollectionChangedEventArgs>(OnProjectChildrenChanged))
                                .Subscribe<NotifyCollectionChangedEventArgs>();

        }

        void resetPropertyChangedEvent(IExplorerNodeViewModel oldParent, IExplorerNodeViewModel newParent)
        {
            if (oldParent != null)
            {
                oldParent.PropertyChanged -= new PropertyChangedEventHandler(onPropertyChanged);
            }
            if (newParent != null)
            {
                newParent.PropertyChanged += new PropertyChangedEventHandler(onPropertyChanged);
            }
        }
        void initTemplatePages()
        {
            if (this.Project == null)
            {
                return;
            }
            this.ResetTemplatePages(this.Project.Children!.ChildTemplatePages());
        }
        /// <summary>
        /// 模板页面集合
        /// </summary>
        [Browsable(false)]
        public ObservableCollection<ITemplatePageViewModel> TemplatePages
        {
            get
            {
                return this._templatePages;
            }
        }
        /// <summary>
        /// 项目视图模型
        /// </summary>
        [Browsable(false)]
        public override IProjectViewModel Project
        {
            get
            {
                if (this.Parent != null)
                {
                    return this.Parent.Project;
                }
                return null;
            }
        }
        /// <summary>
        /// 是否可编辑
        /// </summary>
        [Browsable(false)]
        public override bool IsEditable
        {
            get
            {
                return true;
            }
        }
        /// <summary>
        /// 控件唯一标识符
        /// </summary>
        [Browsable(false)]
        public Guid ControlID
        {
            get
            {
                return this._controlID;
            }
            set
            {
                if (this._controlID != value)
                {
                    ValueChangedEventArgs<Guid> e = new ValueChangedEventArgs<Guid>(this._controlID, value);
                    this._controlID = value;
                    this.IdChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<Guid>(() => this.ControlID);
                }
            }
        }
        /// <summary>
        /// X平移
        /// </summary>
        [EditorOrder(-238.0), NumberEditor(false, Precision = 0), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
        public double XTranslation
        {
            get
            {
                return this._xTranslation;
            }
            set
            {
                if (this._xTranslation != value)
                {
                    ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this._xTranslation, value);
                    base.RaisePropertyChanging<double>(() => this.XTranslation);
                    this._xTranslation = value;
                    base.RaisePropertyChanged<double>(() => this.XTranslation);
                    this.XTranslationChanged?.Invoke(this, e);
                }
            }
        }
        /// <summary>
        /// Y平移
        /// </summary>
        [EditorOrder(-237.0), NumberEditor(false, Precision = 0), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
        public double YTranslation
        {
            get
            {
                return this._yTranslation;
            }
            set
            {
                if (this._yTranslation != value)
                {
                    ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this._yTranslation, value);
                    base.RaisePropertyChanging<double>(() => this.YTranslation);
                    this._yTranslation = value;
                    base.RaisePropertyChanged<double>(() => this.YTranslation);
                    this.YTranslationChanged?.Invoke(this, e);
                }
            }
        }
        /// <summary>
        /// 旋转角度
        /// </summary>
        [EditorOrder(-236.0), PropertyCategory("Layout", typeof(StandardPropertyCategories))]
        public double Angle
        {
            get
            {
                return this._angle;
            }
            set
            {
                if (this._angle != value)
                {
                    ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this._angle, value);
                    base.RaisePropertyChanging<double>(() => this.Angle);
                    this._angle = value;
                    base.RaisePropertyChanged<double>(() => this.Angle);
                    this.AngleChanged?.Invoke(this, e);
                }
            }
        }
        /// <summary>
        /// 不透明度
        /// </summary>
        [EditorOrder(-252.0), OpacityEditor, PropertyCategory("Appearance", typeof(StandardPropertyCategories)), MaximumValue(1.0), MinimumValue(0.0)]
        public double Opacity
        {
            get
            {
                return this._opacity;
            }
            set
            {
                if (this._opacity != value)
                {
                    ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this._opacity, value);
                    base.RaisePropertyChanging<double>(() => this.Opacity);
                    this._opacity = value;
                    this.OpacityChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<double>(() => this.Opacity);
                }
            }
        }
        /// <summary>
        /// 工具提示
        /// </summary>
        [EditorOrder(-251.0), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
        public string ToolTip
        {
            get
            {
                return this._toolTip;
            }
            set
            {
                if (this._toolTip != value)
                {
                    ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this._toolTip, value);
                    base.RaisePropertyChanging<string>(() => this.ToolTip);
                    this._toolTip = value;
                    this.ToolTipChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<string>(() => this.ToolTip);
                }
            }
        }
        /// <summary>
        /// 实际宽度
        /// </summary>
        [DisplayName("Width"), EditorOrder(-240.0), NumberEditor(false, Precision = 0), PropertyCategory("Layout", typeof(StandardPropertyCategories)), MinimumValue(0.0)]
        public double ActualWidth
        {
            get
            {
                return this.ControlWidth;
            }
            set
            {
                double x = value - this.ActualWidth;
                this.ControlWidth = value;
                Vector sizeChange = new Vector(x, 0.0);
                this.AdjustPositionForResize(sizeChange);
            }
        }
        /// <summary>
        /// 实际高度
        /// </summary>
        [DisplayName("Height"), EditorOrder(-239.0), NumberEditor(false, Precision = 0), PropertyCategory("Layout", typeof(StandardPropertyCategories)), MinimumValue(0.0)]
        public double ActualHeight
        {
            get
            {
                return this.ControlHeight;
            }
            set
            {
                double y = value - this.ActualHeight;
                this.ControlHeight = value;
                Vector sizeChange = new Vector(0.0, y);
                this.AdjustPositionForResize(sizeChange);
            }
        }
        /// <summary>
        /// 显示模板
        /// </summary>
        [Browsable(true), DisplayName("ShowTemplate"), PropertyCategory("Template", typeof(StandardPropertyCategories)), VisibilityProperty("AreTemplateControlsAndShowTemplateVisible")]
        public bool ShowTemplate
        {
            get
            {
                return this._showTemplate;
            }
            set
            {
                if (this._showTemplate != value)
                {
                    ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this._showTemplate, value);
                    base.RaisePropertyChanging<bool>(() => this.ShowTemplate);
                    this._showTemplate = value;
                    this.ShowTemplateChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<bool>(() => this.ShowTemplate);
                }
            }
        }
        /// <summary>
        /// 模板模态
        /// </summary>
        [Browsable(true), DisplayName("TemplateModality"), PropertyCategory("Template", typeof(StandardPropertyCategories)), VisibilityProperty("AreTemplateControlsVisible")]
        public DialogModality TemplateModality
        {
            get
            {
                return this.dialogModality;
            }
            set
            {
                if (this.dialogModality != value)
                {
                    ValueChangedEventArgs<DialogModality> e = new ValueChangedEventArgs<DialogModality>(this.dialogModality, value);
                    base.RaisePropertyChanging<DialogModality>(() => this.TemplateModality);
                    this.dialogModality = value;
                    this.TemplateModalityChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<DialogModality>(() => this.TemplateModality);
                }
            }
        }
        /// <summary>
        /// 模板源
        /// </summary>
        [Browsable(true), DisplayName("Template"), OptionListEditor("TemplatePages", IsUnselectable = true), PropertyCategory("Template", typeof(StandardPropertyCategories)), VisibilityProperty("AreTemplateControlsVisible")]
        public ITemplatePageViewModel TemplateSource
        {
            get
            {
                return this._templateSource;
            }
            set
            {
                if (this._templateSource != value)
                {
                    ValueChangedEventArgs<ITemplatePageViewModel> e = new ValueChangedEventArgs<ITemplatePageViewModel>(this._templateSource, value);
                    base.RaisePropertyChanging<ITemplatePageViewModel>(() => this.TemplateSource);
                    this._templateSource = value;
                    this.TemplateSourceChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<ITemplatePageViewModel>(() => this.TemplateSource);
                }
            }
        }
        /// <summary>
        /// 模板标题
        /// </summary>
        [Browsable(true), DisplayName("TemplateTitle"), PropertyCategory("Template", typeof(StandardPropertyCategories)), VisibilityProperty("AreTemplateControlsVisible")]
        public string TemplateTitle
        {
            get
            {
                return this._templateTitle;
            }
            set
            {
                if (this._templateTitle != value)
                {
                    ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this._templateTitle, value);
                    base.RaisePropertyChanging<string>(() => this.TemplateTitle);
                    this._templateTitle = value;
                    this.TemplateTitleChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<string>(() => this.TemplateTitle);
                }
            }
        }
        /// <summary>
        /// 参数值集合
        /// </summary>
        [CollectionEditor(typeof(IParameterValue), IsCollectionReadOnly = true, UseHorizontalEditing = true), 
            PropertyCategory("Template", typeof(StandardPropertyCategories)),
            VisibilityProperty("AreTemplateControlsVisible")]
        public ObservableCollection<IParameterValue> ParameterValues
        {
            get
            {
                return this._parameterValues;
            }
        }
        /// <summary>
        /// XAML内联属性构建器
        /// </summary>
        [Browsable(false)]
        public StringBuilder XamlInlinePropertiesBuilder
        {
            get
            {
                return this._xamlInlinePropertiesBuilder;
            }
            set
            {
                this._xamlInlinePropertiesBuilder = value;
            }
        }
        /// <summary>
        /// Services
        /// </summary>
        [Browsable(false)]
        public IServiceProvider Services
        {
            get;
            private set;
        }
        /// <summary>
        /// ZIndex
        /// </summary>
        [Browsable(false)]
        public int ZIndex
        {
            get
            {
                return this._zIndex;
            }
            set
            {
                if (this._zIndex != value)
                {
                    ValueChangedEventArgs<int> e = new ValueChangedEventArgs<int>(this._zIndex, value);
                    base.RaisePropertyChanging<int>(() => this.ZIndex);
                    this._zIndex = value;
                    this.ZIndexChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<int>(() => this.ZIndex);
                }
            }
        }
        /// <summary>
        /// 控件宽度
        /// </summary>
        [Browsable(false), MinimumValue(0.0)]
        public double ControlWidth
        {
            get
            {
                return this._controlWidth;
            }
            set
            {
                if (this._controlWidth != value)
                {
                    base.RaisePropertyChanging<double>(() => this.ControlWidth);
                    this._controlWidth = value;
                    base.RaisePropertyChanged<double>(() => this.ControlWidth);
                }
            }
        }
        /// <summary>
        /// 控件高度
        /// </summary>
        [Browsable(false), MinimumValue(0.0)]
        public double ControlHeight
        {
            get
            {
                return this._controlHeight;
            }
            set
            {
                if (this._controlHeight != value)
                {
                    base.RaisePropertyChanging<double>(() => this.ControlHeight);
                    this._controlHeight = value;
                    base.RaisePropertyChanged<double>(() => this.ControlHeight);
                }
            }
        }
        /// <summary>
        /// 是否托管在模板上
        /// </summary>
        [Browsable(false)]
        public bool IsHostedOnTemplate
        {
            get
            {
                return this.Parent is ITemplatePageViewModel;
            }
        }
        /// <summary>
        /// 是否托管在页面上
        /// </summary>
        [Browsable(false)]
        public bool IsHostedOnPage
        {
            get
            {
                return this.Parent is IProjectPageViewModel && !this.IsHostedOnTemplate;
            }
        }
        /// <summary>
        /// 是否为组成员
        /// </summary>
        [Browsable(false)]
        public bool IsGroupMember
        {
            get
            {
                return this.Parent is IControlGroupContainerViewModel;
            }
        }
        /// <summary>
        /// 是否启用模板
        /// </summary>
        [Browsable(false)]
        public bool IsTemplateEnabled
        {
            get
            {
                return this._isTemplateEnabled;
            }
            set
            {
                if (this._isTemplateEnabled != value)
                {
                    ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this._isTemplateEnabled, value);
                    this._isTemplateEnabled = value;
                    this.IsTemplateEnabledChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<bool>(() => this.IsTemplateEnabled);
                    base.RaisePropertyChanged<bool>(() => this.AreTemplateControlsVisible);
                }
            }
        }
        /// <summary>
        /// 是否启用显示模板
        /// </summary>
        [Browsable(false)]
        public bool IsShowTemplateEnabled
        {
            get
            {
                return this._isShowTemplateEnabled;
            }
            set
            {
                if (this._isShowTemplateEnabled != value)
                {
                    ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this._isShowTemplateEnabled, value);
                    this._isShowTemplateEnabled = value;
                    this.IsShowTemplateEnabledChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<bool>(() => this.IsShowTemplateEnabled);
                }
            }
        }
        /// <summary>
        /// 模板控件是否可见
        /// </summary>
        [Browsable(false)]
        public bool AreTemplateControlsVisible
        {
            get
            {
                return this.IsTemplateEnabled && this.IsHostedOnPage;
            }
        }
        /// <summary>
        /// 模板控件和显示模板是否可见
        /// </summary>
        [Browsable(false)]
        public bool AreTemplateControlsAndShowTemplateVisible
        {
            get
            {
                return this.AreTemplateControlsVisible && this.IsShowTemplateEnabled;
            }
        }
        /// <summary>
        /// 宽度
        /// </summary>
        [Browsable(false), MinimumValue(0.0)]
        public double Width
        {
            get
            {
                return this._width;
            }
            set
            {
                if (this._width != value)
                {
                    base.RaisePropertyChanging<double>(() => this.Width);
                    this._width = value;
                    base.RaisePropertyChanged<double>(() => this.Width);
                    if (!this.initControlWidth)
                    {
                        this.ControlWidth = value;
                        this.initControlWidth = true;
                    }
                }
            }
        }
        /// <summary>
        /// 高度
        /// </summary>
        [Browsable(false), MinimumValue(0.0)]
        public double Height
        {
            get
            {
                return this._height;
            }
            set
            {
                if (this._height != value)
                {
                    base.RaisePropertyChanging<double>(() => this.Height);
                    this._height = value;
                    base.RaisePropertyChanged<double>(() => this.Height);
                    if (!this.initControlHeight)
                    {
                        this.ControlHeight = value;
                        this.initControlHeight = true;
                    }
                }
            }
        }
        /// <summary>
        /// 显示选择预览
        /// </summary>
        [Browsable(false)]
        public bool ShowSelectionPreview
        {
            get
            {
                return this._showSelectionPreview;
            }
            set
            {
                if (this._showSelectionPreview != value)
                {
                    ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this._showSelectionPreview, value);
                    this._showSelectionPreview = value;
                    this.ShowSelectionPreviewChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<bool>(() => this.ShowSelectionPreview);
                }
            }
        }
        /// <summary>
        /// 显示旋转装饰器
        /// </summary>
        [Browsable(false)]
        public virtual bool ShowRotationDecorator
        {
            get
            {
                return this._showRotationDecorator;
            }
            set
            {
                if (this._showRotationDecorator != value)
                {
                    ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this._showRotationDecorator, value);
                    this._showRotationDecorator = value;
                    this.ShowRotationDecoratorChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<bool>(() => this.ShowRotationDecorator);
                }
            }
        }
        /// <summary>
        /// 显示拖动装饰器
        /// </summary>
        [Browsable(false)]
        public virtual bool ShowDragDecorator
        {
            get
            {
                return this._showDragDecorator;
            }
            set
            {
                if (this._showDragDecorator != value)
                {
                    ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this._showDragDecorator, value);
                    this._showDragDecorator = value;
                    this.ShowDragDecoratorChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<bool>(() => this.ShowDragDecorator);
                }
            }
        }
        /// <summary>
        /// 显示调整大小装饰器
        /// </summary>
        [Browsable(false)]
        public virtual bool ShowResizeDecorator
        {
            get
            {
                return this._showResizeDecorator;
            }
            set
            {
                if (this._showResizeDecorator != value)
                {
                    ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this._showResizeDecorator, value);
                    this._showResizeDecorator = value;
                    this.ShowResizeDecoratorChanged?.Invoke(this, e);
                    base.RaisePropertyChanged<bool>(() => this.ShowResizeDecorator);
                }
            }
        }
        /// <summary>
        /// 显示选择装饰器
        /// </summary>
        [Browsable(false)]
        public virtual bool ShowSelectionDecorator
        {
            get
            {
                return this._showSelectionDecorator;
            }
            set
            {
                if (this._showSelectionDecorator != value)
                {
                    this._showSelectionDecorator = value;
                    base.RaisePropertyChanged<bool>(() => this.ShowSelectionDecorator);
                }
            }
        }
        /// <summary>
        /// 缩放比例
        /// </summary>
        [Browsable(false)]
        public double Scale
        {
            get
            {
                IProjectPageViewModel? projectPageViewModel = this.Parent as IProjectPageViewModel;
                if (projectPageViewModel != null)
                {
                    return projectPageViewModel.Scale;
                }
                return 1.0;
            }
        }
        /// <summary>
        /// 反向缩放比例
        /// </summary>
        [Browsable(false)]
        public double InverseScale
        {
            get
            {
                IProjectPageViewModel? projectPageViewModel = this.Parent as IProjectPageViewModel;
                if (projectPageViewModel != null)
                {
                    return projectPageViewModel.InverseScale;
                }
                return 1.0;
            }
        }
        /// <summary>
        /// 是否强制等比例调整大小
        /// </summary>
        [Browsable(false)]
        public virtual bool ForceProportionalResize
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 动作监视器
        /// </summary>
        [Browsable(false)]
        public IActionMonitor ActionMonitor
        {
            get;
            private set;
        }
        /// <summary>
        /// 控件类型
        /// </summary>
        [Browsable(false)]
        public Type ControlType
        {
            get;
            private set;
        }
        /// <summary>
        /// 设计器所需大小
        /// </summary>
        [Browsable(false)]
        public virtual Size DesignerDesiredSize
        {
            get
            {
                return new Size(50.0, 50.0);
            }
        }
        /// <summary>
        /// 是否被选中
        /// </summary>
        [Browsable(false)]
        public override bool IsSelected
        {
            get
            {
                return base.IsSelected;
            }
            set
            {
                if (this.IsSelected == value)
                {
                    return;
                }
                base.IsSelected = value;
                this.UpdateDecoratorVisibility(value);
            }
        }
        /// <summary>
        ///     旋转
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="transformMode"></param>
        /// <param name="snapAngle"></param>
        public void Rotate(double angle, TransformMode transformMode, bool snapAngle = false)
        {
            double num = this.Angle;
            if (transformMode == TransformMode.Relative)
            {
                num += angle;
            }
            else
            {
                num = angle;
            }
            if (snapAngle)
            {
                num = Math.Round(num / 15.0) * 15.0;
            }
            this.Angle = num;
        }
        /// <summary>
        ///    平移
        /// </summary>
        /// <param name="horizontal"></param>
        /// <param name="vertical"></param>
        /// <param name="mode"></param>
        public void Translate(double horizontal, double vertical, TransformMode mode)
        {
            if (mode == TransformMode.Relative)
            {
                this.XTranslation += horizontal;
                this.YTranslation += vertical;
                return;
            }
            this.XTranslation = horizontal;
            this.YTranslation = vertical;
        }
        /// <summary>
        /// 获取命名空间
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetNamespaces()
        {
            return Enumerable.Empty<string>();
        }
        /// <summary>
        /// 获取所需程序集
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<AssemblyReference> GetRequiredAssemblies()
        {
            return Enumerable.Empty<AssemblyReference>();
        }
        /// <summary>
        /// 选择控件
        /// </summary>
        public void Select()
        {
            if (this.IsSelected)
            {
                return;
            }
            ISelectionContainer parentSelectionContainer = this.GetParentSelectionContainer();
            if (parentSelectionContainer == null)
            {
                return;
            }
            parentSelectionContainer.SetSelection(this);
        }
        /// <summary>
        /// 切换选择状态
        /// </summary>
        public void ToggleSelection()
        {
            ISelectionContainer parentSelectionContainer = this.GetParentSelectionContainer();
            if (parentSelectionContainer == null)
            {
                return;
            }
            if (this.IsSelected)
            {
                parentSelectionContainer.RemoveFromSelection(this);
                return;
            }
            parentSelectionContainer.AddToSelection(this);
        }
        /// <summary>
        /// 更新选择状态
        /// </summary>
        /// <param name="isShiftKeyPressed"></param>
        /// <param name="isControlKeyPressed"></param>
        public void UpdateSelection(bool isShiftKeyPressed, bool isControlKeyPressed)
        {
            ISelectionContainer parentSelectionContainer = this.GetParentSelectionContainer();
            if (parentSelectionContainer == null)
            {
                return;
            }
            if (isShiftKeyPressed)
            {
                parentSelectionContainer.AddToSelection(this);
                return;
            }
            parentSelectionContainer.UpdateSelection(this, isControlKeyPressed);
        }
        /// <summary>
        /// 更新装饰器
        /// </summary>
        public void UpdateDecorators()
        {
            this.UpdateDecoratorVisibility(this.IsSelected);
        }
        /// <summary>
        ///     变换开始
        /// </summary>
        public virtual void TransformationStarted()
        {
        }
        /// <summary>
        /// Silverlight 运行时控件参数
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public virtual IEnumerable<KeyValuePair<string, string>> GetSilverlightObjectParameters()
        {
            return new KeyValuePair<string, string>[0];
        }
        /// <summary>
        /// 请求焦点
        /// </summary>
        public void RequestFocus()
        {
            this.FocusRequested?.Invoke(this, new EventArgs());
        }
        private void onTemplateSourceChanged(object? sender, ValueChangedEventArgs<ITemplatePageViewModel> e)
        {
            if (e.OldValue != null)
            {
                e.OldValue.TemplateParameters.CollectionChanged -= this.OnTemplateParametersCollectionChanged;
            }
            if (e.NewValue != null)
            {
                e.NewValue.TemplateParameters.CollectionChanged += this.OnTemplateParametersCollectionChanged;
                this.ResetTemplateParameters();
                if (this.parameterReferenceMonitor == null)
                {
                    this.parameterReferenceMonitor = this.parameterReferenceMonitorFactory.CreateMonitor(this);
                }
            }
        }
        private void OnTemplateParametersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    AddTemplateParameters(e.NewItems);
                    return;
                case NotifyCollectionChangedAction.Remove:
                    this.RemoveTemplateParameters(e.OldItems!);
                    break;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.ResetTemplateParameters();
                    return;
                default:
                    return;
            }
        }
        private void AddTemplateParameters(System.Collections.IList newItems)
        {
            foreach (ParameterDefinition definition in newItems)
            {
                CreateParameterValue(definition);
            }
        }
        // 删除模板参数
        private void RemoveTemplateParameters(System.Collections.IList oldItems)
        {
            foreach(IParameterDefinition item in oldItems)
            {
                IParameterValue pv = this.ParameterValues.SingleOrDefault(t => t.Definition == item)!;
                this.ParameterValues.Remove(pv);
            }

        }
        private void ResetTemplateParameters()
        {
            IParameterValue[] existingParameterValues = this.ParameterValues.ToArray();
            this.ParameterValues.Clear();
            this.RestoreTemplateParameterValues(existingParameterValues);
        }
        private void RestoreTemplateParameterValues(IParameterValue[] existingParameterValues)
        {
            foreach (IParameterDefinition definition in this.TemplateSource.TemplateParameters)
            {
                AddTemplateParameterPreservingValue(existingParameterValues, definition);
            }
        }
        void AddTemplateParameterPreservingValue(IEnumerable<IParameterValue> existingValues, IParameterDefinition definition)
        {
            ParameterValue parameterValue = CreateParameterValue(definition);
            IParameterValue? existingValue = FindMatchingTemplateParameterValue(parameterValue, existingValues);
            if (existingValue != null)
            {
                parameterValue.Value = existingValue.Value;
            }
        }
        private ParameterValue CreateParameterValue(IParameterDefinition parameterDefinition)
        {
            ParameterValue parameterValue = new ParameterValue(parameterDefinition, this.Services, this);
            this.ParameterValues.Add(parameterValue);
            return parameterValue;
        }
        IParameterValue? FindMatchingTemplateParameterValue(ParameterValue parameterValue, IEnumerable<IParameterValue> existingValues)
        {
            return existingValues.SingleOrDefault(value => value.Name == parameterValue.Name &&
                value.Definition.GetType() == parameterValue.Definition.GetType());
        }
        /// <summary>
        /// 显示隐藏属性
        /// </summary>
        protected virtual void ShowHideProperties()
        {
            base.IsBackgroundVisible = true;
            base.IsForegroundVisible = true;
        }
        private void AddTemplatePages(IEnumerable<ITemplatePageViewModel> templatePages)
        {
            this.TemplatePages.AddRange(templatePages);
        }
        private void RemoveTemplatePages(IEnumerable<ITemplatePageViewModel> templatePages)
        {
            this.TemplatePages.RemoveRange(templatePages);
        }
        private void ResetTemplatePages(IEnumerable<ITemplatePageViewModel> templatePages)
        {
            this.TemplatePages.Clear();
            this.AddTemplatePages(templatePages);
        }
        private void OnProjectChildrenChanged(NotifyCollectionChangedEventArgs eventArgs)
        {
            List<ITemplatePageViewModel> emptyItems = new List<ITemplatePageViewModel>();
            IEnumerable<ITemplatePageViewModel> newTemplatePages = (eventArgs.NewItems ?? emptyItems).OfType<ITemplatePageViewModel>();
            IEnumerable<ITemplatePageViewModel> oldTemplatePages = (eventArgs.OldItems ?? emptyItems).OfType<ITemplatePageViewModel>();
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    this.AddTemplatePages(newTemplatePages);
                    return;
                case NotifyCollectionChangedAction.Remove:
                    this.RemoveTemplatePages(oldTemplatePages);
                    return;
                case NotifyCollectionChangedAction.Replace:
                case NotifyCollectionChangedAction.Move:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    this.ResetTemplatePages(newTemplatePages);
                    break;
                default:
                    return;
            }
        }
        /// <summary>
        /// 是否为有效的拖放目标
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public override bool IsValidDropTarget(IEnumerable<object> payload)
        {
            return false;
        }
        /// <summary>
        /// 释放托管资源
        /// </summary>
        protected override void DisposeManagedResources()
        {
            base.DisposeManagedResources();
            this.parameterReferenceMonitor.Dispose();
            foreach (IDisposable current in this.disposeSignalWatchers)
            {
                current.Dispose();
            }
            this.disposeSignalWatchers.Clear();

            if (this.projectChildrenChangedSubscription != null)
            {
                this.projectChildrenChangedSubscription.Dispose();
            }

            this.Parent = null;
        }
        private void onParentChanged(object? sender, ValueChangedEventArgs<IExplorerNodeViewModel> e)
        {
            IExplorerNodeViewModel oldValue = e.OldValue;
            IExplorerNodeViewModel newValue = e.NewValue;
            resetPropertyChangedEvent(oldValue, newValue);
        }
        void onPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            string? propertyName;
            if ((propertyName = e.PropertyName) != null)
            {
                if (!(propertyName == "Scale"))
                {
                    if (propertyName == "InverseScale")
                    {
                        base.RaisePropertyChanged<double>(() => this.InverseScale);
                    }
                }
                else
                {
                    base.RaisePropertyChanged<double>(() => this.Scale);
                }
            }
            base.RaisePropertyChanged<bool>(() => this.IsHostedOnTemplate);
            base.RaisePropertyChanged<bool>(() => this.IsHostedOnPage);
            base.RaisePropertyChanged<bool>(() => this.AreTemplateControlsVisible);
        }
        private void AdjustPositionForResize(Vector sizeChange)
        {
            sizeChange = new Vector(sizeChange.X / 2.0, sizeChange.Y / 2.0);
            sizeChange = GeometryService.GetRotationMatrix(this.Angle).Transform(sizeChange);
            this.XTranslation += sizeChange.X;
            this.YTranslation += sizeChange.Y;
        }
        /// <summary>
        /// 获取命名空间前缀
        /// </summary>
        /// <returns></returns>
        public string? GetNamespacePrefix()
        {
            string? fullNamespace = this.GetNamespaces().FirstOrDefault<string>();
            return this.GetNamespacePrefix(fullNamespace);
        }
        /// <summary>
        /// 获取命名空间前缀
        /// </summary>
        /// <param name="fullNamespace"></param>
        /// <returns></returns>
        public string? GetNamespacePrefix(string? fullNamespace)
        {
            if (fullNamespace != null && fullNamespace.Contains("="))
            {
                return fullNamespace.Substring(0, fullNamespace.IndexOf("=", StringComparison.InvariantCulture));
            }
            return fullNamespace;
        }
        /// <summary>
        /// 获取XAML控件名称
        /// </summary>
        /// <returns></returns>
        public virtual string GetXamlControlName()
        {
            return base.GetType().Name;
        }
        /// <summary>
        /// 获取父选择容器
        /// </summary>
        /// <returns></returns>
        protected ISelectionContainer? GetParentSelectionContainer()
        {
            IExplorerNodeViewModel parent = this.Parent;
            ISelectionContainer selectionContainer = parent as ISelectionContainer;
            while (parent != null && selectionContainer == null)
            {
                parent = parent.Parent;
                selectionContainer = (parent as ISelectionContainer);
            }
            return selectionContainer;
        }
        private void UpdateDecoratorVisibility(bool isSelected)
        {
            this.ShowSelectionDecorator = isSelected;
            this.ShowRotationDecorator = isSelected;
            this.ShowResizeDecorator = isSelected;
            this.ShowSelectionPreview = isSelected;
        }
        /// <summary>
        /// 注册已解析的信号名称属性
        /// </summary>
        /// <param name="signalProperty"></param>
        /// <param name="resolvedSignalProperty"></param>
        protected void RegisterResolvedSignalNameProperty(Expression<Func<string>> signalProperty, Expression<Func<string>> resolvedSignalProperty)
        {
            ISignalWatcher signalWatcher = this.signalResolverService.GetSignalWatcher(this, signalProperty, resolvedSignalProperty);
            signalWatcher.Start();
            this.disposeSignalWatchers.Add(signalWatcher);
        }

        #region xaml生成部分
        /// <summary>
        /// 获取XAML
        /// </summary>
        /// <returns></returns>
        public virtual string GetXaml()
        {
            string controlXaml = this.GetControlXaml();// 获取控件XAML
            return string.Format(CultureInfo.InvariantCulture, 
                @"
                <Grid
                    Canvas.Left=""{0}"" Canvas.Top=""{1}"" Canvas.ZIndex=""{6}""
                    Width=""{7}"" Height=""{8}""
                >
                            <Canvas
                                RenderTransformOrigin=""0.5,0.5""
                                HorizontalAlignment=""Center"" VerticalAlignment=""Center""
                                Width=""{3}"" Height=""{4}""
                            >
                                <Canvas.RenderTransform>
                                    <TransformGroup>
                                        <RotateTransform Angle=""{2}"" />
                                    </TransformGroup>
                                </Canvas.RenderTransform>
                                {5}
                            </Canvas>
                </Grid>
", 
                Math.Round(this.XTranslation),
                Math.Round(this.YTranslation),
                this.Angle,
                Math.Round(this.ControlWidth),
                Math.Round(this.ControlHeight),
                controlXaml,
                this.ZIndex,
                Math.Round(this.Width),
                Math.Round(this.Height));
        }
        /// <summary>
        /// 获取控件XAML
        /// </summary>
        /// <returns></returns>
        protected virtual string GetControlXaml()
        {
            StringBuilder xamlBuilder = new StringBuilder();
            StringBuilder childCollectionsBuilder = new StringBuilder();

            this._xamlInlinePropertiesBuilder = new StringBuilder();

            AppendControlXamlStartTag(xamlBuilder);
            this.AddXamlAttributes();
            xamlBuilder.Append(this.XamlInlinePropertiesBuilder);// 添加内联属性
            AppendClosingAngleBracket(xamlBuilder);

            this.AddXamlCollections(childCollectionsBuilder);
            xamlBuilder.AppendLine(childCollectionsBuilder.ToString());

            AppendControlXamlEndTag(xamlBuilder);

            return xamlBuilder.ToString();
        }
        /// <summary>
        /// 添加控件XAML开始标签
        /// </summary>
        /// <param name="xamlBuilder"></param>
        private void AppendControlXamlStartTag(StringBuilder xamlBuilder)
        {
            string? namespacePrefix = this.GetNamespacePrefix();
            string xamlControlName = this.GetXamlControlName();
            AppendControlXamlStartTag(xamlBuilder, namespacePrefix, xamlControlName);
        }
        /// <summary>
        /// 添加控件XAML开始标签
        /// </summary>
        /// <param name="xamlBuilder"></param>
        /// <param name="namespacePrefix"></param>
        /// <param name="xamlControlName"></param>
        private void AppendControlXamlStartTag(StringBuilder xamlBuilder, string? namespacePrefix, string xamlControlName)
        {
            if (!string.IsNullOrWhiteSpace(namespacePrefix))
            {
                namespacePrefix += ":";
            }
            xamlBuilder.AppendFormat("<{0}{1} ", namespacePrefix, xamlControlName);
        }
        /// <summary>
        /// 添加右尖括号
        /// </summary>
        /// <param name="xamlBuilder"></param>
        private void AppendClosingAngleBracket(StringBuilder xamlBuilder)
        {
            xamlBuilder.AppendLine(">");
        }
        /// <summary>
        /// 添加控件XAML结束标签
        /// </summary>
        /// <param name="xamlBuilder"></param>
        private void AppendControlXamlEndTag(StringBuilder xamlBuilder)
        {
            string? namespacePrefix = this.GetNamespacePrefix();
            string xamlControlName = this.GetXamlControlName();
            if (!string.IsNullOrWhiteSpace(namespacePrefix))
            {
                namespacePrefix += ":";
            }
            xamlBuilder.AppendFormat("</{0}{1}> ", namespacePrefix, xamlControlName);
        }

        /// <summary>
        /// 添加XAML属性
        /// </summary>
        protected virtual void AddXamlAttributes()
        {
            this.AddXamlAttribute("Canvas.Left", 0);
            this.AddXamlAttribute("Canvas.Top", 0);
            this.AddXamlAttribute("Height", Math.Round(this.ControlHeight));
            this.AddXamlAttribute("Width", Math.Round(this.ControlWidth));
            this.AddXamlAttribute("Name", base.ID);
            this.AddXamlAttribute("Opacity", this.Opacity.ToString(CultureInfo.InvariantCulture));

            if (this.IsTemplateEnabled)
            {
                if (this._isShowTemplateEnabled)
                {
                    this.AddXamlAttribute("ShowPopup", this.ShowTemplate);
                }
                this.AddXamlAttribute("PopupModality", this.TemplateModality);
                this.AddXamlAttribute("PopupSource", this.TemplateSource);
                this.AddXamlAttribute("PopupTitle", this.TemplateTitle);
            }
            if (!string.IsNullOrWhiteSpace(this.ToolTip))
            {
                this.AddXamlAttribute("ToolTipService.ToolTip", this.ToolTip);
            }
        }
        /// <summary>
        /// 添加XAML集合
        /// </summary>
        /// <param name="collectionXaml"></param>
        protected virtual void AddXamlCollections(StringBuilder collectionXaml)
        {
            if (this.IsTemplateEnabled)
            {
                this.AddXamlCollection(collectionXaml, "PopupParameters", this.ParameterValues, true);
            }
        }
        /// <summary>
        /// 添加XAML集合
        /// </summary>
        /// <param name="collectionXaml"></param>
        /// <param name="namespacePrefix"></param>
        /// <param name="definition"></param>
        /// <param name="propertyName"></param>
        /// <param name="collection"></param>
        protected void AddXamlCollection(StringBuilder collectionXaml, string namespacePrefix, string definition, string propertyName, IEnumerable<ICollectionItem> collection)
        {
            this.AddCollectionOpenElement(collectionXaml, namespacePrefix, definition, propertyName);
            foreach (ICollectionItem current in collection)
            {
                collectionXaml.AppendLine(current.GetXaml());
            }
            this.AddCollectionCloseElement(collectionXaml, namespacePrefix, definition, propertyName);
        }
        /// <summary>
        /// 添加XAML集合
        /// </summary>
        /// <param name="collectionXaml"></param>
        /// <param name="namespacePrefix"></param>
        /// <param name="propertyName"></param>
        /// <param name="collection"></param>
        protected void AddXamlCollection(StringBuilder collectionXaml, string namespacePrefix, string propertyName, IEnumerable<ICollectionItem> collection)
        {
            this.AddCollectionOpenElement(collectionXaml, namespacePrefix, propertyName);
            foreach (ICollectionItem current in collection)
            {
                collectionXaml.AppendLine(current.GetXaml());
            }
            this.AddCollectionCloseElement(collectionXaml, namespacePrefix, propertyName);
        }
        /// <summary>
        /// 添加XAML集合
        /// </summary>
        /// <param name="collectionXaml"></param>
        /// <param name="propertyName"></param>
        /// <param name="items"></param>
        /// <param name="isDefinitionUsed"></param>
        protected void AddXamlCollection(StringBuilder collectionXaml, string propertyName, IEnumerable<ICollectionItem> items, bool isDefinitionUsed = true)
        {
            string namespacePrefix = this.GetNamespacePrefix();
            if (isDefinitionUsed)
            {
                string xamlControlName = this.GetXamlControlName();
                this.AddXamlCollection(collectionXaml, namespacePrefix, xamlControlName, propertyName, items);
                return;
            }
            this.AddXamlCollection(collectionXaml, namespacePrefix, propertyName, items);
        }
        /// <summary>
        /// 添加集合元素结束标签
        /// </summary>
        /// <param name="collectionXaml"></param>
        /// <param name="namespacePrefix"></param>
        /// <param name="definition"></param>
        /// <param name="propertyName"></param>
        protected void AddCollectionCloseElement(StringBuilder collectionXaml, string namespacePrefix, string definition, string propertyName)
        {
            collectionXaml.AppendFormat("</{0}:{1}.{2}>{3}", new object[]
            {
                namespacePrefix,
                definition,
                propertyName,
                Environment.NewLine
            });
        }
        /// <summary>
        /// 添加集合元素开始标签
        /// </summary>
        /// <param name="collectionXaml"></param>
        /// <param name="namespacePrefix"></param>
        /// <param name="definition"></param>
        /// <param name="propertyName"></param>
        protected void AddCollectionOpenElement(StringBuilder collectionXaml, string namespacePrefix, string definition, string propertyName)
        {
            collectionXaml.AppendFormat("<{0}:{1}.{2}>{3}", new object[]
            {
                namespacePrefix,
                definition,
                propertyName,
                Environment.NewLine
            });
        }
        /// <summary>
        /// 添加集合元素结束标签
        /// </summary>
        /// <param name="collectionXaml"></param>
        /// <param name="namespacePrefix"></param>
        /// <param name="definition"></param>
        protected void AddCollectionCloseElement(StringBuilder collectionXaml, string namespacePrefix, string definition)
        {
            collectionXaml.AppendFormat("</{0}:{1}>{2}", namespacePrefix, definition, Environment.NewLine);
        }
        /// <summary>
        /// 添加集合元素开始标签
        /// </summary>
        /// <param name="collectionXaml"></param>
        /// <param name="namespacePrefix"></param>
        /// <param name="definition"></param>
        protected void AddCollectionOpenElement(StringBuilder collectionXaml, string namespacePrefix, string definition)
        {
            collectionXaml.AppendFormat("<{0}:{1}>{2}", namespacePrefix, definition, Environment.NewLine);
        }
        /// <summary>
        /// 添加XAML属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="xamlBuilder"></param>
        protected void AddXamlAttribute(string propertyName, double propertyValue, StringBuilder xamlBuilder)
        {
            this.AddXamlAttribute(propertyName, propertyValue.ToString(CultureInfo.InvariantCulture), xamlBuilder);
        }
        /// <summary>
        /// 添加XAML属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="xamlBuilder"></param>
        protected void AddXamlAttribute(string propertyName, int propertyValue, StringBuilder xamlBuilder)
        {
            this.AddXamlAttribute(propertyName, propertyValue.ToString(CultureInfo.InvariantCulture), xamlBuilder);
        }
        /// <summary>
        /// 添加XAML属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="inlinePropertiesBuilder"></param>
        protected void AddXamlAttribute(string propertyName, object propertyValue, StringBuilder inlinePropertiesBuilder)
        {
            string value = string.Empty;
            if (propertyValue != null && !string.IsNullOrWhiteSpace(propertyValue.ToString()))
            {
                ParameterDefinition parameterDefinition = propertyValue as ParameterDefinition;
                if (parameterDefinition != null)
                {
                    value = WebUtility.HtmlEncode(parameterDefinition.GetParameterDefinitionXamlReference() ?? string.Empty);
                }
                else
                {
                    value = WebUtility.HtmlEncode(propertyValue.ToString() ?? string.Empty);
                }
            }
            if (!string.IsNullOrWhiteSpace(value))
            {
                inlinePropertiesBuilder.Append(" ").Append(propertyName).Append("=\"").Append(value).Append("\"");
            }
        }
        /// <summary>
        /// 添加XAML属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        protected void AddXamlAttribute(string propertyName, double propertyValue)
        {
            this.AddXamlAttribute(propertyName, propertyValue.ToString(CultureInfo.InvariantCulture));
        }
        /// <summary>
        /// 添加XAML属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        protected void AddXamlAttribute(string propertyName, Freezable propertyValue)
        {
            object propertyValue2;
            if (propertyValue != null && propertyValue.CanFreeze)
            {
                propertyValue2 = propertyValue.GetAsFrozen();
            }
            else
            {
                propertyValue2 = propertyValue;
            }
            this.AddXamlAttribute(propertyName, propertyValue2);
        }
        /// <summary>
        /// 添加XAML属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        protected void AddXamlAttribute(string propertyName, int propertyValue)
        {
            this.AddXamlAttribute(propertyName, propertyValue.ToString(CultureInfo.InvariantCulture));
        }
        /// <summary>
        /// 添加XAML属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="xamlBuilder"></param>
        protected void AddXamlAttribute(string propertyName, DateTime propertyValue, StringBuilder xamlBuilder)
        {
            this.AddXamlAttribute(propertyName, propertyValue.ToString(CultureInfo.InvariantCulture), xamlBuilder);
        }
        /// <summary>
        /// 添加XAML属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        /// <param name="xamlBuilder"></param>
        protected void AddXamlAttribute(string propertyName, DateTime? propertyValue, StringBuilder xamlBuilder)
        {
            if (!propertyValue.HasValue)
            {
                return;
            }
            this.AddXamlAttribute(propertyName, propertyValue.Value.ToString(CultureInfo.InvariantCulture), xamlBuilder);
        }
        /// <summary>
        /// 添加XAML属性
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="propertyValue"></param>
        protected void AddXamlAttribute(string propertyName, object propertyValue)
        {
            this.AddXamlAttribute(propertyName, propertyValue, this.XamlInlinePropertiesBuilder);
        }
        #endregion

        /// <summary>
        /// 调整大小
        /// </summary>
        /// <param name="horizontalHandlePosition"></param>
        /// <param name="verticalHandlePosition"></param>
        /// <param name="dragX"></param>
        /// <param name="dragY"></param>
        public virtual void Resize(HorizontalAlignment horizontalHandlePosition, VerticalAlignment verticalHandlePosition, double dragX, double dragY)
        {
            double ptr1 = 0.0;
            double ptr2 = 0.0;
            double ptr3 = 0.0;
            double ptr4 = 0.0;
            if (horizontalHandlePosition == HorizontalAlignment.Left)
            {
                ptr1 = -dragX;
                ptr3 = dragX / 2.0;
            }
            if (horizontalHandlePosition == HorizontalAlignment.Right)
            {
                ptr1 = dragX;
                ptr3 = dragX / 2.0;
            }
            if (verticalHandlePosition == VerticalAlignment.Top)
            {
                ptr2 = -dragY;
                ptr4 = dragY / 2.0;
            }
            if (verticalHandlePosition == VerticalAlignment.Bottom)
            {
                ptr2 = dragY;
                ptr4 = dragY / 2.0;
            }
            this.ResizeContainerCanvas(horizontalHandlePosition, verticalHandlePosition, ptr1, ptr2);
            this.Translate(ptr3, ptr4, TransformMode.Relative);
        }
        /// <summary>
        /// 调整容器画布大小
        /// </summary>
        /// <param name="horizontalResizeEdge"></param>
        /// <param name="verticalResizeEdge"></param>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        protected virtual void ResizeContainerCanvas(HorizontalAlignment horizontalResizeEdge, VerticalAlignment verticalResizeEdge, double deltaX, double deltaY)
        {
            GeneralTransform generalTransform = new RotateTransform(this.Angle);
            if ((horizontalResizeEdge == HorizontalAlignment.Left && verticalResizeEdge == VerticalAlignment.Top) || (horizontalResizeEdge == HorizontalAlignment.Right && verticalResizeEdge == VerticalAlignment.Bottom))
            {
                generalTransform = generalTransform.Inverse;
            }
            Point point = new Point(deltaX, deltaY);
            point = generalTransform.Transform(point);
            double ptr1 = Math.Max(0.0, this.ControlWidth + point.X);
            double prt2 = Math.Max(0.0, this.ControlHeight + point.Y);
            this.ControlWidth = ptr1;
            this.ControlHeight = prt2;
        }
        /// <summary>
        /// 获取完整信号名称
        /// </summary>
        /// <param name="signalSuffix"></param>
        /// <returns></returns>
        public string GetFullSignalName(string signalSuffix)
        {
            return string.Format("{0}{1}", this.GetSignalPrefix(), signalSuffix ?? string.Empty);
        }
        /// <summary>
        /// 获取信号名称后缀
        /// </summary>
        /// <param name="fullSignalName"></param>
        /// <returns></returns>
        public string GetSignalSuffix(string fullSignalName)
        {
            if (fullSignalName == null)
            {
                return null;
            }
            string text = this.GetSignalPrefix() ?? string.Empty;
            if (!fullSignalName.StartsWith(text))
            {
                return fullSignalName;
            }
            return fullSignalName.Substring(text.Length);
        }
        /// <summary>
        /// 获取信号名称前缀
        /// </summary>
        /// <returns></returns>
        public string GetSignalPrefix()
        {
            return this.signalResolverService.GetResolvedSignalPrefix(this, base.ParentPage);
        }
    
    }
}
