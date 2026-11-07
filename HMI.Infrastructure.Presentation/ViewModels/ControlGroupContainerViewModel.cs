// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.Views;
using HMI.Infrastructure.Services;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Reactive.Linq;
using System.Text;
using System.Windows;

namespace HMI.Infrastructure.Presentation.ViewModels
{
    public class ControlGroupContainerViewModel : CommonControlViewModel, IControlGroupContainerViewModel
	{
        private readonly IObservableCollection<ControlBoundsCalculator> childBoundsCalculators = new FullyObservableCollection<ControlBoundsCalculator>();
        private readonly ICollectionMonitor<ControlBoundsCalculator> childBoundsCalculatorMonitor;
        private IDisposable childrenCollectionSubscription;

		private Point _initialPosition;
		private Size _initialSize;
		private bool _ignoreAngleChange;
		private bool _ignoreTranslation;
		private bool _ignoreResize;

        public override bool ForceProportionalResize
		{
			get
			{
				return true;
			}
		}
        [Browsable(false)]
		public Point InitialPosition
		{
			get
			{
				return _initialPosition;
			}
			set
			{
				if (_initialPosition == value)
				{
					return;
				}
                RaisePropertyChanging(() => InitialPosition);
                _initialPosition = value;
                RaisePropertyChanged(() => InitialPosition);
			}
		}
        [Browsable(false)]
		public Size InitialSize
		{
			get
			{
				return _initialSize;
			}
			set
			{
				if (_initialSize == value)
				{
					return;
				}
                RaisePropertyChanging(() => InitialSize);
                _initialSize = value;
                RaisePropertyChanged(() => InitialSize);
			}
		}
        [Browsable(false)]
		public bool IgnoreAngleChange
		{
			get
			{
				return _ignoreAngleChange;
			}
			set
			{
				if (_ignoreAngleChange == value)
				{
					return;
				}
                RaisePropertyChanging(() => IgnoreAngleChange);
                _ignoreAngleChange = value;
                RaisePropertyChanged(() => IgnoreAngleChange);
			}
		}
        [Browsable(false)]
		public bool IgnoreTranslation
		{
			get
			{
				return _ignoreTranslation;
			}
			set
			{
				if (_ignoreTranslation == value)
				{
					return;
				}
                RaisePropertyChanging(() => IgnoreTranslation);
                _ignoreTranslation = value;
                RaisePropertyChanged(() => IgnoreTranslation);
			}
		}
        [Browsable(false)]
		public bool IgnoreResize
		{
			get
			{
				return _ignoreResize;
			}
			set
			{
				if (_ignoreResize == value)
				{
					return;
				}
                RaisePropertyChanging(() => IgnoreResize);
                _ignoreResize = value;
                RaisePropertyChanged(() => IgnoreResize);
			}
		}
        [Browsable(false)]
		public bool UpdateBoundsWhenChildrenChange
		{
			get;
			set;
		}
        /// <param name="container"></param>
        /// <param name="parent"></param>
        /// <param name="controlType"></param>
        public ControlGroupContainerViewModel(IServiceProvider container, IExplorerNodeViewModel parent, Type controlType) : base(container, parent, controlType)
		{
            UpdateBoundsWhenChildrenChange = true;
			InitializeChildrenCollectionObserver();

            AngleChanged += new EventHandler<ValueChangedEventArgs<double>>(OnAngleChanged);
            XTranslationChanged += new EventHandler<ValueChangedEventArgs<double>>(OnXTranslationChanged);
            YTranslationChanged += new EventHandler<ValueChangedEventArgs<double>>(OnYTranslationChanged);
		}
        void InitializeChildrenCollectionObserver()
        {
            var childrenCollectionChanges = Observable
				.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
					addHandler => Children.CollectionChanged += addHandler, 
					removeHandler => Children.CollectionChanged -= removeHandler);
            
            childrenCollectionSubscription = childrenCollectionChanges
				.Select(t => t.EventArgs)
                .Do(OnChildrenCollectionChanged)
                .Subscribe<NotifyCollectionChangedEventArgs>();
        }
        /// <param name="source"></param>
        public ControlGroupContainerViewModel(ControlGroupContainerViewModel source) : this(source.Services, source.Parent, typeof(GroupControl))
		{
            childBoundsCalculatorMonitor = source.Services.Resolve<ICollectionMonitor<ControlBoundsCalculator>>();
            childBoundsCalculatorMonitor.Start(childBoundsCalculators);

            InitialPosition = source.InitialPosition;
            InitialSize = source.InitialSize;
            Angle = source.Angle;
            XTranslation = source.XTranslation;
            YTranslation = source.YTranslation;
            ZIndex = source.ZIndex;
            Width = source.Width;
            Height = source.Height;
            ControlWidth = source.ControlWidth;
            ControlHeight = source.ControlHeight;
            Parent = source.Parent;
		}
        public override void TransformationStarted()
		{
			base.TransformationStarted();
			foreach (ICommonControlViewModel childControl in Children.Controls())
			{
				RaiseChildTransformPropertyChanges(childControl);
			}
		}
        void RaiseChildTransformPropertyChanges(ICommonControlViewModel childControl)
        {
            childControl.RaisePropertyChanging(() => childControl.XTranslation);
            childControl.RaisePropertyChanged(() => childControl.XTranslation);

            childControl.RaisePropertyChanging(() => childControl.YTranslation);
            childControl.RaisePropertyChanged(() => childControl.YTranslation);

            childControl.RaisePropertyChanging(() => childControl.Angle);
            childControl.RaisePropertyChanged(() => childControl.Angle);

            childControl.RaisePropertyChanging(() => childControl.ControlWidth);
            childControl.RaisePropertyChanged(() => childControl.ControlWidth);

            childControl.RaisePropertyChanging(() => childControl.ControlHeight);
            childControl.RaisePropertyChanged(() => childControl.ControlHeight);
        }
        /// <param name="horizontalHandlePosition"></param>
        /// <param name="verticalHandlePosition"></param>
        /// <param name="dragX"></param>
        /// <param name="dragY"></param>
        public override void Resize(HorizontalAlignment horizontalHandlePosition, VerticalAlignment verticalHandlePosition, double dragX, double dragY)
		{
			if (IgnoreResize)
			{
				return;
			}
            IgnoreResize = true;
			bool ignoreTranslation = IgnoreTranslation;
            IgnoreTranslation = true;
			base.Resize(horizontalHandlePosition, verticalHandlePosition, dragX, dragY);
			Size size = new Size(ControlWidth, ControlHeight);
			RecalculateChildBounds(size);

            IgnoreTranslation = ignoreTranslation;
            IgnoreResize = false;
		}
        void RecalculateChildBounds(Size newGroupSize)
        {
            foreach (ControlBoundsCalculator boundsCalculator in childBoundsCalculators)
            {
                RecalculateChildBounds(boundsCalculator, newGroupSize);
            }
        }
        void RecalculateChildBounds(ControlBoundsCalculator boundsCalculator, Size newGroupSize)
        {
            boundsCalculator.PerformResize(InitialSize, newGroupSize);
            boundsCalculator.PerformTranslation(InitialSize, newGroupSize, InitialPosition);
        }
        /// <returns></returns>
        public override string GetXaml()
        {
            StringBuilder xamlBuilder = new StringBuilder();
            foreach (ICommonControlViewModel childControl in base.Children.OfType<ICommonControlViewModel>())
            {
                xamlBuilder.AppendLine(childControl.GetXaml());
            }
            return xamlBuilder.ToString();
        }
        /// <returns></returns>
        public override IEnumerable<string> GetNamespaces()
        {
            IEnumerable<ICommonControlViewModel> childControls = base.Children.OfType<ICommonControlViewModel>();
            return childControls.SelectMany(control => control.GetNamespaces()).Distinct<string>();
        }
        /// <returns></returns>
        public override IEnumerable<AssemblyReference> GetRequiredAssemblies()
        {
            IEnumerable<ICommonControlViewModel> childControls = base.Children.OfType<ICommonControlViewModel>();
            return childControls.SelectMany(control => control.GetRequiredAssemblies());
        }
        /// <returns></returns>
        [Obsolete]
        public override IEnumerable<KeyValuePair<string, string>> GetSilverlightObjectParameters()
        {
            IEnumerable<ICommonControlViewModel> childControls = base.Children.OfType<ICommonControlViewModel>();
            return childControls.SelectMany(control => control.GetSilverlightObjectParameters());
        }

        /// <param name="children"></param>
        public void AddChildren(IEnumerable<IExplorerNodeViewModel> children)
		{
			foreach (IExplorerNodeViewModel child in children)
			{
				RefreshChildId(child);
                AddChild(child);
			}
		}
        void RefreshChildId(IExplorerNodeViewModel child)
        {
            child.ID = child.ID;
        }
        private void AddChild(IExplorerNodeViewModel child)
		{
            Children.Add(child);
			child.Parent = this;
		}
		private void OnYTranslationChanged(object sender, ValueChangedEventArgs<double> e)
		{
			if (IgnoreTranslation)
			{
				return;
			}
            IgnoreTranslation = true;
			TranslateChildren(0.0, e.NewValue - e.OldValue);
            UpdateInitialGroupBounds();
            IgnoreTranslation = false;
		}
		private void TranslateChildren(double deltaX, double deltaY)
		{
			foreach (ICommonControlViewModel childControl in Children.Controls())
			{
				childControl.Translate(deltaX, deltaY, TransformMode.Relative);
			}
		}
		private void OnXTranslationChanged(object sender, ValueChangedEventArgs<double> e)
		{
			if (IgnoreTranslation)
			{
				return;
			}
            IgnoreTranslation = true;
			TranslateChildren(e.NewValue - e.OldValue, 0.0);
            UpdateInitialGroupBounds();
            IgnoreTranslation = false;
		}
		void RotateChildrenControls(Point center, double angleDelta)
		{
			foreach (ICommonControlViewModel childControl in Children.Controls())
			{
				RotateChildControl(childControl, center, angleDelta);
			}
		}
        void RotateChildControl(ICommonControlViewModel childControl, Point center, double angleDelta)
        {
            Point positionAfterRotation = GeometryService.GetPositionAfterRotation(center, childControl, angleDelta);
            childControl.XTranslation = positionAfterRotation.X;
            childControl.YTranslation = positionAfterRotation.Y;
            childControl.Rotate(angleDelta, TransformMode.Relative, false);
        }
        private void OnAngleChanged(object sender, ValueChangedEventArgs<double> e)
		{
			if (IgnoreAngleChange)
			{
				return;
			}
			bool ignoreAngleChange = IgnoreAngleChange;
            IgnoreAngleChange = true;
			RotateChildrenByDelta(e.NewValue - e.OldValue);
            UpdateInitialGroupBounds();
            IgnoreAngleChange = ignoreAngleChange;
		}
        void RotateChildrenByDelta(double angleDelta)
        {
            Point controlCenter = GeometryService.GetControlCenter(this);
            RotateChildrenControls(controlCenter, angleDelta);
        }
        private void UpdateInitialGroupBounds()
		{
            InitialPosition = new Point(XTranslation, YTranslation);
            InitialSize = new Size(ControlWidth, ControlHeight);
            RebuildChildBoundsCalculators();
		}
		private void RebuildChildBoundsCalculators()
		{
            childBoundsCalculators.Clear();
			foreach (ICommonControlViewModel childControl in Children.Controls())
			{
				ControlBoundsCalculator boundsCalculator = new ControlBoundsCalculator(childControl, this);
                childBoundsCalculators.Add(boundsCalculator);
			}
		}
        void OnChildrenCollectionChanged(NotifyCollectionChangedEventArgs change)
		{
			if (UpdateBoundsWhenChildrenChange)
			{
                UpdateGroupBounds();
			}
			IEnumerable<ICommonControlViewModel> newControls = ConvertToCommonControls(change.NewItems);
			HideControlsDecorators(newControls);

            IEnumerable<ICommonControlViewModel> oldControls = ConvertToCommonControls(change.OldItems);
			ShowControlsDecorators(oldControls);
        }
        IEnumerable<ICommonControlViewModel> ConvertToCommonControls(System.Collections.IList? items)
        {
            if (items != null)
            {
                return items.OfType<ICommonControlViewModel>();
            }
            return Enumerable.Empty<ICommonControlViewModel>();
        }
        void ShowControlsDecorators(IEnumerable<ICommonControlViewModel> controls)
        {
            foreach (ICommonControlViewModel control in controls)
            {
                ShowDecorator(control);
            }
        }
        void ShowDecorator(ICommonControlViewModel control)
        {
            control.ShowSelectionPreview = false;
            control.ShowDragDecorator = true;
        }
        void HideControlsDecorators(IEnumerable<ICommonControlViewModel> controls)
        {
            foreach (ICommonControlViewModel control in controls)
            {
                HideDecorator(control);
            }
        }
        void HideDecorator(ICommonControlViewModel control)
        {
            control.ShowDragDecorator = false;
            control.ShowResizeDecorator = false;
            control.ShowRotationDecorator = false;
            control.ShowSelectionDecorator = false;
            control.ShowSelectionPreview = true;
        }
        private void RemoveAllChildren()
		{
			lock (children.SyncRoot)
			{
				while (Children.Any())
				{
                    Children.RemoveAt(0);
				}
			}
		}
		protected override void DisposeManagedResources()
		{
            RemoveAllChildren();

			DisposeChildrenCollectionSubscription();

			base.DisposeManagedResources();
		}
        void DisposeChildrenCollectionSubscription()
        {
            if (childrenCollectionSubscription != null)
            {
                childrenCollectionSubscription.Dispose();
            }
        }
        public void UpdateGroupBounds()
		{
			if (!Children.Any())
			{
				return;
			}
			Point start;
            Point end;
            GeometryService.GetChildrenBoundsInSurfaceCoordinates(out start, out end, Children.Controls().ToArray());
            
        	RotateGroupToZero();
            TranslateGroup(start);
            ResizeGroup(end, start);
            UpdateInitialGroupBounds();
        }
        private void ResizeGroup(Point end, Point start)
		{
            Width = ControlWidth = end.X - start.X;
            Height = ControlHeight = end.Y - start.Y;
		}
		private void TranslateGroup(Point start)
		{
            IgnoreTranslation = true;
            XTranslation = start.X;
            YTranslation = start.Y;
            IgnoreTranslation = false;
		}
		private void RotateGroupToZero()
		{
            IgnoreAngleChange = true;
			base.Rotate(0.0, TransformMode.Absolute, false);
            IgnoreAngleChange = false;
		}
		

	}
}
