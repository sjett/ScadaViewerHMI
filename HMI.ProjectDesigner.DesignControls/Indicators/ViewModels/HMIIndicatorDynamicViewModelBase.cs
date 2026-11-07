// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.ComponentModel;
using System.IO;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.Indicators.ViewModels
{
	public abstract class HMIIndicatorDynamicViewModelBase : HMIIndicatorBaseViewModel
	{
		public const string DefaultTemplate = "\r\n\t<Grid   xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n\t\t\txmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\r\n\t\txmlns:localize=\"clr-namespace:HMI.Common.Presentation.Localization;assembly=HMI.Common.Presentation\"\r\n\t\t\txmlns:local=\"clr-namespace:HMI.ProjectDesigner.DesignControls.Indicators\"\r\n\t\t\txmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\" \r\n\t\t\txmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\r\n\t\t\tmc:Ignorable=\"d\">\r\n\t\t<Viewbox>\r\n\t\t<Grid Margin=\"0\" Height=\"85\" HorizontalAlignment=\"Left\" Width=\"85\" VerticalAlignment=\"Bottom\" d:LayoutOverrides=\"Height\">\r\n\t\t\t<Grid.RowDefinitions>\r\n\t\t\t\t<RowDefinition/>\r\n\t\t\t</Grid.RowDefinitions>\r\n\t\t\t<Viewbox>\r\n\t\t\t\t<Grid Width=\"85\" Height=\"85\">\r\n\t\t\t\t\t<Grid.RowDefinitions>\r\n\t\t\t\t\t\t<RowDefinition Height=\"0.7*\"/>\r\n\t\t\t\t\t\t<RowDefinition Height=\"0.3*\"/>\r\n\t\t\t\t\t</Grid.RowDefinitions>\r\n\t\t\t\t\t<Path Data=\"F1 M13.58,11.728 C13.58,11.728 22.139999,20.832001 22.139999,20.832001 C22.139999,20.832001 25.223,15.491 25.223,15.491 C25.223,15.491 32.988998,22.284 36.671001,25.459999 C36.671001,25.459999 7.7589998,25.459999 7.7589998,25.459999 C7.7589998,25.459999 7.7589998,22.493999 7.7589998,22.493999 C7.7589998,22.493999 13.58,11.728 13.58,11.728 z M32.192001,8.1680002 C33.798,8.1680002 35.098999,9.4700003 35.098999,11.074 C35.098999,12.68 33.798,13.98 32.192001,13.98 C30.59,13.98 29.288,12.68 29.288,11.074 C29.288,9.4700003 30.59,8.1680002 32.192001,8.1680002 z M4.9349685,4.741189 C4.9349685,4.741189 4.9369688,28.381815 4.9369688,28.381815 C4.9369688,28.381815 41.192032,28.381815 41.192032,28.381815 C41.192032,28.381815 41.06403,4.741189 41.06403,4.741189 C41.06403,4.741189 4.9349685,4.741189 4.9349685,4.741189 z M1.266,1.171 C1.266,1.171 44.734001,1.171 44.734001,1.171 C44.734001,1.171 44.734001,31.827999 44.734001,31.827999 C44.734001,31.827999 1.266,31.827999 1.266,31.827999 C1.266,31.827999 1.266,1.171 1.266,1.171 z\" UseLayoutRounding=\"False\" Stretch=\"Fill\" Stroke=\"#FF5E5E5E\" Fill=\"#FFF1F1F1\" StrokeThickness=\"1.5\"/>\r\n\t\t\t\t\t<Path Data=\"M58.083984,34.748993 L60.083984,34.748993 L60.083984,36.748993 L58.083984,36.748993 z M0.5,34.748993 L2.5,34.748993 L2.5,36.748993 L0.5,36.748993 z M0.5,29.082001 L2.5,29.082001 L2.5,31.082001 L0.5,31.082001 z M28.666992,25.917999 L30.666992,25.917999 L30.666992,27.917999 L28.666992,27.917999 z M34.832977,14.583008 L36.832977,14.583008 L36.832977,16.583008 L34.832977,16.583008 z M48.665985,12.251007 L50.665985,12.251007 L50.665985,14.251007 L48.665985,14.251007 z M11.791992,7.1650085 L13.791992,7.1650085 L13.791992,9.1650085 L11.791992,9.1650085 z M54.541992,6.0830078 L56.541992,6.0830078 L56.541992,8.0830078 L54.541992,8.0830078 z M42.539978,6.0830078 L44.539978,6.0830078 L44.539978,8.0830078 L42.539978,8.0830078 z M48.665985,0.5 L50.665985,0.5 L50.665985,2.5 L48.665985,2.5 z\" Fill=\"#FF979797\" Margin=\"11.502,13,13.914,11.751\" Stretch=\"Fill\" Stroke=\"#FF505050\" UseLayoutRounding=\"False\"/>\r\n\t\t\t\t\t<TextBlock TextWrapping=\"Wrap\" Text=\"{localize:Resx ResxName=HMI.ProjectDesigner.DesignControls.Resources.ErrorTexts, Key=NoXAML}\" Foreground=\"#FF444444\" FontFamily=\"Verdana\" TextAlignment=\"Center\" Grid.Row=\"1\" VerticalAlignment=\"Center\" HorizontalAlignment=\"Center\" FontSize=\"10\"/>\r\n\t\t\t\t</Grid>\r\n\t\t\t</Viewbox>\r\n\t\t</Grid>\r\n\t\t</Viewbox>\r\n\t\t</Grid>";
		public const string InvalidTemplate = "\r\n\t<Grid   xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n\t\t\txmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\r\n\t\txmlns:localize=\"clr-namespace:HMI.Common.Presentation.Localization;assembly=HMI.Common.Presentation\"\r\n\t\t\txmlns:local=\"clr-namespace:HMI.ProjectDesigner.DesignControls.Indicators\"\r\n\t\t\txmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\" \r\n\t\t\txmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\r\n\t\t\tmc:Ignorable=\"d\">\r\n\t\t<Viewbox>\r\n\t\t<Grid Margin=\"0\" Height=\"85\" VerticalAlignment=\"Bottom\" d:LayoutOverrides=\"Height\">\r\n\t\t\t<Grid.RowDefinitions>\r\n\t\t\t\t<RowDefinition/>\r\n\t\t\t</Grid.RowDefinitions>\r\n\t\t\t<Viewbox>\r\n\t\t\t\t<Grid Width=\"85\" Height=\"85\">\r\n\t\t\t\t\t<Grid.RowDefinitions>\r\n\t\t\t\t\t\t<RowDefinition Height=\"0.7*\"/>\r\n\t\t\t\t\t\t<RowDefinition Height=\"0.3*\"/>\r\n\t\t\t\t\t</Grid.RowDefinitions>\r\n\t\t\t\t\t<Path Data=\"F1 M4.9349685,4.741189 C4.9349685,4.741189 4.9369688,28.381815 4.9369688,28.381815 C4.9369688,28.381815 41.192032,28.381815 41.192032,28.381815 C41.192032,28.381815 41.06403,4.741189 41.06403,4.741189 C41.06403,4.741189 4.9349685,4.741189 4.9349685,4.741189 z M1.266,1.171 C1.266,1.171 44.734001,1.171 44.734001,1.171 C44.734001,1.171 44.734001,31.827999 44.734001,31.827999 C44.734001,31.827999 1.266,31.827999 1.266,31.827999 C1.266,31.827999 1.266,1.171 1.266,1.171 z\" UseLayoutRounding=\"False\" Stretch=\"Fill\" Stroke=\"#FF6E0000\" Fill=\"#FFF1F1F1\" StrokeThickness=\"1.5\"/>\r\n\t\t\t\t\t<Path Data=\"F1M26.721,7.059C26.721,7.059 19.649,13.93 19.649,13.93 19.649,13.93 26.721,20.801 26.721,20.801 26.721,20.801 21.064,26.458 21.064,26.458 21.064,26.458 13.993,19.387 13.993,19.387 13.993,19.387 6.922,26.458 6.922,26.458 6.922,26.458 1.265,20.801 1.265,20.801 1.265,20.801 8.336,13.93 8.336,13.93 8.336,13.93 1.265,7.059 1.265,7.059 1.265,7.059 6.921,1.402 6.921,1.402 6.921,1.402 13.993,8.473 13.993,8.473 13.993,8.473 21.064,1.402 21.064,1.402 21.064,1.402 26.721,7.059 26.721,7.059z\" Fill=\"#FF6E0000\" UseLayoutRounding=\"False\" Width=\"28\" Height=\"28\" VerticalAlignment=\"Center\" HorizontalAlignment=\"Center\"/>\r\n\t\t\t\t\t<TextBlock TextWrapping=\"Wrap\" Text=\"{localize:Resx ResxName=HMI.ProjectDesigner.DesignControls.Resources.ErrorTexts, Key=InvalidXAML}\" Foreground=\"#FF6E0000\" FontFamily=\"Verdana\" TextAlignment=\"Center\" Grid.Row=\"1\" VerticalAlignment=\"Center\" HorizontalAlignment=\"Center\"/>\r\n\t\t\t\t</Grid>\r\n\t\t\t</Viewbox>\r\n\t\t</Grid>                    \r\n\t\t</Viewbox>\r\n\t\t</Grid>";
		private readonly IFileSystemService fileSystemService;
		private string defaultStateXaml;
		private string defaultStateXamlPath;
		private string stateXamlPath1;
		private string stateXamlPath2;
		private string stateXamlPath3;
		private string stateXamlPath4;
		private string stateXamlPath5;
		private string stateXamlPath6;
		private string stateXamlPath7;
		private string stateXamlPath8;

		public event System.EventHandler<ValueChangedEventArgs<string>> DefaultStateXamlChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> DefaultStateXamlPathChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateXamlPath1Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateXamlPath2Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateXamlPath3Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateXamlPath4Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateXamlPath5Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateXamlPath6Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateXamlPath7Changed;
		public event System.EventHandler<ValueChangedEventArgs<string>> StateXamlPath8Changed;

		[Browsable(false)]
		public string DefaultStateXaml
		{
			get
			{
				return this.defaultStateXaml;
			}
			set
			{
				if (this.defaultStateXaml != value)
				{
					base.RaisePropertyChanging<string>(() => this.DefaultStateXaml);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.defaultStateXaml, value);
					this.defaultStateXaml = value;
					this.DefaultStateXamlChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.DefaultStateXaml);
				}
			}
		}
		[MediaBrowser(MediaType.Xaml), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string DefaultStateXamlPath
		{
			get
			{
				return this.defaultStateXamlPath;
			}
			set
			{
				if (this.defaultStateXamlPath != value)
				{
					base.RaisePropertyChanging<string>(() => this.DefaultStateXamlPath);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.defaultStateXamlPath, value);
					this.defaultStateXamlPath = value;
					this.DefaultStateXamlPathChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.DefaultStateXamlPath);
					this.UpdateDefaultXaml();
				}
			}
		}
		[MediaBrowser(MediaType.Xaml), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string StateXamlPath1
		{
			get
			{
				return this.stateXamlPath1;
			}
			set
			{
				if (this.stateXamlPath1 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateXamlPath1, value);
					base.RaisePropertyChanging<string>(() => this.StateXamlPath1);
					this.stateXamlPath1 = value;
					this.StateXamlPath1Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateXamlPath1);
				}
			}
		}
		[MediaBrowser(MediaType.Xaml), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string StateXamlPath2
		{
			get
			{
				return this.stateXamlPath2;
			}
			set
			{
				if (this.stateXamlPath2 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateXamlPath2, value);
					base.RaisePropertyChanging<string>(() => this.StateXamlPath2);
					this.stateXamlPath2 = value;
					this.StateXamlPath2Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateXamlPath2);
				}
			}
		}
		[MediaBrowser(MediaType.Xaml), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string StateXamlPath3
		{
			get
			{
				return this.stateXamlPath3;
			}
			set
			{
				if (this.stateXamlPath3 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateXamlPath3, value);
					base.RaisePropertyChanging<string>(() => this.StateXamlPath3);
					this.stateXamlPath3 = value;
					this.StateXamlPath3Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateXamlPath3);
				}
			}
		}
		[MediaBrowser(MediaType.Xaml), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string StateXamlPath4
		{
			get
			{
				return this.stateXamlPath4;
			}
			set
			{
				if (this.stateXamlPath4 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateXamlPath4, value);
					base.RaisePropertyChanging<string>(() => this.StateXamlPath4);
					this.stateXamlPath4 = value;
					this.StateXamlPath4Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateXamlPath4);
				}
			}
		}
		[MediaBrowser(MediaType.Xaml), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string StateXamlPath5
		{
			get
			{
				return this.stateXamlPath5;
			}
			set
			{
				if (this.stateXamlPath5 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateXamlPath5, value);
					base.RaisePropertyChanging<string>(() => this.StateXamlPath5);
					this.stateXamlPath5 = value;
					this.StateXamlPath5Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateXamlPath5);
				}
			}
		}
		[MediaBrowser(MediaType.Xaml), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string StateXamlPath6
		{
			get
			{
				return this.stateXamlPath6;
			}
			set
			{
				if (this.stateXamlPath6 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateXamlPath6, value);
					base.RaisePropertyChanging<string>(() => this.StateXamlPath6);
					this.stateXamlPath6 = value;
					this.StateXamlPath6Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateXamlPath6);
				}
			}
		}
		[MediaBrowser(MediaType.Xaml), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string StateXamlPath7
		{
			get
			{
				return this.stateXamlPath7;
			}
			set
			{
				if (this.stateXamlPath7 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateXamlPath7, value);
					base.RaisePropertyChanging<string>(() => this.StateXamlPath7);
					this.stateXamlPath7 = value;
					this.StateXamlPath7Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateXamlPath7);
				}
			}
		}
		[MediaBrowser(MediaType.Xaml), PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string StateXamlPath8
		{
			get
			{
				return this.stateXamlPath8;
			}
			set
			{
				if (this.stateXamlPath8 != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.stateXamlPath8, value);
					base.RaisePropertyChanging<string>(() => this.StateXamlPath8);
					this.stateXamlPath8 = value;
					this.StateXamlPath8Changed.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.StateXamlPath8);
				}
			}
		}
		public HMIIndicatorDynamicViewModelBase(IServiceProvider container, IExplorerNodeViewModel parentValue, System.Type controlType) : base(container, parentValue, controlType)
		{
			this.fileSystemService = container.Resolve<IFileSystemService>();
		}
		private void UpdateDefaultXaml()
		{
			string fallbackXaml = "\r\n\t<Grid   xmlns=\"http://schemas.microsoft.com/winfx/2006/xaml/presentation\"\r\n\t\t\txmlns:x=\"http://schemas.microsoft.com/winfx/2006/xaml\"\r\n\t\t\txmlns:localize=\"clr-namespace:HMI.Common.Presentation.Localization;assembly=HMI.Common.Presentation\"\r\n\t\txmlns:local=\"clr-namespace:HMI.ProjectDesigner.DesignControls.Indicators\"\r\n\t\t\txmlns:mc=\"http://schemas.openxmlformats.org/markup-compatibility/2006\" \r\n\t\t\txmlns:d=\"http://schemas.microsoft.com/expression/blend/2008\"\r\n\t\t\tmc:Ignorable=\"d\">\r\n\t\t<Viewbox>\r\n\t\t<Grid Margin=\"0\" Height=\"85\" HorizontalAlignment=\"Left\" Width=\"85\" VerticalAlignment=\"Bottom\" d:LayoutOverrides=\"Height\">\r\n\t\t\t<Grid.RowDefinitions>\r\n\t\t\t\t<RowDefinition/>\r\n\t\t\t</Grid.RowDefinitions>\r\n\t\t\t<Viewbox>\r\n\t\t\t\t<Grid Width=\"85\" Height=\"85\">\r\n\t\t\t\t\t<Grid.RowDefinitions>\r\n\t\t\t\t\t\t<RowDefinition Height=\"0.7*\"/>\r\n\t\t\t\t\t\t<RowDefinition Height=\"0.3*\"/>\r\n\t\t\t\t\t</Grid.RowDefinitions>\r\n\t\t\t\t\t<Path Data=\"F1 M13.58,11.728 C13.58,11.728 22.139999,20.832001 22.139999,20.832001 C22.139999,20.832001 25.223,15.491 25.223,15.491 C25.223,15.491 32.988998,22.284 36.671001,25.459999 C36.671001,25.459999 7.7589998,25.459999 7.7589998,25.459999 C7.7589998,25.459999 7.7589998,22.493999 7.7589998,22.493999 C7.7589998,22.493999 13.58,11.728 13.58,11.728 z M32.192001,8.1680002 C33.798,8.1680002 35.098999,9.4700003 35.098999,11.074 C35.098999,12.68 33.798,13.98 32.192001,13.98 C30.59,13.98 29.288,12.68 29.288,11.074 C29.288,9.4700003 30.59,8.1680002 32.192001,8.1680002 z M4.9349685,4.741189 C4.9349685,4.741189 4.9369688,28.381815 4.9369688,28.381815 C4.9369688,28.381815 41.192032,28.381815 41.192032,28.381815 C41.192032,28.381815 41.06403,4.741189 41.06403,4.741189 C41.06403,4.741189 4.9349685,4.741189 4.9349685,4.741189 z M1.266,1.171 C1.266,1.171 44.734001,1.171 44.734001,1.171 C44.734001,1.171 44.734001,31.827999 44.734001,31.827999 C44.734001,31.827999 1.266,31.827999 1.266,31.827999 C1.266,31.827999 1.266,1.171 1.266,1.171 z\" UseLayoutRounding=\"False\" Stretch=\"Fill\" Stroke=\"#FF5E5E5E\" Fill=\"#FFF1F1F1\" StrokeThickness=\"1.5\"/>\r\n\t\t\t\t\t<Path Data=\"M58.083984,34.748993 L60.083984,34.748993 L60.083984,36.748993 L58.083984,36.748993 z M0.5,34.748993 L2.5,34.748993 L2.5,36.748993 L0.5,36.748993 z M0.5,29.082001 L2.5,29.082001 L2.5,31.082001 L0.5,31.082001 z M28.666992,25.917999 L30.666992,25.917999 L30.666992,27.917999 L28.666992,27.917999 z M34.832977,14.583008 L36.832977,14.583008 L36.832977,16.583008 L34.832977,16.583008 z M48.665985,12.251007 L50.665985,12.251007 L50.665985,14.251007 L48.665985,14.251007 z M11.791992,7.1650085 L13.791992,7.1650085 L13.791992,9.1650085 L11.791992,9.1650085 z M54.541992,6.0830078 L56.541992,6.0830078 L56.541992,8.0830078 L54.541992,8.0830078 z M42.539978,6.0830078 L44.539978,6.0830078 L44.539978,8.0830078 L42.539978,8.0830078 z M48.665985,0.5 L50.665985,0.5 L50.665985,2.5 L48.665985,2.5 z\" Fill=\"#FF979797\" Margin=\"11.502,13,13.914,11.751\" Stretch=\"Fill\" Stroke=\"#FF505050\" UseLayoutRounding=\"False\"/>\r\n\t\t\t\t\t<TextBlock TextWrapping=\"Wrap\" Text=\"{localize:Resx ResxName=HMI.ProjectDesigner.DesignControls.Resources.ErrorTexts, Key=NoXAML}\" Foreground=\"#FF444444\" FontFamily=\"Verdana\" TextAlignment=\"Center\" Grid.Row=\"1\" VerticalAlignment=\"Center\" HorizontalAlignment=\"Center\" FontSize=\"10\"/>\r\n\t\t\t\t</Grid>\r\n\t\t\t</Viewbox>\r\n\t\t</Grid>\r\n\t\t</Viewbox>\r\n\t\t</Grid>";
			if (!string.IsNullOrWhiteSpace(this.DefaultStateXamlPath))
			{
				fallbackXaml = this.LoadDefaultStateXaml(fallbackXaml);
			}
			this.DefaultStateXaml = fallbackXaml;
		}
		private string LoadDefaultStateXaml(string fallbackXaml)
		{
			string path = System.IO.Path.Combine(this.Project.ProjectXamlsFolder, this.DefaultStateXamlPath);
			try
			{
				fallbackXaml = this.LoadXamlFromFile(path, fallbackXaml);
			}
			catch
			{
			}
			return fallbackXaml;
		}
		private string LoadXamlFromFile(string path, string fallbackXaml)
		{
			string fileContent = this.fileSystemService.GetFileContent(path);
			if (!string.IsNullOrWhiteSpace(fileContent))
			{
				return fileContent;
			}
			return fallbackXaml;
		}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("DefaultStateXamlPath", this.DefaultStateXamlPath);
			base.AddXamlAttribute("StateXamlPath1", this.StateXamlPath1);
			base.AddXamlAttribute("StateXamlPath2", this.StateXamlPath2);
			base.AddXamlAttribute("StateXamlPath3", this.StateXamlPath3);
			base.AddXamlAttribute("StateXamlPath4", this.StateXamlPath4);
			base.AddXamlAttribute("StateXamlPath5", this.StateXamlPath5);
			base.AddXamlAttribute("StateXamlPath6", this.StateXamlPath6);
			base.AddXamlAttribute("StateXamlPath7", this.StateXamlPath7);
			base.AddXamlAttribute("StateXamlPath8", this.StateXamlPath8);
		}
	}
}
