// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.Common.Presentation.Events.Interactions
{
	public static class OpenFileInteractionEventExtensions
	{
		public static void PublishOpenFileInteractionEvent(this IEventAggregator eventAggregator, OpenFileInteractionDetails details)
		{
            eventAggregator.Publish<OpenFileInteractionEvent, OpenFileInteractionDetails>(details);
		}
		public static SubscriptionToken SubscribeOpenFileInteractionEvent(this IEventAggregator eventAggregator, Action<OpenFileInteractionDetails> openFileInteractionCallback)
		{
            return eventAggregator.Subscribe<OpenFileInteractionEvent, OpenFileInteractionDetails>(openFileInteractionCallback, ThreadOption.PublisherThread, false);
		}
		public static SubscriptionToken SubscribeAndHandleOpenFileInteractionEvent(this IEventAggregator eventAggregator)
		{
			return eventAggregator.SubscribeOpenFileInteractionEvent(new Action<OpenFileInteractionDetails>(ShowOpenFileDialog));
		}
        private static void ShowOpenFileDialog(OpenFileInteractionDetails details)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = details.Filter,
                FilterIndex = details.FilterIndex,
                Multiselect = details.Multiselect,
                InitialDirectory = details.InitialDirectory ?? string.Empty,
                Title = details.Title,
                AddExtension = details.AddExtension,
                CheckFileExists = details.CheckFileExists,
                CheckPathExists = details.CheckPathExists,
                DefaultExt = details.DefaultExt ?? string.Empty,
                DereferenceLinks = details.DereferenceLinks,
                ShowReadOnly = details.ShowReadOnly,
                ReadOnlyChecked = details.ReadOnlyChecked
            };
            foreach (Microsoft.Win32.FileDialogCustomPlace customPlace in details.CustomPlaces)
            {
                openFileDialog.CustomPlaces.Add(customPlace);
            }
            Window owner = details.IsActiveWindowChild ? UserInterface.GetActiveWindow() : null;
            bool? dialogResult = openFileDialog.ShowDialog(owner);
            details.Callback(dialogResult.GetValueOrDefault(), openFileDialog.FileNames, details.State);
        }
    }
}
