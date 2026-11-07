// SPDX-License-Identifier: GPL-3.0-only
using Microsoft.Win32;
using System.IO;
using System.Windows;

namespace HMI.Common.Presentation.Events.Interactions
{
    public static class BrowseFolderInteractionEventExtensions
    {
        public static void PublishBrowseFolderInteractionEvent(this IEventAggregator eventAggregator, BrowseFolderInteractionDetails details)
        {
            eventAggregator.Publish<BrowseFolderInteractionEvent, BrowseFolderInteractionDetails>(details);
        }
        public static SubscriptionToken SubscribeBrowseFolderInteractionEvent(this IEventAggregator eventAggregator, Action<BrowseFolderInteractionDetails> openFileInteractionCallback)
        {
            return eventAggregator.Subscribe<BrowseFolderInteractionEvent, BrowseFolderInteractionDetails>(openFileInteractionCallback, ThreadOption.PublisherThread, false);
        }
        public static SubscriptionToken SubscribeAndHandleBrowseFolderInteractionEvent(this IEventAggregator eventAggregator)
        {
            return eventAggregator.SubscribeBrowseFolderInteractionEvent(new Action<BrowseFolderInteractionDetails>(HandleBrowseFolderInteraction));
        }

        private static void HandleBrowseFolderInteraction(BrowseFolderInteractionDetails interactionDetails)
        {
            Microsoft.Win32.OpenFolderDialog openFolderDialog = new Microsoft.Win32.OpenFolderDialog
            {
                Title = interactionDetails.Description,
                RootDirectory = Environment.GetFolderPath(interactionDetails.RootFolder),
                FolderName = interactionDetails.SelectedPath,
                Multiselect = false,
                //DontIncludeNetworkFoldersBelowDomainLevel = !browseFolderInfo.IncludeNetworkFoldersBelowDomainLevel,
                //ShowBothFilesAndFolders = browseFolderInfo.ShowBothFilesAndFolders,
                //ShowEditBox = browseFolderInfo.ShowEditBox,
                //ShowFullPathInEditBox = browseFolderInfo.ShowFullPathInEditBox,
                //ShowNewFolderButton = browseFolderInfo.ShowNewFolderButton
            };

            //FolderBrowserDialogEx folderBrowserDialogEx = new FolderBrowserDialogEx
            //{
            //    Description = browseFolderInfo.Description,
            //    NewStyle = true,
            //    RootFolder = browseFolderInfo.RootFolder,
            //    SelectedPath = browseFolderInfo.SelectedPath,
            //    DontIncludeNetworkFoldersBelowDomainLevel = !browseFolderInfo.IncludeNetworkFoldersBelowDomainLevel,
            //    ShowBothFilesAndFolders = browseFolderInfo.ShowBothFilesAndFolders,
            //    ShowEditBox = browseFolderInfo.ShowEditBox,
            //    ShowFullPathInEditBox = browseFolderInfo.ShowFullPathInEditBox,
            //    ShowNewFolderButton = browseFolderInfo.ShowNewFolderButton
            //};
            Window? owner = interactionDetails.IsActiveWindowChild ? UserInterface.GetActiveWindow() : null;
            bool? flag = openFolderDialog.ShowDialog(owner);
            interactionDetails.Callback(flag.GetValueOrDefault(), openFolderDialog.FolderName, interactionDetails.State);
        }
    }
}
