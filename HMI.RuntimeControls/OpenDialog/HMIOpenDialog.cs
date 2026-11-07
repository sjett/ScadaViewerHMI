// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HMI.Runtime.Core;
using HMI.Runtime.Shared;
using HMI.Runtime.Shared.Common;
using HMI.Runtime.Shared.Dialogs;

namespace HMI.RuntimeControls.OpenDialog
{
	public class HMIOpenDialog : Button, IDisposable
	{
		private IHMIConnector connector;
		private readonly DialogManager dialogManager;
		private readonly ObservableCollection<ParameterItem> popupParameters;

		public static readonly DependencyProperty PopupModalityProperty = DependencyProperty.Register("PopupModality", typeof(DialogModality), typeof(HMIOpenDialog), new PropertyMetadata(DialogModality.Modeless, new PropertyChangedCallback(HMIOpenDialog.OnPopupModalityPropertyChanged)));
		public static readonly DependencyProperty PopupSourceProperty = DependencyProperty.Register("PopupSource", typeof(string), typeof(HMIOpenDialog), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIOpenDialog.PopupSourcePropertyChanged)));
		public static readonly DependencyProperty PopupTitleProperty = DependencyProperty.Register("PopupTitle", typeof(string), typeof(HMIOpenDialog), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIOpenDialog.PopupTitlePropertyChanged)));
		public static readonly DependencyProperty SymbolicTextProperty = DependencyProperty.Register("SymbolicText", typeof(string), typeof(HMIOpenDialog), new PropertyMetadata(string.Empty, new PropertyChangedCallback(HMIOpenDialog.OnSymbolicTextPropertyChanged)));
		
		[Category("HMI")]
		public DialogModality PopupModality
		{
			get
			{
				return (DialogModality)base.GetValue(HMIOpenDialog.PopupModalityProperty);
			}
			set
			{
				base.SetValue(HMIOpenDialog.PopupModalityProperty, value);
			}
		}
		[Category("HMI")]
		public ObservableCollection<ParameterItem> PopupParameters
		{
			get
			{
				return this.popupParameters;
			}
		}
		[Category("HMI")]
		public string PopupSource
		{
			get
			{
				return (string)base.GetValue(HMIOpenDialog.PopupSourceProperty);
			}
			set
			{
				base.SetValue(HMIOpenDialog.PopupSourceProperty, value);
			}
		}
		[Category("HMI")]
		public string PopupTitle
		{
			get
			{
				return (string)base.GetValue(HMIOpenDialog.PopupTitleProperty);
			}
			set
			{
				base.SetValue(HMIOpenDialog.PopupTitleProperty, value);
			}
		}
		[Category("HMI")]
		public string SymbolicText
		{
			get
			{
				return (string)base.GetValue(HMIOpenDialog.SymbolicTextProperty);
			}
			set
			{
				base.SetValue(HMIOpenDialog.SymbolicTextProperty, value);
			}
		}
		private static void OnPopupModalityPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void PopupSourcePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void PopupTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
		}
		private static void OnSymbolicTextPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			HMIOpenDialog? wFOpenDialog = d as HMIOpenDialog;
			if (wFOpenDialog == null)
			{
				return;
			}
			wFOpenDialog.Content=((string)e.NewValue);
		}
		public HMIOpenDialog()
		{
			this.popupParameters = new ObservableCollection<ParameterItem>();
			this.dialogManager = new DialogManager(null, null, null);
		}
		~HMIOpenDialog()
		{
			this.Dispose(false);
		}
		private void OnSymbolicTextTranslationsChangedHandler(string[] symbolicNames, string[] translationTexts)
		{
			for (int i = 0; i < symbolicNames.Length; i++)
			{
				if (!(symbolicNames[i] != this.SymbolicText))
				{
					base.Content=(translationTexts[i]);
					return;
				}
			}
		}
		private void HMIControl_MouseEnter(object sender, MouseEventArgs e)
		{
			if (string.IsNullOrEmpty(this.PopupSource))
			{
				return;
			}
			base.Cursor=(Cursors.Hand);
		}
		private void HMIControl_MouseLeave(object sender, MouseEventArgs e)
		{
			base.Cursor=(Cursors.Arrow);
		}
		public override void OnApplyTemplate()
		{
			base.OnApplyTemplate();
			bool isInDesignMode = DesignerProperties.GetIsInDesignMode(this);
			this.ResolvePopupPlaceholders();
			if (!isInDesignMode && this.connector == null)
			{
				this.connector = RuntimeServices.GetRequiredService<IHMIConnector>();
                this.RegisterSymbolicTexts();
				this.RegisterMouseHandlers();
				return;
			}
		}
		protected override void OnClick()
		{
			base.OnClick();
			this.TryShowPopupDialog();
		}
		private void ResolvePopupPlaceholders()
		{
			this.ResolvePopupParametersPlaceholders();
			this.ResolvePopupSourcePlaceholders();
			this.ResolvePopupTitlePlaceholders();
		}
		private void ResolvePopupParametersPlaceholders()
		{
			foreach (ParameterItem paramItem in this.popupParameters)
			{
				paramItem.ParameterName = StringPlaceholdersResolver.Resolve(paramItem.ParameterName, this);
				paramItem.ParameterValue = StringPlaceholdersResolver.Resolve(paramItem.ParameterValue, this);
			}
		}
		private void ResolvePopupSourcePlaceholders()
		{
			this.PopupSource = StringPlaceholdersResolver.Resolve(this.PopupSource, this);
		}
		private void ResolvePopupTitlePlaceholders()
		{
			this.PopupTitle = StringPlaceholdersResolver.Resolve(this.PopupTitle, this);
		}
		private void RegisterSymbolicTexts()
		{
			this.connector.RegisterLanguageChangedHandler(new string[]
			{
				this.SymbolicText
			}, new OnLanguageChangedDelegate(this.OnSymbolicTextTranslationsChangedHandler));
		}
		private void RegisterMouseHandlers()
		{
			base.MouseEnter+=(new MouseEventHandler(this.HMIControl_MouseEnter));
			base.MouseLeave+=(new MouseEventHandler(this.HMIControl_MouseLeave));
		}
		private bool TryShowPopupDialog()
		{
			if (string.IsNullOrEmpty(this.PopupSource))
			{
				return true;
			}
			try
			{
				this.dialogManager.Show(this, this.PopupSource, this.PopupParameters, this.PopupTitle, this.PopupModality);
			}
			catch (Exception ex)
			{
				this.connector.OutputDebugText(HMIDebugLevel.Error, string.Format("{0} {1}", base.GetType().Name, ex.Message), new object[0]);
			}
			return false;
		}
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			if (!disposing)
			{
				return;
			}
			this.dialogManager.Dispose();
			this.DisposeConnector();
			this.DisposeMouseHandlers();
			this.StopStoryboards();
		}
		private void DisposeConnector()
		{
			if (this.connector != null)
			{
				this.connector.Dispose();
			}
		}
		private void DisposeMouseHandlers()
		{
			base.MouseEnter-=(new MouseEventHandler(this.HMIControl_MouseEnter));
			base.MouseLeave-=(new MouseEventHandler(this.HMIControl_MouseLeave));
		}
	}
}
