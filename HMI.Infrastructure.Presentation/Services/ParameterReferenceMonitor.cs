// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Common.Instrumentation.Metadata;
using HMI.Domain.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using System.Collections.Specialized;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Text.RegularExpressions;
namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 参数引用监视器
    /// </summary>
	public class ParameterReferenceMonitor : IParameterReferenceMonitor
	{
		 private static readonly Regex TemplateParameterReferenceRegex = new Regex("\\[PC:(?<parameter>[^\\]]+)\\]", RegexOptions.Compiled | RegexOptions.CultureInvariant);
		 private readonly List<IParameterDefinition> _templateParameterDefinitions = new List<IParameterDefinition>();
		private readonly IPropertyInspectorService _propertyInspectorService;
		 private IHierarchicalNode _controlNode;
		private string[] _properties;
		 private ITemplatePageViewModel _parentTemplatePage;
		private bool _isDisposed;
		
		
		public string[] Properties
		{
			get
			{
				if (_properties == null)
				{
                    _properties = this.getTemplatePropertyNames(_controlNode);
				}
				return _properties;
			}
		}
		public ParameterReferenceMonitor(IHierarchicalNode control, IPropertyInspectorService propertyInspectorService)
		{
			Contract.Requires<ArgumentNullException>(control != null, "control");
			Contract.Requires<ArgumentNullException>(propertyInspectorService != null, "propertyInspectorService");

		    _controlNode = control;
		    this._propertyInspectorService = propertyInspectorService;
		    _parentTemplatePage = _controlNode.Parent as ITemplatePageViewModel;
		    _controlNode.ParentChanged += new EventHandler<ValueChangedEventArgs<IExplorerNodeViewModel>>(OnParentChanged);
			
		    Initialize();
		}
		private void Initialize()
		{
		    RefreshParentParameterDefinitions();
		    if (_parentTemplatePage == null)
		    {
		        return;
		    }
		    SubscribeToParameterNameChanges();
		    ValidateTemplateParameterReferences(_parentTemplatePage);
		    SubscribeToTemplateParameterCollectionChanges();
		}
		private void SubscribeToTemplateParameterCollectionChanges()
		{
		    _parentTemplatePage.TemplateParameters.CollectionChanged += OnTemplateParameterCollectionChanged;
		}
		private void RefreshParentParameterDefinitions()
		{
		    _templateParameterDefinitions.Clear();
		    if (_parentTemplatePage == null)
		    {
		        return;
		    }
		   _templateParameterDefinitions.AddRange(_parentTemplatePage.TemplateParameters);
		}
		private void SubscribeToParameterNameChanges()
		{
		    foreach (IParameterDefinition definition in _templateParameterDefinitions)
		    {
		        definition.NameChanged += OnParameterNameChanged;
		    }
		}
		private void ValidateTemplateParameterReferences(ITemplatePageViewModel parent)
		{
		    ValidatePropertyReferences(parent);
		}
		private void ValidatePropertyReferences(ITemplatePageViewModel parent)
		{
		    HashSet<string> parameterNames = GetTemplateParameterNames(parent);
		    string[] properties = Properties;
		    for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++)
		    {
		        string propertyName = properties[propertyIndex];
		        ClearUnknownParameterReference(propertyName, parameterNames);
		    }
		}
		private HashSet<string> GetTemplateParameterNames(ITemplatePageViewModel parent)
		{
		    return new HashSet<string>(parent.TemplateParameters.Select(parameter => parameter.Name));
		}
		private void ClearUnknownParameterReference(string propertyName, HashSet<string> parameterNames)
		{
		    string referencedParameterName = GetReferencedParameterName(propertyName);
		    if (referencedParameterName != null && !parameterNames.Contains(referencedParameterName))
		    {
		        SetPropertyValue(null, propertyName);
		    }
		}
		private string GetReferencedParameterName(string propertyName)
		{
		    string propertyValue = Reflection.GetPropertyValue(_controlNode, propertyName) as string;
		    if (string.IsNullOrWhiteSpace(propertyValue))
		    {
		        return null;
		    }
		    Match match = TemplateParameterReferenceRegex.Match(propertyValue);
		    if (!match.Success)
		    {
		        return null;
		    }
		    return GetParameterName(match);
		}
		private void SetPropertyValue(string value, string propertyName)
		{
		    Reflection.SetPropertyValue(_controlNode, propertyName, value);
		}
		private string GetParameterName(Match match)
		{
		    return match.Groups["parameter"].Value;
		}
		private void OnParentChanged(object sender, ValueChangedEventArgs<IExplorerNodeViewModel> e)
		{
		    _parentTemplatePage = e.NewValue as ITemplatePageViewModel;
			ResetParentTemplatePage(e.OldValue as ITemplatePageViewModel);
			Initialize();
		}
		private void ResetParentTemplatePage(ITemplatePageViewModel previousTemplatePage)
		{
		    UnsubscribeFromParameterNameChanges();
		    RefreshParentParameterDefinitions();
		    UnsubscribeFromTemplateParameterCollection(previousTemplatePage);
		}
		private void UnsubscribeFromParameterNameChanges()
		{
		    foreach (IParameterDefinition definition in _templateParameterDefinitions)
		    {
		        definition.NameChanged -= OnParameterNameChanged;
		    }
		}
		private void OnParameterNameChanged(object sender, ValueChangedEventArgs<string> e)
		{
			string[] properties = Properties;
			for (int propertyIndex = 0; propertyIndex < properties.Length; propertyIndex++)
			{
				string propertyName = properties[propertyIndex];
				UpdateParameterReference(e.OldValue, propertyName, e.NewValue);
			}
		}
		private void UpdateParameterReference(string oldParameterName, string propertyName, string newParameterName)
		{
		    string referencedParameterName = GetReferencedParameterName(propertyName);
		    if (referencedParameterName == oldParameterName)
		    {
		        SetPropertyValue("[PC:" + newParameterName + "]", propertyName);
		    }
		}
		private void UnsubscribeFromTemplateParameterCollection(ITemplatePageViewModel templatePage)
		{
		    if (templatePage != null)
		    {
		        templatePage.TemplateParameters.CollectionChanged -= OnTemplateParameterCollectionChanged;
		    }
		}
		private void OnTemplateParameterCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			UnsubscribeFromParameterNameChanges();
			RefreshParentParameterDefinitions();
			SubscribeToParameterNameChanges();
			ValidateTemplateParameterReferences(_controlNode.Parent as ITemplatePageViewModel);
		}

		protected virtual string[] getTemplatePropertyNames(object controlObject)
		{
			IEnumerable<Domain.Models.IProperty> properties = _propertyInspectorService.GetProperties(controlObject);

			return properties.Where(property => property.SupportsTemplating).Select(property => property.Name).ToArray();
		}
		public void Dispose()
		{
            Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void releaseControlNode()
		{
			if (_controlNode != null)
			{
                _controlNode.ParentChanged -= new EventHandler<ValueChangedEventArgs<IExplorerNodeViewModel>>(OnParentChanged);
				UnsubscribeFromTemplateParameterCollection(_parentTemplatePage);
				UnsubscribeFromParameterNameChanges();
                _templateParameterDefinitions.Clear();
                _controlNode = null;
			}
		}
		protected virtual void ReleaseOther()
		{
		}
		protected virtual void Dispose(bool disposable)
		{
			if (!_isDisposed)
			{
				if (disposable)
				{
                    releaseControlNode();
				}
                ReleaseOther();
			}
            _isDisposed = true;
		}
		
		
	}
}
