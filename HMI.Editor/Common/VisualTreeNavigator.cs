// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
namespace HMI.Editor.Common
{
    /// <summary>
    /// 提供在视觉树中导航的方法。
    /// </summary>
    public static class VisualTreeNavigator
	{
        /// <summary>
        /// 在视觉树中查找第一个满足条件的子元素。条件是：元素是类型T的实例，并且它的父元素是ItemsPresenter。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="visual"></param>
        /// <returns></returns>
        public static T FindItemsPanel<T>(Visual visual) where T : Visual
		{
			for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
			{
				Visual childVisual = VisualTreeHelper.GetChild(visual, i) as Visual;
				if (childVisual != null)
				{
					if (childVisual is T && VisualTreeHelper.GetParent(childVisual) is ItemsPresenter)
					{
						return (T)(object)childVisual;
					}
					T itemsPanel = FindItemsPanel<T>(childVisual);
					if (itemsPanel != null)
					{
						return itemsPanel;
					}
				}
			}
			return default;
		}
        /// <summary>
        /// 在视觉树中查找第一个满足条件的祖先元素。条件是：元素是类型TAncestor的实例。
        /// </summary>
        /// <typeparam name="TAncestor"></typeparam>
        /// <param name="parentObject"></param>
        /// <returns></returns>
        public static TAncestor FindAncestor<TAncestor>(DependencyObject parentObject) where TAncestor : DependencyObject
		{
			DependencyObject parent = VisualTreeHelper.GetParent(parentObject);
			while (parent != null && !(parent is TAncestor))
			{
				parent = VisualTreeHelper.GetParent(parent);
			}
			return parent as TAncestor;
		}
	}
}
