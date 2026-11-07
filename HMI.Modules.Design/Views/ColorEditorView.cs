// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Presentation.Views;
using HMI.Infrastructure.Services;
using HMI.Modules.Design.Resources;
using Newtonsoft.Json.Linq;
using NLog;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Xceed.Wpf.Toolkit;

namespace HMI.Modules.Design.Views
{
	[ExcludeFromCodeCoverage, TemplatePart(Name = "uxEditor", Type = typeof(CheckBox))]
	public class ColorEditorView : EditorView
	{
		private const string RecentlyUsedColorsKey = "RecentlyUsedColors";
		private const string ColorValueKey = "value";
		private const string ColorNameKey = "name";
		private const string ColorPickerTemplatePartName = "uxEditor";

		internal static readonly Logger Logger;
		internal static readonly ObservableCollection<ColorItem> RecentlyUsedColors;
		private ColorPicker colorPicker;
		

		static ColorEditorView()
		{
			ColorEditorView.Logger = LogManager.GetLogger("ColorEditorView");

			FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(ColorEditorView), new FrameworkPropertyMetadata(typeof(ColorEditorView)));
			
			ColorEditorView.RecentlyUsedColors = new ObservableCollection<ColorItem>(LoadRecentlyUsedColors());

			Observable
				.FromEventPattern<NotifyCollectionChangedEventHandler, NotifyCollectionChangedEventArgs>(
					addHandler => ColorEditorView.RecentlyUsedColors.CollectionChanged += addHandler,
					removeHandler => ColorEditorView.RecentlyUsedColors.CollectionChanged -= removeHandler)
				.Throttle(TimeSpan.FromSeconds(1.0))
				.Subscribe(e => SavePreferences());
		}
        static void SavePreferences()
        {
            try
            {
                var preferences = CreatePreferences(ColorEditorView.RecentlyUsedColors);
                Preferences.SetPreferences(typeof(ColorEditorView), preferences);
            }
            catch (Exception exception)
            {
                ColorEditorView.Logger.WarnException(ErrorTexts.CouldNotSaveRecentlyUsedColors, exception);
            }
        }
        static JObject CreatePreferences(IEnumerable<ColorItem> colorItems)
        {
            return new JObject
            {
                new JProperty(RecentlyUsedColorsKey, [.. colorItems.Select(SerializeColorItem)])
            };
        }
        static IEnumerable<ColorItem> LoadRecentlyUsedColors()
        {
            try
            {
                ColorEditorView.Logger.Trace("Loading recently used colors");

                var preferences = Preferences.GetPreferences(typeof(ColorEditorView));
                var colorItems = preferences?[RecentlyUsedColorsKey]?.ToArray() ?? [];
                IEnumerable<ColorItem> loadedItems = colorItems.Select(DeserializeColorItem);

                return loadedItems.Where(item => item != null);
            }
            catch (Exception exception)
            {
                ColorEditorView.Logger.WarnException(ErrorTexts.CouldNotLoadRecentlyUsedColors, exception);
                return Enumerable.Empty<ColorItem>();
            }
        }
        static ColorItem DeserializeColorItem(JToken colorItemJson)
        {
            try
            {
                var colorValue = ((string?)colorItemJson[ColorValueKey]);
                object convertedColor = ColorConverter.ConvertFromString(colorValue);
                if (convertedColor == null)
                {
                    return null;
                }

                Color color = (Color)convertedColor;
                var colorName = (string?)colorItemJson[ColorNameKey];
                return new ColorItem(color, colorName);
            }
            catch
            {
                return null;
            }
        }
        public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			this.colorPicker = this.FindTemplateChild<ColorPicker>(ColorPickerTemplatePartName);
			this.colorPicker.SetBinding(ColorPicker.SelectedColorProperty, base.ValueBinding);
			this.colorPicker.RecentColors = ColorEditorView.RecentlyUsedColors;
		}
		
		
		static JObject SerializeColorItem(ColorItem colorItem)
		{
			return new JObject
			{
				new JProperty(ColorValueKey, colorItem.Color.ToString()),
				new JProperty(ColorNameKey, colorItem.Name)
			};
		}
		
	}
}
