// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.Metadata;
using HMI.Common.Presentation.ViewModels;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Services;
using HMI.Modules.PropertyEditor.Resources;
using HMI.Modules.PropertyEditor.Services;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows.Data;
using System.Windows.Media;

namespace HMI.Modules.PropertyEditor.Models
{
	/// <summary>
	/// 表示多选对象之间共有的属性，并将值变更同步到所有对象。
	/// </summary>
	public class MultipleInstanceProperty : DisposableViewModel, IProperty,  IBindingSource
	{
        class BrushComparer : IEqualityComparer<Brush>
        {

            public bool Equals(Brush? x, Brush? y)
            {
                return (x == null && y == null) || (x != null && y != null && !(x.GetType() != y.GetType()) && (AreSolidColorBrushesEqual(x as SolidColorBrush, y as SolidColorBrush) || AreLinearGradientBrushesEqual(x as LinearGradientBrush, y as LinearGradientBrush)));
            }
            bool AreSolidColorBrushesEqual(SolidColorBrush firstBrush, SolidColorBrush secondBrush)
            {
                return firstBrush != null && secondBrush != null && firstBrush.Color == secondBrush.Color && firstBrush.Opacity == secondBrush.Opacity;
            }
            bool AreLinearGradientBrushesEqual(LinearGradientBrush firstBrush, LinearGradientBrush secondBrush)
            {
                if (firstBrush == null || secondBrush == null)
                {
                    return false;
                }
                if (firstBrush.MappingMode == secondBrush.MappingMode && firstBrush.StartPoint == secondBrush.StartPoint && firstBrush.EndPoint == secondBrush.EndPoint && firstBrush.ColorInterpolationMode == secondBrush.ColorInterpolationMode && firstBrush.SpreadMethod == secondBrush.SpreadMethod)
                {
                    return AreGradientStopCollectionsEqual(firstBrush.GradientStops, secondBrush.GradientStops);
                }
                return false;
            }
            bool AreGradientStopCollectionsEqual(GradientStopCollection firstGradientStops, GradientStopCollection secondGradientStops)
            {
                if (firstGradientStops.Count == secondGradientStops.Count)
                {
                    return firstGradientStops.Zip(secondGradientStops, (first, second) =>
                        first.Offset == second.Offset && first.Color == second.Color).All(equal => equal);
                }
                return false;
            }
            public int GetHashCode(Brush obj)
            {
                if (obj == null)
                {
                    return 0;
                }
                SolidColorBrush? solidColorBrush = obj as SolidColorBrush;
                if (solidColorBrush != null)
                {
                    return solidColorBrush.Color.GetHashCode() ^ solidColorBrush.Opacity.GetHashCode();
                }
                LinearGradientBrush? linearGradientBrush = obj as LinearGradientBrush;
                if (linearGradientBrush != null)
                {
                    int endpointHash = linearGradientBrush.StartPoint.GetHashCode() ^ linearGradientBrush.EndPoint.GetHashCode();
                    int gradientStopHash = linearGradientBrush.GradientStops.Aggregate(
                        0,
                        (hash, stop) => hash ^ stop.Offset.GetHashCode() ^ stop.Color.GetHashCode());

                    return endpointHash ^ gradientStopHash;
                }
                return obj.GetHashCode();
            }

        }

		private readonly IActionMonitor actionMonitor;
		private readonly string _propertyPath;
        private readonly ICollection<IDisposable> subscriptions = new CompositeDisposable();
        private readonly ICollection<IProperty> sharedProperties;
		private bool multicastValueInitialized;
		private object multicastValue;
		private bool _isVisible;
		private bool _isEnabled;
			
		public object MulticastValue
		{
			get
			{
				if (!multicastValueInitialized)
				{
                    RefreshMulticastValue();
				}
				return multicastValue;
			}
			set
			{
				if (Equals(value, multicastValue))
				{
					return;
				}
                multicastValue = value;
				MultiboundUndoAction multiboundUndoAction = new MultiboundUndoAction(this, Name);
				multiboundUndoAction.IsCollectingActions = true;
                actionMonitor.RecordAction(multiboundUndoAction);
				foreach (IProperty current in sharedProperties)
				{
					Reflection.SetPropertyValue(current.SourceInstance, current.PropertyPath, value);
				}
				multiboundUndoAction.IsCollectingActions = false;
				base.RaisePropertyChanged<bool>(() => IsUndefined);
			}
		}
		public IPropertyEditorBuilder PropertyEditorBuilder
		{
			get;
			internal set;
		}
		public bool IsReadOnly
		{
			get;
			internal set;
		}
		public bool IsCollection
		{
			get
			{
				return false;
			}
		}
		public Type Type
		{
			get;
			internal set;
		}
		public bool SupportsTemplating
		{
			get;
			internal set;
		}
		public string DisplayName
		{
			get;
			private set;
		}
		public string Name
		{
			get;
			internal set;
		}
		public string Category
		{
			get;
			internal set;
		}
		public double SortOrder
		{
			get;
			internal set;
		}
		public virtual bool IsVisible
		{
			get
			{
				return _isVisible;
			}
			internal set
			{
				if (_isVisible == value)
				{
					return;
				}
				base.RaisePropertyChanging<bool>(() => IsVisible);
                _isVisible = value;
				base.RaisePropertyChanged<bool>(() => IsVisible);
			}
		}
		public virtual bool IsEnabled
		{
			get
			{
				return _isEnabled;
			}
			internal set
			{
				if (_isEnabled == value)
				{
					return;
				}
				base.RaisePropertyChanging<bool>(() => IsEnabled);
                _isEnabled = value;
				base.RaisePropertyChanged<bool>(() => IsEnabled);
			}
		}
		public string PropertyPath
		{
			get
			{
				return _propertyPath;
			}
		}
		public object SourceInstance
		{
			get
			{
				return this;
			}
		}
		public bool IsUndefined
		{
			get
			{
				return Equals(multicastValue, Binding.DoNothing);
			}
		}
		public MultipleInstanceProperty(ICollection<Domain.Models.IProperty> sharedProperties, IActionMonitor actionMonitor)
		{
			Contract.Requires<ArgumentNullException>(sharedProperties != null, "sharedProperties");
			Contract.Requires<ArgumentNullException>(actionMonitor != null, "actionMonitor");

            _propertyPath = Reflection.GetPropertyName<object>(() => MulticastValue);
            this.sharedProperties = sharedProperties;
            this.actionMonitor = actionMonitor;

			ValidateSharedProperties();
			InitializePropertyMetadata();
		}
        void ValidateSharedProperties()
        {
			if (this.sharedProperties.Any(t => t.IsCollection))
			{
				throw new InvalidOperationException(ErrorTexts.MultipleSelectionDoesNotSupportCollectionProperties);
			}
        }
        void InitializePropertyMetadata()
        {
            InitializeName();
            InitializeType();
            InitializeReadOnlyState();
            ObserveVisibilityChanges();
            ObserveEnabledStateChanges();
            InitializeCategory();

            this.InitializeDisplayName();

            InitializeSortOrder();
            InitializePropertyEditor();
            SubscribeToPropertyChanges();
        }
        void InitializeName()
        {
			this.Name = this.sharedProperties.Select(t => t.Name).Distinct<string>().Single<string>();
        }
        void InitializeType()
        {
            this.Type = this.sharedProperties.Select(t=> t.Type).Distinct<Type>().Single<Type>();
        }
        void InitializeReadOnlyState()
        {
            this.IsReadOnly = this.sharedProperties.Any(t=> t.IsReadOnly);
        }
        void ObserveVisibilityChanges()
        {
            this.subscriptions.Add(
				this.sharedProperties
                .Select(t => ObserveVisibilityPropertyChanges(t, Reflection.GetPropertyName<bool>(() => t.IsVisible)))
				.Merge<EventPattern<PropertyChangedEventArgs>>()
				.Do(e => UpdateVisibility())
				.Subscribe<EventPattern<PropertyChangedEventArgs>>());

            UpdateVisibility();
        }
        void UpdateVisibility()
        {
			this.IsVisible = this.sharedProperties.All(t => t.IsVisible);
        }
        IObservable<EventPattern<PropertyChangedEventArgs>> ObserveVisibilityPropertyChanges(INotifyPropertyChanged sourceInstance, string pName)
        {
			return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
				addHandler => sourceInstance.PropertyChanged += addHandler,
				removeHandler => sourceInstance.PropertyChanged -= removeHandler)
				.Where(t => t.EventArgs.PropertyName == pName);
        }
        void ObserveEnabledStateChanges()
        {
            this.subscriptions.Add(
				this.sharedProperties
				.Select(t => ObserveEnabledPropertyChanges(t, Reflection.GetPropertyName<bool>(() => t.IsEnabled)))
				.Merge<EventPattern<PropertyChangedEventArgs>>()
				.Do(e => UpdateEnabledState())
				.Subscribe<EventPattern<PropertyChangedEventArgs>>());
            
			UpdateEnabledState();
        }
        void UpdateEnabledState()
        {
            this.IsEnabled = this.sharedProperties.All(t=> t.IsEnabled);
        }
        IObservable<EventPattern<PropertyChangedEventArgs>> ObserveEnabledPropertyChanges(INotifyPropertyChanged sourceInstance, string pName)
        {
			return Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
				addHandler => sourceInstance.PropertyChanged += addHandler,
				removeHandler => sourceInstance.PropertyChanged -= removeHandler)
				.Where(t => t.EventArgs.PropertyName == pName);
        }
        void InitializeCategory()
        {
			this.Category = this.sharedProperties.Select(t => t.Category).Distinct<string>().Single<string>();
        }
        void InitializeSortOrder()
        {
			this.SortOrder = this.sharedProperties.Average(t => t.SortOrder);
        }
        void InitializePropertyEditor()
        {
            this.PropertyEditorBuilder = this.sharedProperties.Select(t=> t.PropertyEditorBuilder).First<IPropertyEditorBuilder>();

            this.SupportsTemplating = this.sharedProperties.Select(t => t.SupportsTemplating).First<bool>();
        }
        void SubscribeToPropertyChanges()
        {
            this.subscriptions.AddRange(CreatePropertyChangingSubscriptions());
            this.subscriptions.AddRange(CreatePropertyChangedSubscriptions());
        }
        IEnumerable<IDisposable> CreatePropertyChangingSubscriptions()
        {
            var propertySources = this.sharedProperties
				.Select(property => (sourceInstance: property.SourceInstance as INotifyPropertyChanging, propertyPath: property.PropertyPath));

			return propertySources
				.Where(source => source.sourceInstance != null)
				.Select(source => ObservePropertyChanging(source.propertyPath, source.sourceInstance!));
        }
        IDisposable ObservePropertyChanging(string propertyPath, INotifyPropertyChanging sourceInstance)
        {
			return ObserveSourcePropertyChanging(propertyPath, sourceInstance)
				.Do(e => base.RaisePropertyChanging(_propertyPath))
				.Subscribe<EventPattern<PropertyChangingEventArgs>>();
        }
		IObservable<EventPattern<PropertyChangingEventArgs>> ObserveSourcePropertyChanging(string propertyPath, INotifyPropertyChanging sourceInstance)
		{
			return Observable
				.FromEventPattern<PropertyChangingEventHandler, PropertyChangingEventArgs>(
				addHandler => sourceInstance.PropertyChanging += addHandler,
				removeHandler => sourceInstance.PropertyChanging -= removeHandler)
				.Where(t => t.EventArgs.PropertyName == propertyPath);
		}
        IEnumerable<IDisposable> CreatePropertyChangedSubscriptions()
        {
            var propertySources = this.sharedProperties
				.Select(property => (sourceInstance: property.SourceInstance as INotifyPropertyChanged, propertyPath: property.PropertyPath));

            return propertySources
            .Where(source => source.sourceInstance != null)
				.Select(source => ObservePropertyChanged(source.propertyPath, source.sourceInstance!));
        }
        IDisposable ObservePropertyChanged(string propertyName, INotifyPropertyChanged sourceInstance)
        {
            return ObserveSourcePropertyChanged(sourceInstance, propertyName)
				.Do(t=> base.RaisePropertyChanged(_propertyPath))
				.Subscribe<EventPattern<PropertyChangedEventArgs>>();
        }
		IObservable<EventPattern<PropertyChangedEventArgs>> ObserveSourcePropertyChanged(INotifyPropertyChanged sourceInstance, string propertyName)
		{
			return Observable
				.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
				addHandler => sourceInstance.PropertyChanged += addHandler,
				removeHandler => sourceInstance.PropertyChanged -= removeHandler)
				.Where(t => t.EventArgs.PropertyName == propertyName);
		}
        void InitializeDisplayName()
		{
			string[] array = sharedProperties.Select(t => t.DisplayName).Distinct().ToArray();
            DisplayName = array.Length > 1 ? Name : array[0];
		}

		private void RefreshMulticastValue()
		{
			IEnumerable<object> source = sharedProperties.Select(t => Reflection.GetPropertyValue(t.SourceInstance, t.PropertyPath));
			object[] array;
			if (Type == typeof(Brush))
			{
				array = source.Cast<Brush>().Distinct(new BrushComparer()).Cast<object>().ToArray<object>();
			}
			else
			{
				array = source.Distinct().ToArray();
			}
            multicastValue = array.Length > 1 ? Binding.DoNothing : array[0];
			base.RaisePropertyChanged<bool>(() => IsUndefined);
            multicastValueInitialized = true;
		}
		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
			DisposeSubscriptions();
			DisposeSharedProperties();
		}
        void DisposeSubscriptions()
        {
            if (this.subscriptions == null)
            {
                return;
            }
            foreach (IDisposable current in this.subscriptions)
            {
                current.Dispose();
            }
        }
        void DisposeSharedProperties()
        {
            if (this.sharedProperties == null)
            {
                return;
            }
            foreach (IProperty current in this.sharedProperties)
            {
                current.Dispose();
            }
        }
        public IEnumerable<T> GetAttributes<T>()
		{
			return sharedProperties.SelectMany( GetAttributeFromProperty<T> );
		}
		public bool IsCompatibileWith(IProperty property)
		{
			throw new NotSupportedException();
		}

		private IEnumerable<T> GetAttributeFromProperty<T>(IProperty property)
		{
			return property.GetAttributes<T>();
		}
	}
}
