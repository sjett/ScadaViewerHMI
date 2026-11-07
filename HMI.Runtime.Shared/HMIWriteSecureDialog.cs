// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using HMI.Runtime.Core;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Dialogs;
using HMI.Runtime.Shared.Resources;
namespace HMI.Runtime.Shared
{
	public class HMIWriteSecureDialog : DialogControl
	{
		private const string SymbolicNameDialogCaption = "HMISN_Ctrl_HMIWriteSecure_DialogCaption";
		private const string DefaultTextDialogCaption = "HMI 安全写入";
		private HMIWriteSecureDialogContent dialogContent;
		private IHMIConnector connector;
		private OnLanguageChangedDelegate symbolicTextTranslationsChangedDelegate;
		private Dictionary<string, SymbolicTextTranslation> translations = new Dictionary<string, SymbolicTextTranslation>();
		public string Password
		{
			get
			{
				if (this.dialogContent != null)
				{
					return this.dialogContent.Password;
				}
				return string.Empty;
			}
			set
			{
				if (this.dialogContent != null)
				{
					this.dialogContent.Password = value;
				}
			}
		}
		private bool IsInDesignMode
		{
			get
			{
				return DesignerProperties.GetIsInDesignMode(this);
			}
		}
		public HMIWriteSecureDialog()
		{
			base.HeaderText = "HMISN_Ctrl_HMIWriteSecureDialog_Title";
			this.dialogContent = new HMIWriteSecureDialogContent();
			base.Content=(this.dialogContent);
			this.dialogContent.Closed += new EventHandler<DialogClosedEventArgs>(this.dialogContent_Closed);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			if (!this.IsInDesignMode && this.connector == null)
			{
				this.InitializeCommunication();
				this.InitializeTranslations();
				this.RegisterLanguageChangedHandler();
			}
		}
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            this.Password = string.Empty;
        }

		//protected override void OnOpened(RoutedEventArgs args)
		//{
		//	base.OnOpened(args);
		//	this.Password = string.Empty;
		//}

		private void InitializeCommunication()
		{
			this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
			this.symbolicTextTranslationsChangedDelegate = new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler);
		}
		private void RegisterLanguageChangedHandler()
		{
		}
		private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicNames, string[] translationTexts)
		{
			for (int i = 0; i < symbolicNames.Length; i++)
			{
				SymbolicTextTranslation symbolicTextTranslation = this.translations[symbolicNames[i]];
				symbolicTextTranslation.Translation = translationTexts[i];
			}
			this.UpdateControlTexts();
		}
		private void SetHeaderText(SymbolicTextTranslation stt, string defaultText)
		{
			base.HeaderText = (HMIWriteSecureDialog.IsValidText(stt.Translation) ? stt.Translation : defaultText);
		}
		private static bool IsValidText(string text)
		{
            return text != null && !text.Equals("!null!", StringComparison.InvariantCultureIgnoreCase);
		}
		private void InitializeTranslations()
		{
			this.AddTranslation("HMISN_Ctrl_HMIWriteSecure_DialogCaption");
		}
		private void AddTranslation(string symbolicName)
		{
			SymbolicTextTranslation symbolicTextTranslation = new SymbolicTextTranslation(symbolicName);
			this.translations.Add(symbolicTextTranslation.SymbolicText, symbolicTextTranslation);
		}
		private void UpdateControlTexts()
		{
			this.SetHeaderText(this.translations["HMISN_Ctrl_HMIWriteSecure_DialogCaption"], "HMI Secure Write");
		}
		private void dialogContent_Closed(object sender, DialogClosedEventArgs e)
		{
			base.Close(e.DialogResult);
		}
		protected override void OnDispose()
		{
			base.OnDispose();
			this.Dispose(true);
		}
		private void Dispose(bool dispose)
		{
			if (dispose)
			{
				if (this.dialogContent != null)
				{
					this.dialogContent.Closed -= new EventHandler<DialogClosedEventArgs>(this.dialogContent_Closed);
					this.dialogContent.Dispose();
				}
				this.DisposeHMIConnector();
				this.StopStoryboards();
			}
		}
		private void DisposeHMIConnector()
		{
			if (this.connector != null)
			{
				this.UnregisterLanguageChangedHandler();
				this.connector.Dispose();
				this.connector = null;
			}
		}
		private void UnregisterLanguageChangedHandler()
		{
		}
	}
}
