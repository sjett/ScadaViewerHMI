// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Services;
using HMI.Modules.Design.Attributes;
using HMI.Modules.PropertyEditor.Resources;
using HMI.Common.Instrumentation.ErrorHandling;

namespace HMI.Modules.PropertyEditor.Services
{
	public class CompositeMetadataProvider : IExplicitPropertyMetadataProvider
	{
		private readonly IDictionary<Type, bool> _allowsMultipleByAttributeType = new Dictionary<Type, bool>();
        private readonly IList<IExplicitPropertyMetadataProvider> _metadataProviders = new List<IExplicitPropertyMetadataProvider>();
		
		public void AddProvider(IExplicitPropertyMetadataProvider metadataProvider)
		{
			Contract.Requires<ArgumentNullException>(metadataProvider != null, "metadataProvider");
            _metadataProviders.Add(metadataProvider);
		}
		public ICollection<Attribute> GetAttributes(string propertyName)
		{
			EnsureMetadataProviderExists();
			_allowsMultipleByAttributeType.Clear();

			var attributesWithProviders = _metadataProviders
                .SelectMany(
					t => t.GetAttributes(propertyName) ?? Enumerable.Empty<Attribute>(),
					(p, a) => (provider: p, providerAttribute: a))
				.Where(t => ShouldIncludeAttribute(t.providerAttribute));

		    return attributesWithProviders.Select(t => t.providerAttribute).ToList();
		}
		private void EnsureMetadataProviderExists()
        {
		    if (this._metadataProviders.Count == 0)
            {
                throw new InvalidOperationException(ErrorTexts.NoMetadataProviderDefined);
            }
        }
        private bool ShouldIncludeAttribute(Attribute attribute)
        {
            bool shouldInclude = false;
            bool allowsMultiple;
            Type attributeType = GetAttributeTypeKey(attribute);
            if (this._allowsMultipleByAttributeType.TryGetValue(attributeType, out allowsMultiple))
            {
                if (allowsMultiple)
                {
                    shouldInclude = true;
                }
            }
            else
            {
                this._allowsMultipleByAttributeType[attributeType] = AllowsMultipleAttributes(attributeType);
                shouldInclude = true;
            }
            return shouldInclude;
        }
        private Type GetAttributeTypeKey(Attribute attribute)
        {
            if (attribute is PropertyEditorAttribute)
            {
                return typeof(PropertyEditorAttribute);
            }
            return attribute.GetType();
        }
        private bool AllowsMultipleAttributes(Type attributeType)
        {
            IEnumerable<AttributeUsageAttribute> usages = attributeType.GetCustomAttributes(typeof(AttributeUsageAttribute), true).Cast<AttributeUsageAttribute>();

            return usages.Select(usage => usage.AllowMultiple).DefaultIfEmpty(false).First<bool>();
        }
		
	}
}
