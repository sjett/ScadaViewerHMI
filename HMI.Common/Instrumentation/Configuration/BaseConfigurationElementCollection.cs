// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
namespace HMI.Common.Instrumentation.Configuration
{
    public abstract class BaseConfigurationElementCollection<TConfigurationElementType> : ConfigurationElementCollection, IList<TConfigurationElementType>, ICollection<TConfigurationElementType>, IEnumerable<TConfigurationElementType>, IEnumerable where TConfigurationElementType : ConfigurationElement, IConfigurationElementCollectionElement, new()
    {
        bool ICollection<TConfigurationElementType>.IsReadOnly
        {
            get
            {
                return IsReadOnly();
            }
        }
        public TConfigurationElementType this[int index]
        {
            get
            {
                return (TConfigurationElementType)(object)BaseGet(index);
            }
            set
            {
                if (BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return Activator.CreateInstance<TConfigurationElementType>();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            TConfigurationElementType tConfigurationElementType = (TConfigurationElementType)(object)element;
            return tConfigurationElementType.ElementKey;
        }
        IEnumerator<TConfigurationElementType> IEnumerable<TConfigurationElementType>.GetEnumerator()
		{
            for (int i = 0; i < Count; i++)  
                yield return this[i];  
		}
        public void Add(TConfigurationElementType configurationElement)
        {
            BaseAdd(configurationElement, true);
        }
        public void Clear()
        {
            BaseClear();
        }
        public bool Contains(TConfigurationElementType configurationElement)
        {
            return IndexOf(configurationElement) >= 0;
        }
        public void CopyTo(TConfigurationElementType[] array, int arrayIndex)
        {
            CopyTo((ConfigurationElement[])array, arrayIndex);
        }
        public bool Remove(TConfigurationElementType configurationElement)
        {
            BaseRemove(GetElementKey(configurationElement));
            return true;
        }
        public int IndexOf(TConfigurationElementType configurationElement)
        {
            return BaseIndexOf(configurationElement);
        }
        public void Insert(int index, TConfigurationElementType configurationElement)
        {
            BaseAdd(index, configurationElement);
        }
        public void RemoveAt(int index)
        {
            BaseRemoveAt(index);
        }
    }
}
