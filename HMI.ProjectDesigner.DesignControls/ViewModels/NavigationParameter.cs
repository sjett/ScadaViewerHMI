// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes.Validation;
using System.ComponentModel;
namespace HMI.Infrastructure.Presentation.ViewModels
{
	public class NavigationParameter : CollectionItem
	{
		private string name;
		private string value;
		private readonly IContentReplacementService contentReplacementService;

		public event System.EventHandler<ValueChangedEventArgs<string>> NameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ValueChanged;

		[Browsable(true), NotNull]
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				if (this.name != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.name, value);
					base.RaisePropertyChanging<string>(() => this.Name);
					this.name = value;
					this.NameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.Name);
					base.UpdateEditorDisplayText();
				}
			}
		}
		[Browsable(true)]
		public string Value
		{
			get
			{
				return this.value;
			}
			set
			{
				if (this.value != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.value, value);
					base.RaisePropertyChanging<string>(() => this.Value);
					this.value = value;
					this.ValueChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.Value);
				}
			}
		}
		public NavigationParameter(IServiceProvider services, IExplorerNodeViewModel parentNode) : base(services, parentNode)
		{
			this.contentReplacementService = services.Resolve<IContentReplacementService>();
		}
		public NavigationParameter(IServiceProvider services, ICollectionItem parentCollectionItem) : this(services, parentCollectionItem.Parent)
		{
		}
		public override string ToString()
		{
			return this.Name ?? base.ToString();
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			NavigationParameter navigationParameter = target as NavigationParameter;
			navigationParameter.Name = this.Name;
			navigationParameter.Value = this.Value;
		}
		public override ICollectionItem CreateClone()
		{
			return new NavigationParameter(base.Services, base.Parent);
		}
		public override string GetXaml()
		{
			string text = string.Format("<{0}:{1} ParameterName=\"[Name]\" ParameterValue=\"[Value]\"></{0}:{1}>", "HMIShared", "ParameterItem");
			return this.contentReplacementService.ReplaceStringContent(text, this);
		}
	}
}
