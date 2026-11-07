// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.General.ViewModels
{
	public class HMIWebViewModel : CommonControlViewModel
	{
		private string source;
		private bool testUrl = false;

		public event System.EventHandler<ValueChangedEventArgs<string>> SourceChanged;
		public event System.EventHandler<ValueChangedEventArgs<bool>> TestUrlChanged;

		[Browsable(true), PropertyCategory("Url", typeof(StandardPropertyCategories))]
		public string Source
		{
			get
			{
				return this.source;
			}
			set
			{
				if (this.source != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.source, value);
					base.RaisePropertyChanging<string>(() => this.Source);
					this.source = value;
					this.SourceChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.Source);
				}
			}
		}
		[Browsable(true), PropertyCategory("Url", typeof(StandardPropertyCategories))]
		public bool TestUrl
		{
			get
			{
				return this.testUrl;
			}
			set
			{
                if (this.testUrl != value)
				{
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(this.testUrl, value);
					base.RaisePropertyChanging<bool>(() => this.TestUrl);
					this.testUrl = value;
					this.TestUrlChanged.Raise(this, e);
					base.RaisePropertyChanged<bool>(() => this.TestUrl);
				}
			}
		}

		public HMIWebViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.SourceChanged += new System.EventHandler<ValueChangedEventArgs<string>>(this.HMIWebViewModel_SourceChanged);
		}
		private void HMIWebViewModel_SourceChanged(object sender, ValueChangedEventArgs<string> e)
		{
			if (string.IsNullOrEmpty(e.NewValue))
				return;
			if (!this.TestUrl)
				return;

			//var iswell = Uri.IsWellFormedUriString(e.NewValue, UriKind.RelativeOrAbsolute);

		}

		public override string GetXamlControlName()
		{
			return "HMIWeb";
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return Enumerable.Empty<AssemblyReference>();
		//}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
            return new string[]
            {
                "HMIWeb=\"clr-namespace:HMI.RuntimeControls.General;assembly=HMI.RuntimeControls\""
            };
        }
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("Source", this.source);
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			base.IsBackgroundVisible = false;
			base.IsForegroundVisible = false;
		}
	}
}
