// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Common.Instrumentation.Metadata;
using HMI.Common.Presentation.Events;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Extensions;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
using System.Diagnostics;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Reactive;
using System.Reactive.Linq;

namespace HMI.Modules.PropertyEditor.Models
{
	[DebuggerDisplay("Property Category={Category} Name={Name} Type={Type} Readonly={IsReadOnly}")]
	/// <summary>
	/// 描述属性编辑器中展示的对象属性及其元数据。
	/// </summary>
	public class Property : DisposableViewModel,  IProperty
	{
		class DependentPropertyUpdater
		{
			public string PropertyName
			{
				get;
				private set;
			}
			public Action<object> UpdateAction
			{
				get;
				private set;
			}
			public DependentPropertyUpdater(string propertyName, Action<object> updateAction)
			{
                PropertyName = propertyName;
                UpdateAction = updateAction;
			}
		}
		
		private readonly ICollection<Attribute> attributes;
        private readonly IList<DependentPropertyUpdater> dependentPropertyUpdaters = new List<DependentPropertyUpdater>();
        private readonly IServiceProvider serviceProvider;
        private IDisposable propertyChangedSubscription;

		private bool _isReadOnly;
		private bool _isEnabled = true;
		private bool _isCollection;
		private bool _isVisible;

		public event EventHandler<ValueChangedEventArgs<bool>>? IsVisibleChanged;

		public IPropertyEditorBuilder PropertyEditorBuilder
		{
			get;
            private set;
		}
		public bool IsReadOnly
		{
			get
			{
				return _isReadOnly;
			}
			private set
			{
				if (_isReadOnly != value)
				{
                    _isReadOnly = value;
					base.RaisePropertyChanged<bool>(() => IsReadOnly);
				}
			}
		}
		public bool IsEnabled
		{
			get
			{
				return _isEnabled;
			}
			private set
			{
				if (_isEnabled != value)
				{
                    _isEnabled = value;
					base.RaisePropertyChanged<bool>(() => IsEnabled);
				}
			}
		}
		public bool IsCollection
		{
			get
			{
				return _isCollection;
			}
			private set
			{
				if (_isCollection != value)
				{
                    _isCollection = value;
					base.RaisePropertyChanged<bool>(() => IsCollection);
				}
			}
		}
		public bool IsVisible
		{
			get
			{
				return _isVisible;
			}
			private set
			{
				if (_isVisible == value)
				{
					return;
				}
				base.RaisePropertyChanging<bool>(() => IsVisible);
				ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(_isVisible, value);
                _isVisible = value;
                IsVisibleChanged?.Invoke(this, e);
				base.RaisePropertyChanged<bool>(() => IsVisible);
			}
		}
		public Type Type
		{
			get;
			private set;
		}
		public object Instance
		{
			get;
			private set;
		}
		public string DisplayName
		{
			get;
			internal set;
		}
		public string Name
		{
			get;
			private set;
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
		public bool IsUndefined
		{
			get
			{
				return false;
			}
		}
		public string PropertyPath
		{
			get
			{
				return Name;
			}
		}
		public object SourceInstance
		{
			get
			{
				return Instance;
			}
		}
		public bool SupportsTemplating
		{
			get;
			internal set;
		}
		public Property(object instance, string propertyName, Type propertyType, IExplicitPropertyMetadataProvider metadataProvider, IServiceProvider serviceLocator)
		{
			Contract.Requires<ArgumentNullException>(metadataProvider != null, "metadataProvider");
			Contract.Requires<ArgumentNullException>(propertyName != null, "propertyName");
			Contract.Requires<ArgumentNullException>(propertyType != null, "propertyType");
			Contract.Requires<ArgumentNullException>(instance != null, "instance");
			Contract.Requires<ArgumentNullException>(serviceLocator != null, "serviceLocator");

            Instance = instance;
            Name = propertyName;
            Type = propertyType;
            this.serviceProvider = serviceLocator;
            IsCollection = propertyType.IsCollectionType();
            attributes = metadataProvider.GetAttributes(Name).ToArray<Attribute>();

			getSortOrder();
			getCategory();
			getDisplayName();
			getIsReadOnly();
			getEnabledProperty();
			getPropertyEditor();
			getVisibilityProperty();

			initNotifyProperty();
		}
        void initNotifyProperty()
        {
            InitializeDependentPropertyValues();
            INotifyProperty? instanceNotifyProperty = this.Instance as INotifyProperty;
            SubscribeToInstancePropertyChanges(instanceNotifyProperty);
        }
        void SubscribeToInstancePropertyChanges(INotifyProperty? instanceNotifyProperty)
        {
            if (instanceNotifyProperty == null)
            {
                return;
            }
            IObservable<EventPattern<PropertyChangedEventArgs>> propertyChangedEvents = instanceNotifyProperty.FromPropertyChangedEvents();

            IObservable<string> changedPropertyNames = propertyChangedEvents
				.Select(evt => evt.EventArgs.PropertyName)
				.Do(new Action<string>(this.OnInstancePropertyChanged));

            this.propertyChangedSubscription = changedPropertyNames.Subscribe<string>();
        }
        void InitializeDependentPropertyValues()
        {
            UpdateDependentProperties(this.dependentPropertyUpdaters);
        }
        void UpdateDependentProperties(IEnumerable<DependentPropertyUpdater> propertyUpdaters)
        {
            foreach (DependentPropertyUpdater updater in propertyUpdaters)
            {
                object propertyValue = Reflection.GetPropertyValue(this.Instance, updater.PropertyName);
                updater.UpdateAction(propertyValue);
            }
        }
        void getVisibilityProperty()
        {
            IEnumerable<VisibilityPropertyAttribute> ele = (this as IProperty).GetAttributes<VisibilityPropertyAttribute>();

            this.IsVisible = true;
            if (ele.Count() > 0)
            {
                VisibilityPropertyAttribute? visibilityPropertyAttribute = ele.SingleOrDefault<VisibilityPropertyAttribute>();
                if (visibilityPropertyAttribute != null)
                {
                    ICollection<DependentPropertyUpdater> propertyUpdaters = this.dependentPropertyUpdaters;
                    string visibilityPropertyName = visibilityPropertyAttribute.IsVisibilePropertyName;

                    propertyUpdaters.Add(new DependentPropertyUpdater(visibilityPropertyName, obj => IsVisible = (bool)obj));
                }
            }
        }
        void getPropertyEditor()
        {
            IEnumerable<PropertyEditorAttribute> ele = (this as IProperty).GetAttributes<PropertyEditorAttribute>();
            if (ele.Count() > 0)
            {
                PropertyEditorAttribute propertyEditorAttribute = ele.Single<PropertyEditorAttribute>();
                if (propertyEditorAttribute != null)
                {
                    propertyEditorAttribute.Initialize(this.serviceProvider);
                    this.PropertyEditorBuilder = propertyEditorAttribute;
                    this.SupportsTemplating = (propertyEditorAttribute is ITemplatableEditor);
                }
            }
        }
        void getEnabledProperty()
        {
            IEnumerable<EnabledPropertyAttribute> ele = (this as IProperty).GetAttributes<EnabledPropertyAttribute>();
            if (ele.Count() > 0)
            {
                EnabledPropertyAttribute? enabledPropertyAttribute = ele.SingleOrDefault<EnabledPropertyAttribute>();
                if (enabledPropertyAttribute != null)
                {
                    string enabledPropertyName = enabledPropertyAttribute.IsEnablePropertyName;

                    this.dependentPropertyUpdaters.Add(new DependentPropertyUpdater(enabledPropertyName, obj => IsEnabled = (bool)obj));
                }
            }
        }
        void getIsReadOnly()
        {
            IEnumerable<ReadOnlyAttribute> readOnlyAttributes = (this as IProperty).GetAttributes<ReadOnlyAttribute>();

            if (readOnlyAttributes.Count() > 0)
            {
                this.IsReadOnly = readOnlyAttributes.Select(p=> p.IsReadOnly).DefaultIfEmpty(false).Single<bool>();
            }
        }
        void getDisplayName()
        {
            IEnumerable<DisplayNameAttribute> displayNameAttributes = (this as IProperty).GetAttributes<DisplayNameAttribute>();

            if (displayNameAttributes.Count() > 0)
            {
                this.DisplayName = displayNameAttributes.Select(p => p.DisplayName).Single<string>();
            }
        }
        void getCategory()
        {
            IEnumerable<CategoryAttribute> categoryAttributes = (this as IProperty).GetAttributes<PropertyCategoryAttribute>()
				.Concat((this as IProperty).GetAttributes<CategoryAttribute>());

            if (categoryAttributes.Count() > 0)
            {
                this.Category = categoryAttributes.Select(p => p.Category).First<string>();
            }
        }
        void getSortOrder()
        {
            IEnumerable<EditorOrderAttribute> ele = (this as IProperty).GetAttributes<EditorOrderAttribute>();
            if (ele.Count() > 0)
            {
                EditorOrderAttribute editorOrderAttribute = ele.Single<EditorOrderAttribute>();
                this.SortOrder = editorOrderAttribute.Order;
            }
        }
        public IEnumerable<T> GetAttributes<T>()
		{
			return attributes.OfType<T>();
		}
		public bool IsCompatibileWith(IProperty property)
		{
			Contract.Requires<ArgumentNullException>(property != null, "property");
			return Name == property.Name && Type == property.Type && Category == property.Category && PropertyEditorBuilder.IsMultiSelectionCompatibleWith(this, property);
		}
		protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
			if (propertyChangedSubscription != null)
			{
                propertyChangedSubscription.Dispose();
			}
		}
		~Property()
		{
			this.Dispose(false);
		}
		private void OnInstancePropertyChanged(string changedPropertyName)
		{
			IEnumerable<DependentPropertyUpdater> matchingUpdaters = dependentPropertyUpdaters
                .Where(updater => updater.PropertyName == changedPropertyName);
			UpdateDependentProperties(matchingUpdaters);
		}

	}
}
