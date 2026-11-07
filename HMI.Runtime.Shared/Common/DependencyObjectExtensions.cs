// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections;
using System.Windows;
using System.Windows.Media;
namespace HMI.Runtime.Shared.Common
{
	public static class DependencyObjectExtensions
	{
		public static void StopStoryboards(this DependencyObject element)
		{
			int childrenCount = VisualTreeHelper.GetChildrenCount(element);
			for (int i = 0; i < childrenCount; i++)
			{
				VisualTreeHelper.GetChild(element, i).StopStoryboards();
			}
            StopStoryboardsForElement(element);
		}
		private static void StopStoryboardsForElement(DependencyObject element)
		{
			if (!(element is FrameworkElement))
			{
				return;
			}
			IList visualStateGroups = VisualStateManager.GetVisualStateGroups((FrameworkElement)element);
			for (int i = 0; i < visualStateGroups.Count; i++)
			{
                StopVisualGroupStoryboards(visualStateGroups[i]);
			}
		}
		private static void StopVisualGroupStoryboards(object groupObj)
		{
			VisualStateGroup visualStateGroup = groupObj as VisualStateGroup;
			if (visualStateGroup != null)
			{
				for (int i = 0; i < visualStateGroup.States.Count; i++)
				{
                    StopVisualStateStoryboard(visualStateGroup.States[i]);
				}
			}
		}
		private static void StopVisualStateStoryboard(object stateObj)
		{
			VisualState visualState = stateObj as VisualState;
			if (visualState != null && visualState.Storyboard != null)
			{
				visualState.Storyboard.Stop();
			}
		}
	}
}
