// SPDX-License-Identifier: GPL-3.0-only

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Media;
using HMI.Common.Instrumentation.Events;
using HMI.Domain.Models;
using HMI.Infrastructure;
using HMI.Infrastructure.Models;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Resources;
using HMI.Modules.Design.Attributes;
namespace HMI.ProjectDesigner.DesignControls.General.ViewModels
{
	public class HMILogbookViewModel : CommonControlViewModel
	{
		public class EntryViewModel
		{
			public int EntryNumber
			{
				get;
				set;
			}
			public string Sender
			{
				get;
				set;
			}
			public string TimeStamp
			{
				get;
				set;
			}
			public string Subject
			{
				get;
				set;
			}
			public string Body
			{
				get;
				set;
			}
		}
		private string initialConfiguration;
		private bool clientSideConfiguration;
		private ObservableCollection<LogbookSecurityPolicy> securityPolicies = new ObservableCollection<LogbookSecurityPolicy>();
		private Brush scrollbarForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAAAAAA")).GetAsFrozen();
		private Brush scrollbarBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF")).GetAsFrozen();
		private Brush dialogsForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF787878")).GetAsFrozen();
		private Brush dialogsBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF")).GetAsFrozen();
		private Brush buttonForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF3F3F3F")).GetAsFrozen();
		private Brush buttonBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFE0E0E0")).GetAsFrozen();
		private Brush headerBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF")).GetAsFrozen();
		private Brush senderBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD2E2EA")).GetAsFrozen();
		private Brush subjectBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFD8E8F0")).GetAsFrozen();
		private Brush bodyBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF")).GetAsFrozen();
		private Brush footerBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF")).GetAsFrozen();
		private Brush toolBarBackground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFFFFFFF")).GetAsFrozen();
		private Brush topicForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF787878")).GetAsFrozen();
		private Brush timeFilterForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF787878")).GetAsFrozen();
		private Brush senderForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF676767")).GetAsFrozen();
		private Brush timeStampForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF676767")).GetAsFrozen();
		private Brush subjectForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF676788")).GetAsFrozen();
		private Brush bodyForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF787878")).GetAsFrozen();
		private Brush footerForeground = (Brush)new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF787878")).GetAsFrozen();
		private ObservableCollection<EntryViewModel> entries = GetSampleEntries();
		
		public event EventHandler<ValueChangedEventArgs<string>> InitialConfigurationChanged;
		public event EventHandler<ValueChangedEventArgs<bool>> ClientSideConfigurationChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> ScrollbarForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> ScrollbarBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> DialogsForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> DialogsBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> ButtonForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> ButtonBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> HeaderBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> SenderBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> SubjectBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> BodyBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> FooterBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> ToolBarBackgroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> TopicForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> TimeFilterForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> SenderForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> TimeStampForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> SubjectForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> BodyForegroundChanged;
		public event EventHandler<ValueChangedEventArgs<Brush>> FooterForegroundChanged;
		public override Size DesignerDesiredSize
		{
			get
			{
				return new Size(500.0, 550.0);
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public string InitialConfiguration
		{
			get
			{
				return initialConfiguration;
			}
			set
			{
				if (initialConfiguration != value)
				{
					RaisePropertyChanging(() => InitialConfiguration);
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(initialConfiguration, value);
					initialConfiguration = value;
					InitialConfigurationChanged.Raise(this, e);
					RaisePropertyChanged(() => InitialConfiguration);
				}
			}
		}
		[PropertyCategory("Configuration", typeof(StandardPropertyCategories))]
		public bool ClientSideConfiguration
		{
			get
			{
				return clientSideConfiguration;
			}
			set
			{
				if (clientSideConfiguration != value)
				{
					RaisePropertyChanging(() => ClientSideConfiguration);
					ValueChangedEventArgs<bool> e = new ValueChangedEventArgs<bool>(clientSideConfiguration, value);
					clientSideConfiguration = value;
					ClientSideConfigurationChanged.Raise(this, e);
					RaisePropertyChanged(() => ClientSideConfiguration);
				}
			}
		}
		[CollectionEditor(typeof(LogbookSecurityPolicy), IsCollectionReadOnly = true), PropertyCategory("Security", typeof(StandardPropertyCategories))]
		public ObservableCollection<LogbookSecurityPolicy> SecurityPolicies
		{
			get
			{
				return securityPolicies;
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush ScrollbarForeground
		{
			get
			{
				return scrollbarForeground;
			}
			set
			{
				Brush brush = scrollbarForeground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => ScrollbarForeground);
				scrollbarForeground = value;
				ScrollbarForegroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => ScrollbarForeground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush ScrollbarBackground
		{
			get
			{
				return scrollbarBackground;
			}
			set
			{
				Brush brush = scrollbarBackground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => ScrollbarBackground);
				scrollbarBackground = value;
				ScrollbarBackgroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => ScrollbarBackground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush DialogsForeground
		{
			get
			{
				return dialogsForeground;
			}
			set
			{
				Brush brush = dialogsForeground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => DialogsForeground);
				dialogsForeground = value;
				DialogsForegroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => DialogsForeground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush DialogsBackground
		{
			get
			{
				return dialogsBackground;
			}
			set
			{
				Brush brush = dialogsBackground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => DialogsBackground);
				dialogsBackground = value;
				DialogsBackgroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => DialogsBackground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush ButtonForeground
		{
			get
			{
				return buttonForeground;
			}
			set
			{
				Brush brush = buttonForeground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => ButtonForeground);
				buttonForeground = value;
				ButtonForegroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => ButtonForeground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush ButtonBackground
		{
			get
			{
				return buttonBackground;
			}
			set
			{
				Brush brush = buttonBackground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => ButtonBackground);
				buttonBackground = value;
				ButtonBackgroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => ButtonBackground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush HeaderBackground
		{
			get
			{
				return headerBackground;
			}
			set
			{
				Brush brush = headerBackground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => HeaderBackground);
				headerBackground = value;
				HeaderBackgroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => HeaderBackground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush SenderBackground
		{
			get
			{
				return senderBackground;
			}
			set
			{
				Brush brush = senderBackground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => SenderBackground);
				senderBackground = value;
				SenderBackgroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => SenderBackground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush SubjectBackground
		{
			get
			{
				return subjectBackground;
			}
			set
			{
				Brush brush = subjectBackground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => SubjectBackground);
				subjectBackground = value;
				SubjectBackgroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => SubjectBackground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush BodyBackground
		{
			get
			{
				return bodyBackground;
			}
			set
			{
				Brush brush = bodyBackground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => BodyBackground);
				bodyBackground = value;
				BodyBackgroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => BodyBackground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush FooterBackground
		{
			get
			{
				return footerBackground;
			}
			set
			{
				Brush brush = footerBackground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => FooterBackground);
				footerBackground = value;
				FooterBackgroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => FooterBackground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush ToolBarBackground
		{
			get
			{
				return toolBarBackground;
			}
			set
			{
				Brush brush = toolBarBackground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => ToolBarBackground);
				toolBarBackground = value;
				ToolBarBackgroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => ToolBarBackground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush TopicForeground
		{
			get
			{
				return topicForeground;
			}
			set
			{
				Brush brush = topicForeground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => TopicForeground);
				topicForeground = value;
				TopicForegroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => TopicForeground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush TimeFilterForeground
		{
			get
			{
				return timeFilterForeground;
			}
			set
			{
				Brush brush = timeFilterForeground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => TimeFilterForeground);
				timeFilterForeground = value;
				TimeFilterForegroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => TimeFilterForeground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush SenderForeground
		{
			get
			{
				return senderForeground;
			}
			set
			{
				Brush brush = senderForeground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => SenderForeground);
				senderForeground = value;
				SenderForegroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => SenderForeground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush TimeStampForeground
		{
			get
			{
				return timeStampForeground;
			}
			set
			{
				Brush brush = timeStampForeground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => TimeStampForeground);
				timeStampForeground = value;
				TimeStampForegroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => TimeStampForeground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush SubjectForeground
		{
			get
			{
				return subjectForeground;
			}
			set
			{
				Brush brush = subjectForeground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => SubjectForeground);
				subjectForeground = value;
				SubjectForegroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => SubjectForeground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush BodyForeground
		{
			get
			{
				return bodyForeground;
			}
			set
			{
				Brush brush = bodyForeground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => BodyForeground);
				bodyForeground = value;
				BodyForegroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => BodyForeground);
			}
		}
		[PropertyCategory("Appearance", typeof(StandardPropertyCategories))]
		public Brush FooterForeground
		{
			get
			{
				return footerForeground;
			}
			set
			{
				Brush brush = footerForeground;
				if (brush == value)
				{
					return;
				}
				RaisePropertyChanging(() => FooterForeground);
				footerForeground = value;
				FooterForegroundChanged.Raise(this, new ValueChangedEventArgs<Brush>(brush, value));
				RaisePropertyChanged(() => FooterForeground);
			}
		}
		[Browsable(false)]
		public string Topic
		{
			get
			{
				return "Topic";
			}
		}
		[Browsable(false)]
		public string DisplayTimeFilter
		{
			get
			{
				return "Time Filter";
			}
		}
		[Browsable(false)]
		public ObservableCollection<EntryViewModel> Entries
		{
			get
			{
				return entries;
			}
		}
		public HMILogbookViewModel(IServiceProvider container, IExplorerNodeViewModel parentValue, Type controlType) : base(container, parentValue, controlType)
		{
			FillSecurityPolicies();
		}
		private void FillSecurityPolicies()
		{
			SecurityPolicies.Clear();
			SecurityPolicies.Add(new LogbookSecurityPolicy(Services, Parent, LogbookSecuredButtons.NewMessage));
			SecurityPolicies.Add(new LogbookSecurityPolicy(Services, Parent, LogbookSecuredButtons.Export));
			SecurityPolicies.Add(new LogbookSecurityPolicy(Services, Parent, LogbookSecuredButtons.Print));
			SecurityPolicies.Add(new LogbookSecurityPolicy(Services, Parent, LogbookSecuredButtons.Configuration));
			SecurityPolicies.Add(new LogbookSecurityPolicy(Services, Parent, LogbookSecuredButtons.LoadConfiguration));
			SecurityPolicies.Add(new LogbookSecurityPolicy(Services, Parent, LogbookSecuredButtons.SaveConfiguration));
			SecurityPolicies.Add(new LogbookSecurityPolicy(Services, Parent, LogbookSecuredButtons.Refresh));
			SecurityPolicies.Add(new LogbookSecurityPolicy(Services, Parent, LogbookSecuredButtons.EditTemplate));
		}
		public override string GetXamlControlName()
		{
			return "HMILogbook";
		}
		public override System.Collections.Generic.IEnumerable<string> GetNamespaces()
		{
			return new string[]
			{
                "HMILogbook=\"clr-namespace:HMI.RuntimeControls.General.Views;assembly=HMI.RuntimeControls\""
            };
		}
		//public override System.Collections.Generic.IEnumerable<AssemblyReference> GetRequiredAssemblies()
		//{
		//	return AssemblyGroups.HMIGeneral;
		//}
		protected override void AddXamlAttributes()
		{
			base.AddXamlAttributes();
			base.AddXamlAttribute("InitialConfiguration", this.InitialConfiguration);
			base.AddXamlAttribute("ClientSideConfiguration", this.ClientSideConfiguration);
			base.AddXamlAttribute("ScrollbarBackground", this.ScrollbarBackground);
			base.AddXamlAttribute("ScrollbarForeground", this.ScrollbarForeground);
			base.AddXamlAttribute("DialogsBackground", this.DialogsBackground);
			base.AddXamlAttribute("DialogsForeground", this.DialogsForeground);
			base.AddXamlAttribute("ButtonBackground", this.ButtonBackground);
			base.AddXamlAttribute("ButtonForeground", this.ButtonForeground);
			base.AddXamlAttribute("HeaderBackground", this.HeaderBackground);
			base.AddXamlAttribute("TopicForeground", this.TopicForeground);
			base.AddXamlAttribute("TimeFilterForeground", this.TimeFilterForeground);
			base.AddXamlAttribute("SenderBackground", this.SenderBackground);
			base.AddXamlAttribute("SenderForeground", this.SenderForeground);
			base.AddXamlAttribute("TimeStampForeground", this.TimeStampForeground);
			base.AddXamlAttribute("SubjectBackground", this.SubjectBackground);
			base.AddXamlAttribute("SubjectForeground", this.SubjectForeground);
			base.AddXamlAttribute("BodyBackground", this.BodyBackground);
			base.AddXamlAttribute("BodyForeground", this.BodyForeground);
			base.AddXamlAttribute("FooterBackground", this.FooterBackground);
			base.AddXamlAttribute("FooterForeground", this.FooterForeground);
			base.AddXamlAttribute("ToolBarBackground", this.ToolBarBackground);
		}
		protected override void AddXamlCollections(System.Text.StringBuilder collectionXaml)
		{
			base.AddXamlCollections(collectionXaml);
			this.AddSecurityPolicies(collectionXaml);
		}
		private void AddSecurityPolicies(System.Text.StringBuilder collectionXaml)
		{
			string namespacePrefix = base.GetNamespacePrefix();
			string xamlControlName = this.GetXamlControlName();
			base.AddCollectionOpenElement(collectionXaml, namespacePrefix, xamlControlName, "SecurityPolicies");
			base.AddXamlCollection(collectionXaml, "LogbookSecurityPolicy", this.SecurityPolicies, false);
			base.AddCollectionCloseElement(collectionXaml, namespacePrefix, xamlControlName, "SecurityPolicies");
		}
		protected override void ShowHideProperties()
		{
			base.ShowHideProperties();
			IsBackgroundVisible = false;
			IsForegroundVisible = false;
		}
		private static ObservableCollection<EntryViewModel> GetSampleEntries()
		{
			ObservableCollection<EntryViewModel> observableCollection = new ObservableCollection<EntryViewModel>();
			for (int i = 1; i <= 3; i++)
			{
				observableCollection.Add(new EntryViewModel
				{
					EntryNumber = i,
					Sender = GetSampleUser(i),
					TimeStamp = GetSampleTimeStamp(i),
					Subject = GetSampleSubject(i),
					Body = GetSampleBody(i)
				});
			}
			return observableCollection;
		}
		private static string GetSampleUser(int i)
		{
			return "操作员" + i;
		}
		private static string GetSampleTimeStamp(int i)
		{
			return DateTime.Now.AddDays(-30 * i).AddHours(-7 * i).AddMinutes(-17 * i).ToString("G");
		}
		private static string GetSampleSubject(int i)
		{
			return "主题  " + i;
		}
		private static string GetSampleBody(int i)
		{
			int num = i % 3;
			string result;
			if (num == 1)
			{
				result = "这对他来说将是痛苦的，他将被视为无助的，但与此同时，他将遭受苦难和巨大的痛苦。只要我走到最后，谁会为我们的锻炼而努力，除了他们中的任何一个人都会成功。第二，还是以愉快的方式上床睡觉，避免成为痛苦的面纱。除非他们被杀，否则他们不会被抛弃，他们将失去工作。";
			}
			else
			{
				if (num == 2)
				{
					result = "但在那之后，我们彼此不认识，我们不必担心，对吧？但他们也不喜欢互相介绍自己。";
				}
				else
				{
					result = "但当你看到这个孩子的错误在哪里时，我会打开整个故事，并作为一个幸福生活的建筑师来解释。\r\n没有人愿意感到无聊、憎恨或逃跑，但因为那些不知道意愿的人会承受巨大的痛苦。\r\n任何受苦的人都不会被认为处于痛苦之中，\r\n他想记住，但因为他从未开始受苦或受苦。";
				}
			}
			return result;
		}
	}
}
