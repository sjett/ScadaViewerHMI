// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Runtime.Shared
{
	internal static class PopupUtilities
	{
		public static DependencyObject CreatePopupContent(Control currentControl, string popupSource, IEnumerable<ParameterItem> popupParameters)
		{
			DependencyObject popupContent = NavigationHelper.CreateUserControl(currentControl, popupSource);
			if (popupContent != null)
			{
				IHMINavigationParameter navigationParameters = NavigationHelper.FindParameterControl(popupContent);
				if (navigationParameters != null)
				{
					foreach (ParameterItem popupParameter in popupParameters)
					{
						bool parameterExists = false;
						foreach (ParameterItem existingParameter in navigationParameters.Parameters)
						{
							if (popupParameter.ParameterName.Equals(existingParameter.ParameterName, StringComparison.InvariantCultureIgnoreCase))
							{
								parameterExists = true;
								existingParameter.ParameterValue = popupParameter.ParameterValue;
								break;
							}
						}
						if (!parameterExists)
						{
							navigationParameters.Parameters.Add(popupParameter);
						}
					}
				}
			}
			return popupContent;
		}
	}
}
