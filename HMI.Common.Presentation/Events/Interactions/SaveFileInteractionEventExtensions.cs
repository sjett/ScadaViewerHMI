// SPDX-License-Identifier: GPL-3.0-only
using System.Windows;

namespace HMI.Common.Presentation.Events.Interactions
{
	public static class SaveFileInteractionEventExtensions
	{
		public static void PublishSaveFileInteractionEvent(this IEventAggregator eventAggregator, SaveFileInteractionDetails details)
		{
            eventAggregator.Publish<SaveFileInteractionEvent, SaveFileInteractionDetails>(details);
		}
		public static SubscriptionToken SubscribeSaveFileInteractionEvent(this IEventAggregator eventAggregator, Action<SaveFileInteractionDetails> saveFileInteractionCallback)
		{
            return eventAggregator.Subscribe<SaveFileInteractionEvent, SaveFileInteractionDetails>(saveFileInteractionCallback, ThreadOption.PublisherThread, false);
		}
		public static SubscriptionToken SubscribeAndHandleSaveFileInteractionEvent(this IEventAggregator eventAggregator)
		{
			return eventAggregator.SubscribeSaveFileInteractionEvent(new Action<SaveFileInteractionDetails>(ShowSaveFileDialog));
		}
        private static void ShowSaveFileDialog(SaveFileInteractionDetails details)
        {
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog
            {
                Filter = details.Filter,
                FilterIndex = details.FilterIndex,
                InitialDirectory = details.InitialDirectory ?? string.Empty,
                Title = details.Title,
                AddExtension = details.AddExtension,
                CheckFileExists = details.CheckFileExists,
                CheckPathExists = details.CheckPathExists,
                DefaultExt = details.DefaultExt ?? string.Empty,
                DereferenceLinks = details.DereferenceLinks,
                CreatePrompt = details.CreatePrompt,
                FileName = details.FileName ?? string.Empty,
                OverwritePrompt = details.OverwritePrompt,
                ValidateNames = details.ValidateNames
            };
            foreach (Microsoft.Win32.FileDialogCustomPlace customPlace in details.CustomPlaces)
            {
                saveFileDialog.CustomPlaces.Add(customPlace);
            }
            Window owner = details.IsActiveWindowChild ? UserInterface.GetActiveWindow() : null;
            bool? dialogResult = saveFileDialog.ShowDialog(owner);
            details.Callback(dialogResult.GetValueOrDefault(), saveFileDialog.FileName, details.State);
        }
    }
}
