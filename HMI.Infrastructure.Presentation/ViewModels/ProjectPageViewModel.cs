// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    public class ProjectPageViewModel : NodeViewModel, IProjectPageViewModel
	{
		private readonly IActionMonitor actionMonitor;
		private readonly IObservableCollectionGroup _layoutChildren;
		private readonly IPageChildrenService pageChildrenService;
		private readonly IPageSelectionService pageSelectionService;
		private readonly IPageUndoRedoService pageUndoRedoService;
		private readonly IZOrderService zOrderService;

		private static readonly Regex pageSizeRegex = new Regex("\\s*((\\d+\\.\\d+)|(\\d+))\\s*x\\s*((\\d+\\.\\d+)|(\\d+))\\s*.*", RegexOptions.Compiled);
		
		private string _size = "800x600 (4:3)";
		private bool _usePageNameAsSignalPrefix;
		private bool _lockSize;
		private double _scale = 1.0;
		private readonly ObservableCollection<double> _gridSizes = new ObservableCollection<double>();
		private double _gridSize = 50.0;
		private double _rulerSize = 50.0;
		private bool _stretchPages;
		private double _width = 800.0;
		private double _height = 600.0;
		private readonly ObservableCollection<string> _pageSizes = new ObservableCollection<string>(new string[]
		{
			"320x240 (4:3)",
			"640x480 (4:3)",
			"800x600 (4:3)",
			"960x540 (16:9)",
			"960x720 (4:3)",
			"1024x576 (16:9)",
			"1024x768 (4:3)",
			"1152x864 (4:3)",
			"1280x720 (16:9)",
			"1280x960 (4:3)",
			"1400x1050 (4:3)",
			"1440x1080 (4:3)",
			"1600x900 (16:9)",
			"1600x1200 (4:3)",
			"1920x1080 (16:9)"
		});
		private string _signalPrefix = string.Empty;

        [Browsable(false)]
		public override IProjectViewModel Project
		{
			get
			{
				if (Parent != null)
				{
					return Parent.Project;
				}
				return null;
			}
		}
        [Browsable(true), OptionListEditor("PageSizes", IsEditable = true), PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public string Size
		{
			get
			{
				return _size;
			}
			set
			{
				if (!_lockSize && _size != value && !string.IsNullOrEmpty(value))
				{
                    RaisePropertyChanging(() => Size);
                    _size = value;
                    UpdateDimensionsFromSize();
                    RaisePropertyChanged(() => Size);
				}
			}
		}
        [Browsable(true), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public bool UsePageNameAsSignalPrefix
		{
			get
			{
				return _usePageNameAsSignalPrefix;
			}
			set
			{
				if (_usePageNameAsSignalPrefix != value)
				{
                    RaisePropertyChanging(() => UsePageNameAsSignalPrefix);
                    _usePageNameAsSignalPrefix = value;
                    RaisePropertyChanged(() => UsePageNameAsSignalPrefix);
				}
			}
		}
        [Browsable(false)]
		public override bool IsEditable
		{
			get
			{
				return true;
			}
		}
        [Browsable(false)]
		public IControlFactory ControlFactory
		{
			get;
			private set;
		}
        [Browsable(false)]
		public IObservableCollectionGroup LayoutChildren
		{
			get
			{
				return _layoutChildren;
			}
		}
        [Browsable(false)]
		public IExplorerNodeViewModel SelectedItem
		{
			get
			{
				return Children.FirstOrDefault(t=> t.IsSelected);
			}
		}
        [Browsable(false)]
		public double Scale
		{
			get
			{
				return _scale;
			}
			set
			{
				if (_scale == value)
				{
					return;
				}
                _scale = value;

                RaisePropertyChanged(() => Scale);
                RaisePropertyChanged(() => InverseScale);
                RaisePropertyChanged(() => ScaledWidth);
                RaisePropertyChanged(() => ScaledHeight);
                RaisePropertyChanged(() => ScaledWidthInclRulers);
                RaisePropertyChanged(() => ScaledHeightInclRulers);
			}
		}
        [Browsable(false)]
		public double InverseScale
		{
			get
			{
				return 1.0 / Scale;
			}
		}
        [Browsable(false)]
		public double ScaledWidth
		{
			get
			{
				return Width * Scale;
			}
		}
        [Browsable(false)]
		public double ScaledHeight
		{
			get
			{
				return Height * Scale;
			}
		}
        [Browsable(false)]
		public double ScaledWidthInclRulers
		{
			get
			{
				return Width * Scale + 20.0;
			}
		}
        [Browsable(false)]
		public double ScaledHeightInclRulers
		{
			get
			{
				return Height * Scale + 20.0;
			}
		}
        [Browsable(false)]
		public ObservableCollection<double> GridSizes
		{
			get
			{
				return _gridSizes;
			}
		}
        [Browsable(false)]
		public double GridSize
		{
			get
			{
				return _gridSize;
			}
			set
			{
				if (_gridSize == value)
				{
					return;
				}
                _gridSize = value;
                RaisePropertyChanged(() => GridSize);
			}
		}
        [Browsable(false)]
		public double RulerSize
		{
			get
			{
				return _rulerSize;
			}
			set
			{
				if (_rulerSize != value)
				{
                    _rulerSize = value;
                    RaisePropertyChanged(() => RulerSize);
				}
			}
		}
        [Browsable(false)]
		public bool IsStretchPagesVisible
		{
			get
			{
				return Project != null && Parent == Project && !Project.StretchPages;
			}
		}
        [DisplayName("Stretch"), PropertyCategory("Appearance", typeof(StandardPropertyCategories)), VisibilityProperty("IsStretchPagesVisible")]
		public bool StretchPages
		{
			get
			{
				return _stretchPages;
			}
			set
			{
				if (_stretchPages != value)
				{
                    _stretchPages = value;
                    RaisePropertyChanged(() => StretchPages);
				}
			}
		}
        [Browsable(false)]
		public double Width
		{
			get
			{
				return _width;
			}
			set
			{
				if (_width == value)
				{
					return;
				}
                _width = value;
                RaisePropertyChanged(() => Width);
                RaisePropertyChanged(() => ScaledWidth);
                RaisePropertyChanged(() => ScaledWidthInclRulers);
			}
		}
        [Browsable(false)]
		public double Height
		{
			get
			{
				return _height;
			}
			set
			{
				if (_height == value)
				{
					return;
				}
                _height = value;
                RaisePropertyChanged(() => Height);
                RaisePropertyChanged(() => ScaledHeight);
                RaisePropertyChanged(() => ScaledHeightInclRulers);
			}
		}
        [Browsable(false)]
		public ObservableCollection<string> PageSizes
		{
			get
			{
				return _pageSizes;
			}
		}
        [Browsable(true), PropertyCategory("Signals", typeof(StandardPropertyCategories))]
		public string SignalPrefix
		{
			get
			{
				return _signalPrefix;
			}
			set
			{
				if (_signalPrefix != value)
				{
                    RaisePropertyChanging(() => SignalPrefix);
                    _signalPrefix = value;
                    RaisePropertyChanged(() => SignalPrefix);
				}
			}
		}
        [Browsable(false)]
		public IGuideSettingsViewModel GuideSettings
		{
			get;
			private set;
		}

		public ProjectPageViewModel(IServiceProvider container, IExplorerNodeViewModel parentNode, string name) : base(container, parentNode)
		{
            _layoutChildren = new ObservableCollectionGroup(this, t => !(t is IProjectPageViewModel) );

            GuideSettings = container.Resolve<IGuideSettingsViewModel>();
            actionMonitor = container.Resolve<IActionMonitor>();
            pageChildrenService = container.Resolve<IPageChildrenService>();
            pageSelectionService = container.Resolve<IPageSelectionService>();
            pageUndoRedoService = container.Resolve<IPageUndoRedoService>();
            zOrderService = container.Resolve<IZOrderService>();

            pageChildrenService.AttachToPage(this);
            pageSelectionService.Attach(this);
            pageUndoRedoService.AttachToPageSelectionService(pageSelectionService);
            ControlFactory = container.Resolve<IControlFactory>();

            Children.CollectionChanged += new NotifyCollectionChangedEventHandler(OnChildrenCollectionChanged);
            ID = name;
            IsBackgroundVisible = true;
            GenerateGridSizes();

			addStretchPagesChanged();
		}
        void addStretchPagesChanged()
        {
            if (Project == null)
            {
                return;
            }
            Project.StretchPagesChanged += new EventHandler<ValueChangedEventArgs<bool>>(OnStretchPagesChanged);
        }
        private void UpdateDimensionsFromSize()
		{
			Match match = pageSizeRegex.Match(Size);
			if (match.Groups.Count == 7)
			{
                Width = double.Parse(match.Groups[1].Value, NumberFormatInfo.InvariantInfo);
                Height = double.Parse(match.Groups[4].Value, NumberFormatInfo.InvariantInfo);
			}
		}
        /// <param name="isLocked"></param>
        public void LockSize(bool isLocked)
		{
            _lockSize = isLocked;
		}
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        public void SetSize(double newWidth, double newHeight)
		{
			newWidth = Math.Floor(newWidth);
			newHeight = Math.Floor(newHeight);
			if (Width != newWidth || Height != newHeight)
			{
                Size = string.Format(CultureInfo.InvariantCulture, "{0}x{1}", new object[]
				{
					newWidth,
					newHeight
				});
			}
		}
        /// <param name="dragDropData"></param>
        /// <param name="dropPosition"></param>
        public void DropControl(IDataObject dragDropData, Point dropPosition)
		{
            pageChildrenService.AddControlFromDragDropOperation(dragDropData, dropPosition);
		}
        /// <returns></returns>
        public ICollection<ICommonControlViewModel> GetSelectedItems()
		{
			ICollection<ICommonControlViewModel> result;
			lock (children.SyncRoot)
			{
				result = Children.Controls(t=> t.IsSelected).ToList();
			}
			return result;
		}
        /// <param name="items"></param>
        /// <param name="offsetIfOverlapping"></param>
        public void AddTopmostChildren(ICollection<ICommonControlViewModel> items, bool offsetIfOverlapping)
		{
			foreach (ICommonControlViewModel control in items)
			{
				control.ZIndex -= 1000000;
				RefreshControlId(control);
                AddTopmostChild(offsetIfOverlapping, control);
			}
            zOrderService.BringToFront(this, items);

			PublishDeserializationFinished();
		}
        void RefreshControlId(ICommonControlViewModel control)
        {
            control.ID = control.ID;
        }
        void PublishDeserializationFinished()
        {
            DeserializationFinishedEvent deserializationFinishedEvent = EventAggregator.GetEvent<DeserializationFinishedEvent>();
            if (deserializationFinishedEvent != null)
            {
                deserializationFinishedEvent.Publish(Project);
            }
        }

        void ISelectionContainer.SelectItems(ICollection<ICommonControlViewModel> controls)
		{
            pageSelectionService.SelectItems(controls);
		}
        /// <param name="payload"></param>
        /// <returns></returns>
        public override bool IsValidDropTarget(IEnumerable<object> payload)
		{
			return payload.All(t=> t is ICommonControlViewModel);
		}
		void ISelectionContainer.SetSelection(ICommonControlViewModel control)
		{
            pageSelectionService.SelectSingleChild(control);
		}
		void ISelectionContainer.AddToSelection(ICommonControlViewModel control)
		{
            pageSelectionService.AddControlToSelection(control);
		}
		void ISelectionContainer.RemoveFromSelection(ICommonControlViewModel control)
		{
            pageSelectionService.RemoveControlFromSelection(control);
		}
		void ISelectionContainer.UpdateSelection(ICommonControlViewModel control, bool isSelected)
		{
            pageSelectionService.ToggleItemSelection(control, isSelected);
		}
		void ISelectionContainer.UpdateContainerSelectionStatus()
		{
            pageSelectionService.UpdateContainerSelectionStatus();
		}
		void ISelectionContainer.RecalculateSelectionBounds()
		{
			ISelectionControlViewModel selectionViewModel = pageSelectionService.GetSelectionViewModel();
			selectionViewModel.UpdateGroupBounds();
		}
		void IGroupContainer.PromoteSelectionToGroup()
		{
			lock (children.SyncRoot)
			{
				ISelectionControlViewModel selectionViewModel = pageSelectionService.GetSelectionViewModel();
				IExplorerNodeViewModel[] selectedChildNodes = selectionViewModel.Children.ToArray();
				IControlGroupContainerViewModel controlGroupContainerViewModel = (IControlGroupContainerViewModel)selectionViewModel.CreateGroupFromSelection();
				controlGroupContainerViewModel.ID = "Group";
				selectionViewModel.Children.Clear();
				Children.RemoveRange(selectedChildNodes);
                Children.Add(controlGroupContainerViewModel);
				controlGroupContainerViewModel.AddChildren(selectedChildNodes);
                pageSelectionService.SelectSingleChild(controlGroupContainerViewModel);
			}
		}
		void IGroupContainer.DemoteGroupToSelection(IControlGroupContainerViewModel controlGroupContainer)
		{
            int childIndex;
			bool lockTaken = false;
            object syncRoot = null;
			try
			{
				Monitor.Enter(syncRoot = children.SyncRoot, ref lockTaken);
				IExplorerNodeViewModel[] childNodes = controlGroupContainer.Children.ToArray();

                controlGroupContainer.Children.Clear();
                Children.Remove(controlGroupContainer);

				childIndex = 0;
				while (childIndex < childNodes.Length)
				{
					IExplorerNodeViewModel node = childNodes[childIndex];
					node.Parent = this;
                    Children.Add(node);
					childIndex++;
				}
			}
			finally
			{
                if (lockTaken)
				{
					Monitor.Exit(syncRoot);
				}
			}
		}

		private void AddTopmostChild(bool offsetIfOverlapping, ICommonControlViewModel control)
		{
            Children.Add(control);
			if (offsetIfOverlapping)
			{
				OffsetUntilNoOverlap(control);
			}
		}
        void OffsetUntilNoOverlap(ICommonControlViewModel control)
        {
            while (CountControlsAtSamePosition(control) > 1)
            {
                control.XTranslation += 5.0;
                control.YTranslation += 5.0;
            }
        }
        int CountControlsAtSamePosition(ICommonControlViewModel control)
		{
			return Children
				.Cast<ICommonControlViewModel>()
				.Count(
				child => child.ControlID == control.ControlID &&
				child.XTranslation == control.XTranslation &&
				child.YTranslation == control.YTranslation);
		}
		public override string ToString()
		{
			return ID ?? string.Empty;
		}
		private void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs change)
		{
			switch (change.Action)
			{
			case NotifyCollectionChangedAction.Add:
                    IsSelected = true;
				break;
			case NotifyCollectionChangedAction.Remove:
                    pageSelectionService.UpdateContainerSelectionStatus();
				break;
			}
            pageUndoRedoService.AutoSelectPageAfterUndoRedo = true;
		}
		private void GenerateGridSizes()
		{
			double gridSize = 10.0;
			while (gridSize <= 100.0)
			{
                GridSizes.Add(gridSize);
				gridSize += 10.0;
			}
			gridSize = 100.0;
			while (gridSize <= 500.0)
			{
                GridSizes.Add(gridSize);
                gridSize += 25.0;
			}
		}
		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
            _layoutChildren.Dispose();
		}
		

		
		void OnStretchPagesChanged(object sender, ValueChangedEventArgs<bool> e)
		{
            RaisePropertyChanged(() => IsStretchPagesVisible);
		}

	}
}
