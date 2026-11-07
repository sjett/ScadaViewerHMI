// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Windows;
using System.Windows.Controls;
namespace HMI.Editor.Toolbox
{
	public class Toolbox : ItemsControl
	{
		public static readonly DependencyProperty StyleSelectorProperty = DependencyProperty.Register("StyleSelector", typeof(int), typeof(Toolbox), new FrameworkPropertyMetadata(1, new PropertyChangedCallback(OnStyleSelectorChanged)));
		private readonly ItemsPanelTemplate originalPanelTemplate;
		public int StyleSelector
		{
			get
			{
				return (int)GetValue(StyleSelectorProperty);
			}
			set
			{
                SetValue(StyleSelectorProperty, value);
			}
		}
		private static void OnStyleSelectorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			Toolbox toolbox = d as Toolbox;
			switch ((toolbox.StyleSelector - 1) % 4 + 1)
			{
			case 1:
				toolbox.ItemContainerStyle = toolbox.FindResource("ToolboxItemStyle1") as Style;
				toolbox.ItemsPanel = toolbox.originalPanelTemplate;
				return;
			case 2:
				toolbox.ItemContainerStyle = toolbox.FindResource("ToolboxItemStyle2") as Style;
				toolbox.ItemsPanel = toolbox.originalPanelTemplate;
				return;
			case 3:
				toolbox.ItemContainerStyle = toolbox.FindResource("ToolboxItemStyle3") as Style;
				toolbox.ItemsPanel = toolbox.originalPanelTemplate;
				return;
			case 4:
				toolbox.ItemContainerStyle = toolbox.FindResource("ToolboxItemStyle4") as Style;
				toolbox.ItemsPanel = toolbox.FindResource("WrapItemsPanel") as ItemsPanelTemplate;
				return;
			default:
				return;
			}
		}
		public Toolbox()
		{
            ItemContainerStyle = FindResource("ToolboxItemStyle1") as Style;
            originalPanelTemplate = ItemsPanel;
		}
		protected override DependencyObject GetContainerForItemOverride()
		{
			return new ToolboxItem();
		}
		protected override bool IsItemItsOwnContainerOverride(object item)
		{
			return item is ToolboxItem;
		}
	}
}
