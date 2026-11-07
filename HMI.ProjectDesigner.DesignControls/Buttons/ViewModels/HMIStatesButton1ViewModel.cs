// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
using System.ComponentModel;
namespace HMI.ProjectDesigner.DesignControls.Buttons.ViewModels
{
	public class HMIStatesButton1ViewModel : HMIStatesButtonBaseViewModel
	{
		private string iconImage;
		private double value = 1.0;
		public event System.EventHandler<ValueChangedEventArgs<string>> IconImageChanged;
		public event System.EventHandler<ValueChangedEventArgs<double>> ValueChanged;
		[Browsable(true), MediaBrowser(MediaType.Image), PropertyCategory("Icons", typeof(StandardPropertyCategories))]
		public string IconImage
		{
			get
			{
				return this.iconImage;
			}
			set
			{
				if (this.iconImage != value)
				{
					base.RaisePropertyChanging<string>(() => this.IconImage);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.iconImage, value);
					this.iconImage = value;
					this.IconImageChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.IconImage);
				}
			}
		}
		[Browsable(true), PropertyCategory("General", typeof(StandardPropertyCategories))]
		public double Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (!object.Equals(this.value, value))
				{
					ValueChangedEventArgs<double> e = new ValueChangedEventArgs<double>(this.value, value);
					base.RaisePropertyChanging<double>(() => this.Value);
					this.value = value;
					this.ValueChanged.Raise(this, e);
					base.RaisePropertyChanged<double>(() => this.Value);
				}
			}
		}
		public HMIStatesButton1ViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "HMIStatesButton1";
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("IconImage", ProjectFilesResolver.Resolve(this.IconImage, MediaType.Image));
			base.AddXamlAttribute("Value", this.Value);
		}
	}
}
