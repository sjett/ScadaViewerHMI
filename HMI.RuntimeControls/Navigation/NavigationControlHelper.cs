// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using HMI.Runtime.Shared;

namespace HMI.RuntimeControls.Navigation
{
	internal static class NavigationControlHelper
	{
		public static void CreateNavigationControl(Control currentControl, IEnumerable<NavigationItem> navigationItems)
		{
			foreach (NavigationItem navigationItem in navigationItems)
			{
				if (navigationItem.Source == string.Empty)
				{
					MessageBox.Show("Source is not defined.");
				}
				else
				{
					UserControl navControl = NavigationHelper.CreateUserControl(currentControl, navigationItem.Source);
					if (navControl == null)
					{
						MessageBox.Show("The defined source is not found.");
					}
					else
					{
						if (navigationItem.Target == string.Empty)
						{
							MessageBox.Show("Target is not defined.");
						}
						else
						{
							ContentControl navTaget = NavigationHelper.FindControl(Application.Current.MainWindow, navigationItem.Target);
							if (navTaget == null)
							{
								MessageBox.Show("The defined target is not found.");
							}
							else
							{
								if (navigationItem.Parameters.Count > 0)
								{
									IHMINavigationParameter? navigationParameter = NavigationHelper.FindParameterControl(navControl);
									if (navigationParameter != null)
									{
										navigationParameter.Parameters.Clear();

										foreach (ParameterItem paramItem in navigationItem.Parameters)
										{
											ParameterItem parameterItem = NavigationControlHelper.CloneParameterItem(paramItem);
											navigationParameter.Parameters.Add(parameterItem);
										}

										NavigationHelper.DisposeControls(navTaget);
										GC.Collect();
										navTaget.Content = (navControl);
									}
									else
									{
										MessageBox.Show("The parameter control is missing from the defined target.");
									}
								}
								else
								{
									NavigationHelper.DisposeControls(navTaget);
									GC.Collect();
									navTaget.Content = (navControl);
								}
							}
						}
					}
				}
			}
		}
		private static ParameterItem CloneParameterItem(ParameterItem parameterItem)
		{
			return new ParameterItem
			{
				ParameterName = parameterItem.ParameterName,
				ParameterValue = parameterItem.ParameterValue
			};
		}
	}
}
