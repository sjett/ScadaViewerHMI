// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.ViewModels;
using System.ComponentModel;

namespace HMI.ProjectDesigner.DesignControls.StaticSymbols.ViewModels
{
	public class StaticSymbolViewModel : CommonControlViewModel, IStaticSymbolViewModel
	{
		private string contentString;

		public event System.EventHandler<ValueChangedEventArgs<string>> ContentStringChanged;

		[Browsable(false)]
		public string ContentString
		{
			get
			{
				return this.contentString;
			}
			set
			{
				if (this.contentString != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.contentString, value);
					this.contentString = value;
					this.ContentStringChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ContentString);
				}
			}
		}
		public override System.Windows.Size DesignerDesiredSize
		{
			get
			{
				return new System.Windows.Size(50.0, 50.0);
			}
		}
		public StaticSymbolViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) 
			: base(container, parentValue, controlType)
		{
		}
		public override string GetXamlControlName()
		{
			return "ContentControl";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return Enumerable.Empty<string>();
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return Enumerable.Empty<AssemblyReference>();
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("HorizontalContentAlignment", "Stretch");
			base.AddXamlAttribute("VerticalContentAlignment", "Stretch");
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			collectionXaml.AppendFormat("<ContentControl.Content>{1}{0}{1}</ContentControl.Content>", this.contentString, System.Environment.NewLine);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
