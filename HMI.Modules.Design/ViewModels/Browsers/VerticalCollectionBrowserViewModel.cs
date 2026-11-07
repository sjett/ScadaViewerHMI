// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Browsers.ViewModels;
using HMI.Common.Collections;
using HMI.Common.Presentation.ViewModels;
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Services;
using HMI.Infrastructure.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
namespace HMI.Modules.Design.ViewModels.Browsers
{
	public class VerticalCollectionBrowserViewModel : CollectionBrowserViewModel, IBrowserViewModel, IDisposable, IViewModel, INotifyPropertyChanged, ICollectionBrowserViewModel, IVerticalCollectionBrowserViewModel, IPropertyEditorViewModel
	{
		private readonly IObservableCollection<IProperty> _propertyDescriptors = new FullyObservableCollection<IProperty>();

		public event EventHandler? PropertiesUpdating;

		public event EventHandler? PropertiesUpdated;

		public override ICollectionEditorItem SelectedItem
		{
			get
			{
				return base.SelectedItem;
			}
			set
			{
				if (this.SelectedItem == value)
				{
					return;
				}
				base.SelectedItem = value;
				this.RefreshPropertyDescriptors();
			}
		}
		public IObservableCollection<IProperty> PropertyDescriptors
		{
			get
			{
				return this._propertyDescriptors;
			}
		}
		public VerticalCollectionBrowserViewModel(IServiceProvider services, IActionMonitor actionMonitor, IPropertyInspectorService propertyInspectorService) : base(services, actionMonitor, propertyInspectorService)
		{
		}
		private void RefreshPropertyDescriptors()
		{
			this.PropertyDescriptors.Clear();
			if (this.SelectedItem == null)
			{
				return;
			}
			ICollection<IProperty> properties = this.propertyInspectorService.GetProperties(new object[]
			{
				this.SelectedItem.Item
			}) ?? new Collection<IProperty>();
			this.PropertyDescriptors.AddRange(properties);
		}
	}
}
