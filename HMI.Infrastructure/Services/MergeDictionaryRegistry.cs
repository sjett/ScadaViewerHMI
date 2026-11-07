// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;

namespace HMI.Infrastructure.Services
{
    public class MergeDictionaryRegistry : IMergeDictionaryRegistry
    {
        public void AddDictionaryResource(Uri packUri)
        {
            Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = packUri
            });
        }
    }
}
