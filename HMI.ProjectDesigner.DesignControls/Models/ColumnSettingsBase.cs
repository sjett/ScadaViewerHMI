// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.ComponentModel;
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
namespace HMI.Infrastructure.Models
{
	public abstract class ColumnSettingsBase : CollectionItem, INotifyPropertyChanged
	{
		private readonly IContentReplacementService contentReplacementService;
		private bool isAutoSize = true;
		private double width = 50.0;
		private bool isVisible = true;

		public event System.EventHandler<ValueChangedEventArgs<bool>> IsAutoSizeChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> WidthChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> IsVisibleChanged;

		[Browsable(true)]
		public bool IsAutoSize
		{
			get
			{
				return this.isAutoSize;
			}
			set
			{
				if (this.isAutoSize != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isAutoSize, value);
					base.RaisePropertyChanging<bool>(() => this.IsAutoSize);
					this.isAutoSize = value;
					this.IsAutoSizeChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.IsAutoSize);
				}
			}
		}
		[Browsable(true)]
		public double Width
		{
			get
			{
				return this.width;
			}
			set
			{
				if (this.width != value)
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.width, value);
					base.RaisePropertyChanging<double>(() => this.Width);
					this.width = value;
					this.WidthChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.Width);
				}
			}
		}
		[Browsable(true)]
		public bool IsVisible
		{
			get
			{
				return this.isVisible;
			}
			set
			{
				if (this.isVisible != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.isVisible, value);
					base.RaisePropertyChanging<bool>(() => this.IsVisible);
					this.isVisible = value;
					this.IsVisibleChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.IsVisible);
				}
			}
		}
		public abstract string TypeName
		{
			get;
		}
		public ColumnSettingsBase(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
			this.contentReplacementService = container.Resolve<IContentReplacementService>();
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			ColumnSettingsBase columnSettingsBase = target as ColumnSettingsBase;
			columnSettingsBase.IsAutoSize = this.IsAutoSize;
			columnSettingsBase.Width = this.Width;
			columnSettingsBase.IsVisible = this.IsVisible;
		}

		public override string GetXaml()
		{
			string xamlTemplate = this.GetXamlTemplate();
			return this.contentReplacementService.ReplaceStringContent(xamlTemplate, this);
		}

		protected abstract string GetXamlTemplate();

		public override string ToString()
		{
			return this.TypeName;
		}
	}
}
