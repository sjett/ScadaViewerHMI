// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Infrastructure.Presentation.Resources;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Modules.Design.Attributes;
using HMI.Modules.Design.Attributes.Validation;
using System.ComponentModel;
using System.Net;

namespace HMI.Infrastructure.Models
{
	public class ConditionSignal : CollectionItem
	{
		private string signalName;
		private string parameterName;

		public event System.EventHandler<ValueChangedEventArgs<string>> SignalNameChanged;
		public event System.EventHandler<ValueChangedEventArgs<string>> ParameterNameChanged;

		[Browsable(true), SignalBrowser]
		public string SignalName
		{
			get
			{
				return this.signalName;
			}
			set
			{
				if (this.signalName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.signalName, value);
					base.RaisePropertyChanging<string>(() => this.SignalName);
					this.signalName = value;
					this.SignalNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.SignalName);
				}
			}
		}
		[CustomMethodValidation("ValidateUniqueName")]
		public string ParameterName
		{
			get
			{
				return this.parameterName;
			}
			set
			{
				if (this.parameterName != value)
				{
					ValueChangedEventArgs<string> e = new ValueChangedEventArgs<string>(this.parameterName, value);
					base.RaisePropertyChanging<string>(() => this.ParameterName);
					this.parameterName = value;
					this.ParameterNameChanged.Raise(this, e);
					base.RaisePropertyChanged<string>(() => this.ParameterName);
				}
			}
		}
		public System.Windows.Controls.ValidationResult ValidateUniqueName(string name, System.Globalization.CultureInfo culture)
		{
			if (base.ParentCollection == null)
			{
				return System.Windows.Controls.ValidationResult.ValidResult;
			}
			System.Collections.Generic.IEnumerable<ConditionSignal> source = base.ParentCollection.OfType<ConditionSignal>();
			if (source.Any((ConditionSignal i) => i != this && i.ParameterName == name))
			{
				return new System.Windows.Controls.ValidationResult(false, ErrorTexts.ParameterDefinitionNameMustBeUnique);
			}
			return System.Windows.Controls.ValidationResult.ValidResult;
		}
		public ConditionSignal(IServiceProvider container, IExplorerNodeViewModel parentNode) : base(container, parentNode)
		{
			this.ParameterNameChanged += new System.EventHandler<ValueChangedEventArgs<string>>(this.ConditionSignal_ParameterNameChanged);
		}
		private void ConditionSignal_ParameterNameChanged(object sender, ValueChangedEventArgs<string> e)
		{
			base.UpdateEditorDisplayText();
		}
		public override void DeepCopyFields(ICollectionItem target)
		{
			ConditionSignal conditionSignal = target as ConditionSignal;
			conditionSignal.ParameterName = this.ParameterName;
			conditionSignal.SignalName = this.SignalName;
		}
		public override ICollectionItem CreateClone()
		{
			return new ConditionSignal(base.Services, base.Parent);
		}
		public override string GetXaml()
		{
			string signalNameXaml = this.GetSignalNameXaml();
			return string.Format("<{0}:{1} ParameterName=\"{2}\" SignalName=\"{3}\"></{0}:{1}>", new object[]
			{
				"HMIShared",
				"ConditionSignalItem",
				this.ParameterName,
				signalNameXaml
			});
		}
		private string GetSignalNameXaml()
		{
			return WebUtility.HtmlEncode(this.SignalName ?? string.Empty);
		}
		public override string ToString()
		{
			return this.ParameterName ?? GeneralTexts.NoName;
		}
	}
}
