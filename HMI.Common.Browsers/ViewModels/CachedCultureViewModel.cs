// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Globalization;
using System.Runtime.Caching;
namespace HMI.Common.Browsers.ViewModels
{
	public class CachedCultureViewModel : CultureViewModel
	{
		public override bool IsVisible
		{
			get
			{
				return base.IsVisible;
			}
			set
			{
				base.IsVisible = value;
				MemoryCache.Default[GetVisibilityCacheKey()] = value;
			}
		}
		public CachedCultureViewModel(int languageId) : base(languageId)
		{
			string key = GetVisibilityCacheKey();
            isVisible = (bool)(MemoryCache.Default[key] ?? true);
		}
		private string GetVisibilityCacheKey()
		{
			return string.Format(CultureInfo.InvariantCulture, "CultureViewModel:{0}", new object[]
			{
                ID
            });
		}
	}
}
