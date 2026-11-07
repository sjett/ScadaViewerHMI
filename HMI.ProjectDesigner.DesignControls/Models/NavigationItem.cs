// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Collections;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using HMI.ProjectDesigner.DesignControls.Resources;
using System.ComponentModel;
namespace HMI.Infrastructure.Models
{
	public class NavigationItem : CollectionItem
	{
		private bool isInValidation;
		private INavigationTargetViewModel target;
		private IExplorerNodeViewModel source;
		private readonly FullyObservableCollection<NavigationParameter> parameterValues = new FullyObservableCollection<NavigationParameter>();
		
		[Browsable(true), 
			DisplayName("Display Window"), 
			EditorOrder(1.0), 
			OptionListEditor("Targets"), 
			CustomMethodValidation("ValidateTarget", ValidationStep = System.Windows.Controls.ValidationStep.ConvertedProposedValue), 
			NotNull(ValidationStep = System.Windows.Controls.ValidationStep.RawProposedValue)]
		public INavigationTargetViewModel Target
		{
			get
			{
				return this.target;
			}
			set
			{
				if (this.target != value)
				{
					base.RaisePropertyChanging<INavigationTargetViewModel>(() => this.Target);
					this.target = value;
					base.RaisePropertyChanged<INavigationTargetViewModel>(() => this.Target);
					this.UpdateSources();
					base.UpdateEditorDisplayText();
				}
			}
		}
		protected ICollectionMonitor<NavigationParameter> ParameterValuesMonitor
		{
			get;
			set;
		}
		[Browsable(true), DisplayName("SubPage"), EditorOrder(2.0), OptionListEditor("Sources"), NotNull]
		public IExplorerNodeViewModel Source
		{
			get
			{
				return this.source;
			}
			set
			{
				if (this.source != value)
				{
					base.RaisePropertyChanging<IExplorerNodeViewModel>(() => this.Source);
					this.source = value;
					base.RaisePropertyChanged<IExplorerNodeViewModel>(() => this.Source);
					base.UpdateEditorDisplayText();
				}
			}
		}
		[Browsable(true), CollectionEditor(typeof(NavigationParameter), UseHorizontalEditing = true), EditorOrder(3.0)]
		public IObservableCollection<NavigationParameter> ParameterValues
		{
			get
			{
				return this.parameterValues;
			}
		}
		[Browsable(false)]
		public List<INavigationTargetViewModel> Targets
		{
			get;
			private set;
		}
		[Browsable(false)]
		public List<IProjectPageViewModel> Sources
		{
			get;
			private set;
		}
		public NavigationItem(IServiceProvider services, IExplorerNodeViewModel parentNode) : base(services, parentNode)
		{
			this.ParameterValuesMonitor = services.Resolve<ICollectionMonitor<NavigationParameter>>();
			this.ParameterValuesMonitor.Start(this.ParameterValues);

			INavigationViewModel? navigationViewModel = parentNode as INavigationViewModel;
			if (navigationViewModel != null)
			{
				this.Targets = new List<INavigationTargetViewModel>(navigationViewModel.NavigationTargets);
				return;
			}
			this.Targets = new List<INavigationTargetViewModel>();
		}
		public System.Windows.Controls.ValidationResult ValidateTarget(INavigationTargetViewModel target, System.Globalization.CultureInfo culture)
		{
			System.Windows.Controls.ValidationResult result;
			try
			{
				if (base.ParentCollection == null)
				{
					result = System.Windows.Controls.ValidationResult.ValidResult;
				}
				else
				{
					System.Collections.Generic.IEnumerable<NavigationItem> enumerable = base.ParentCollection.OfType<NavigationItem>();
					if (enumerable.Any((NavigationItem i) => i != this && i.Target == target))
					{
						result = new System.Windows.Controls.ValidationResult(false, ErrorTexts.NavigationTargetCanOnlyAppearOnce);
					}
					else
					{
						result = System.Windows.Controls.ValidationResult.ValidResult;
					}
				}
			}
			finally
			{
				if (!this.isInValidation)
				{
					this.isInValidation = true;
					//base.RaisePropertyChanged("");
					this.isInValidation = false;
				}
			}
			return result;
		}
		public override string ToString()
		{
			string targetId = (this.target != null) ? this.target.ID : "no target";
			string sourceId = (this.source != null) ? this.source.ID : "no source";
			return string.Format("{0} -> {1}", sourceId, targetId);
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			NavigationItem navigationItem = target as NavigationItem;
			navigationItem.Target = this.Target;
			navigationItem.Source = this.Source;
			navigationItem.ParameterValues.Clear();
			navigationItem.ParameterValues.AddRange((
				from v in this.ParameterValues
				select v.Clone()).Cast<NavigationParameter>());
		}
		public override ICollectionItem CreateClone()
		{
			NavigationItem navigationItem = new NavigationItem(base.Services, base.Parent);
			navigationItem.ParameterValuesMonitor.Stop();
			return navigationItem;
		}
		public override string GetXaml()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
			if (this.ParameterValues != null)
			{
				foreach (NavigationParameter current in this.ParameterValues)
				{
					stringBuilder.AppendLine(current.GetXaml());
				}
			}
			string targetId = string.Empty;
			if (this.Target != null)
			{
				targetId = this.Target.ID;
			}
			string sourceId = string.Empty;
			if (this.Source != null)
			{
				sourceId = this.Source.ID;
			}
			return string.Format("<{0}:{1} Target=\"{3}\" Source=\"{4}\">{2}<{0}:{1}.Parameters>{2}{5}</{0}:{1}.Parameters>{2}</{0}:{1}>", new object[]
			{
				"HMINavigation",
				"NavigationItem",
				System.Environment.NewLine,
				targetId,
				sourceId,
				stringBuilder
			});
		}
		private void UpdateSources()
		{
			if (this.Target == null)
			{
				this.Sources = new System.Collections.Generic.List<IProjectPageViewModel>();
			}
			else
			{
				this.Sources = this.Target.Children.OfType<IProjectPageViewModel>().ToList<IProjectPageViewModel>();
			}
			base.RaisePropertyChanged<System.Collections.Generic.List<IProjectPageViewModel>>(() => this.Sources);
		}
	}
}
