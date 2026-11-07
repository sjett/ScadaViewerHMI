// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Common.Instrumentation.ErrorHandling;
using HMI.Common.Instrumentation.Events;
using HMI.Common.Presentation.Reactive;
using HMI.Common.Presentation.ViewModels;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Presentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Infrastructure.ViewModels;
using HMI.Modules.PropertyEditor.Resources;
using Newtonsoft.Json.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;


namespace HMI.Modules.PropertyEditor.ViewModels
{
	public class PropertyEditorViewModel : DisposableViewModel, IPropertyEditorViewModel
	{
        private readonly IProjectManagerViewModel _projectManagerViewModel;
		private readonly IPropertyInspectorService _propertyInspectorService;

		private readonly IUiScheduler _uiScheduler;
		private readonly Subject<bool> _selectionChanged;
        private readonly INotificationService _notificationService;

        private Telerik.Windows.Controls.GridViewLength _propertyNameColumnWidth;
        private Telerik.Windows.Controls.GridViewLength _propertyValueColumnWidth;

		public event EventHandler? PropertiesUpdating;

		public event EventHandler? PropertiesUpdated;

		public event EventHandler<ValueChangedEventArgs<Telerik.Windows.Controls.GridViewLength>>? PropertyNameColumnWidthChanged;

		public event EventHandler<ValueChangedEventArgs<Telerik.Windows.Controls.GridViewLength>>? PropertyValueColumnWidthChanged;

		public Telerik.Windows.Controls.GridViewLength PropertyNameColumnWidth
		{
			get
			{
				return _propertyNameColumnWidth;
			}
			set
			{
				if (_propertyNameColumnWidth != value)
				{
					ValueChangedEventArgs<Telerik.Windows.Controls.GridViewLength> e = new ValueChangedEventArgs<Telerik.Windows.Controls.GridViewLength>(_propertyNameColumnWidth, value);
                    _propertyNameColumnWidth = value;
                    PropertyNameColumnWidthChanged?.Invoke(this, e);
					base.RaisePropertyChanged<Telerik.Windows.Controls.GridViewLength>(() => PropertyNameColumnWidth);
				}
			}
		}
		public Telerik.Windows.Controls.GridViewLength PropertyValueColumnWidth
		{
			get
			{
				return _propertyValueColumnWidth;
			}
			set
			{
				if (_propertyValueColumnWidth != value)
				{
					ValueChangedEventArgs<Telerik.Windows.Controls.GridViewLength> e = new ValueChangedEventArgs<Telerik.Windows.Controls.GridViewLength>(_propertyValueColumnWidth, value);
                    _propertyValueColumnWidth = value;
                    PropertyValueColumnWidthChanged?.Invoke(this, e);
					base.RaisePropertyChanged<Telerik.Windows.Controls.GridViewLength>(() => PropertyValueColumnWidth);
				}
			}
		}
		public IObservableCollection<IProperty> PropertyDescriptors
		{
			get;
			private set;
		}

		public PropertyEditorViewModel(
			IProjectManagerViewModel projectManagerViewModel, 
			IPropertyInspectorService propertyInspectorService, 
			IEventAggregator eventAggregator, 
			INotificationService notificationService, 
			IUiScheduler uiScheduler)
		{
            _propertyNameColumnWidth = new Telerik.Windows.Controls.GridViewLength(0.5, Telerik.Windows.Controls.GridViewLengthUnitType.Star);
            _propertyValueColumnWidth = new Telerik.Windows.Controls.GridViewLength(0.5, Telerik.Windows.Controls.GridViewLengthUnitType.Star);

			Contract.Requires<ArgumentNullException>(projectManagerViewModel != null);
			Contract.Requires<ArgumentNullException>(propertyInspectorService != null);
			Contract.Requires<ArgumentNullException>(eventAggregator != null);
			Contract.Requires<ArgumentNullException>(notificationService != null, "notificationService");
			Contract.Requires<ArgumentNullException>(uiScheduler != null, "uiScheduler");

            PropertyDescriptors = new FullyObservableCollection<IProperty>();
            _projectManagerViewModel = projectManagerViewModel;
            _propertyInspectorService = propertyInspectorService;
            _notificationService = notificationService;

            _uiScheduler = uiScheduler;
            _selectionChanged = new Subject<bool>();

			_selectionChanged
				.Throttle(TimeSpan.FromMilliseconds(50.0), _uiScheduler)
				.ObserveOn(_uiScheduler)
                .Do(b => PropertiesUpdating?.Invoke(this, new EventArgs()))
                .Do(b =>
				{
                    if (PropertyDescriptors.Count > 0)
                    {
                        PropertyDescriptors.Clear();
                    }
                })
                .Do(b =>
				{
                    if (projectManagerViewModel.Project != null)
                    {
                        this.AddProperties();
                    }
                })
                .Do(b => PropertiesUpdated?.Invoke(this, new EventArgs()))
                .Subscribe<bool>();

            SubscribeToNodeSelectionChanges(eventAggregator);

			LoadPreferences();

            PropertyNameColumnWidthChanged += OnPropertyNameColumnWidthChanged;
            PropertyValueColumnWidthChanged += OnPropertyValueColumnWidthChanged;
		}

        private void SubscribeToNodeSelectionChanges(IEventAggregator eventAggregator)
        {
            var @event = eventAggregator.GetEvent<IsNodeSelectedChangedEvent>();
            Contract.Assert(@event != null);
            @event?.Subscribe(new Action<IExplorerNodeViewModel>(this.OnIsNodeSelectedChanged));
        }
        public void OnIsNodeSelectedChanged(IExplorerNodeViewModel node)
        {
            _selectionChanged.OnNext(true);
        }
        private void OnPropertyValueColumnWidthChanged(object sender, ValueChangedEventArgs<Telerik.Windows.Controls.GridViewLength> e)
        {
            SavePreferences();
        }
        private void OnPropertyNameColumnWidthChanged(object sender, ValueChangedEventArgs<Telerik.Windows.Controls.GridViewLength> e)
        {
            SavePreferences();
        }
        private void LoadPreferences()
        {
            try
            {
                var preferences = Preferences.GetPreferences(this);
                if (preferences != null)
                {
                    Telerik.Windows.Controls.GridView.GridViewLengthConverter gridViewLengthConverter = new Telerik.Windows.Controls.GridView.GridViewLengthConverter();
                    string value = ((string?)preferences["PropertyNameColumnWidth"]) ?? "0.5*";
                    this.PropertyNameColumnWidth = (Telerik.Windows.Controls.GridViewLength)gridViewLengthConverter.ConvertFrom(value);

                    string propertyValueColumnWidth = ((string?)preferences["PropertyValueColumnWidth"]) ?? "0.5*";
                    this.PropertyValueColumnWidth = (Telerik.Windows.Controls.GridViewLength)gridViewLengthConverter.ConvertFrom(propertyValueColumnWidth);
                }
            }
            catch (Exception exception)
            {
                this._notificationService.ShowWarning(ErrorTexts.ErrorLoadingPreferences, exception);
            }
        }

        private void SavePreferences()
        {
            Preferences.SetPreferences(this, new JObject
            {
                new JProperty("PropertyNameColumnWidth", this._propertyNameColumnWidth),
                new JProperty("PropertyValueColumnWidth", this._propertyValueColumnWidth)
            });
        }

		private void AddProperties()
		{
			object[] selectedNodes = GetSelectedNodes();
			ICollection<IProperty> properties = _propertyInspectorService.GetProperties(selectedNodes);
            PropertyDescriptors.AddRange(properties);
		}
        private object[] GetSelectedNodes()
        {
            List<object> selectedNodes = new List<object>();
            FindSelectedNodes(selectedNodes, _projectManagerViewModel.Project);
            return selectedNodes.ToArray();
        }
        private void FindSelectedNodes(List<object> selectedNodes, IExplorerNodeViewModel node)
        {
            if (node.IsSelected)
            {
                selectedNodes.Add(node);
            }
            foreach (IExplorerNodeViewModel child in node.Children)
            {
                FindSelectedNodes(selectedNodes, child);
            }
        }
        protected override void DisposeManagedResources()
		{
			base.DisposeManagedResources();
            _selectionChanged.Dispose();
		}
		

	}
}
