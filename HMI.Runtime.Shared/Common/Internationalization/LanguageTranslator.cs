// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Internationalization
{
    using HMI.Runtime.Core;
    using HMI.Runtime.Core.Helper;
    using HMI.Runtime.Shared.Common;
    using System;
    using System.Collections.Generic;
    using System.Globalization;

    public class LanguageTranslator : PropertyChangedNotifier, IDisposable, ILanguageTranslator
    {
        private OnLanguageChangedDelegate onLanguageChangedDelegate;
        private IHMITranslationProvider translationProvider;

        private Dictionary<string, string> translations;

        public event EventHandler? LanguageChanged;

        public LanguageTranslator(IHMITranslationProvider translationProvider)
        {
            Contract.CheckIsNull(translationProvider, "translationProvider");
            this.translationProvider = translationProvider;
            this.translations = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            this.onLanguageChangedDelegate = new OnLanguageChangedDelegate(this.translationProvider_LanguageChanged);
            string[] staticSymbolicTexts = this.GetStaticSymbolicTexts();
            this.RegisterSymbolicTexts(staticSymbolicTexts);
        }

        private void AddToTranslationsDictionary(string[] symbolicTexts)
        {
            foreach (string str in symbolicTexts)
            {
                this.translations.Add(str, string.Empty);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeExplicit();
            }
        }

        protected virtual void DisposeExplicit()
        {
            if (this.onLanguageChangedDelegate != null)
            {
                this.translationProvider.UnregisterLanguageChangedHandler(null, this.onLanguageChangedDelegate);
            }
        }

        ~LanguageTranslator()
        {
            this.Dispose(false);
        }

        protected virtual void FirePropertyChanged()
        {
            base.RaisePropertyChanged<string>(() => this.ShowTableHeaderOnEveryPage);
            base.RaisePropertyChanged<string>(() => this.ShowPageNumber);
            base.RaisePropertyChanged<string>(() => this.PageNumberPosition);
            base.RaisePropertyChanged<string>(() => this.Header);
            base.RaisePropertyChanged<string>(() => this.Footer);
            base.RaisePropertyChanged<string>(() => this.CustomHeader);
            base.RaisePropertyChanged<string>(() => this.CustomFooter);
            base.RaisePropertyChanged<string>(() => this.NumberOfSections);
            base.RaisePropertyChanged<string>(() => this.LeftSection);
            base.RaisePropertyChanged<string>(() => this.CenterSection);
            base.RaisePropertyChanged<string>(() => this.RightSection);
            base.RaisePropertyChanged<string>(() => this.InsertPageNumber);
            base.RaisePropertyChanged<string>(() => this.InsertNumberOfPages);
            base.RaisePropertyChanged<string>(() => this.InsertDate);
            base.RaisePropertyChanged<string>(() => this.InsertTime);
            base.RaisePropertyChanged<string>(() => this.InsertPicture);
            base.RaisePropertyChanged<string>(() => this.RemovePicture);
            base.RaisePropertyChanged<string>(() => this.HeaderFooterHeight);
            base.RaisePropertyChanged<string>(() => this.ShowHeader);
            base.RaisePropertyChanged<string>(() => this.ShowFooter);
            base.RaisePropertyChanged<string>(() => this.PrintingAdvancedOptions);
            base.RaisePropertyChanged<string>(() => this.RemoveAllColorsFromPrintout);
            base.RaisePropertyChanged<string>(() => this.PrintoutVisibility);
            base.RaisePropertyChanged<string>(() => this.NormalVisibility);
            base.RaisePropertyChanged<string>(() => this.AlwaysVisibleVisibility);
            base.RaisePropertyChanged<string>(() => this.HiddenVisibility);
            base.RaisePropertyChanged<string>(() => this.Width);
            base.RaisePropertyChanged<string>(() => this.Auto);
            base.RaisePropertyChanged<string>(() => this.Stretch);
            base.RaisePropertyChanged<string>(() => this.HorizontalAlignment);
            base.RaisePropertyChanged<string>(() => this.VerticalAlignment);
            base.RaisePropertyChanged<string>(() => this.ImageProperties);
            base.RaisePropertyChanged<string>(() => this.Print);
            base.RaisePropertyChanged<string>(() => this.ImageFileToBigMessage);
            base.RaisePropertyChanged<string>(() => this.OK);
            base.RaisePropertyChanged<string>(() => this.Cancel);
        }

        private string[] GetDistinctNotInDictionarySymbolicTexts(string[] symbolicTexts)
        {
            List<string> list = new List<string>();
            foreach (string str in symbolicTexts)
            {
                if (!this.translations.ContainsKey(str) && !list.Contains(str))
                {
                    list.Add(str);
                }
            }
            return list.ToArray();
        }

        protected virtual string[] GetStaticSymbolicTexts()
        {
            return new List<string> { 
                "HMISN_DataTable_PrintDialog.ShowTableHeaderOnEveryPage", 
                "HMISN_DataTable_PrintDialog.ShowPageNumber", 
                "HMISN_DataTable_PrintDialog.PageNumberPosition", 
                "HMISN_DataTable_PrintDialog.Header", 
                "HMISN_DataTable_PrintDialog.Footer", 
                "HMISN_DataTable_PrintDialog.CustomHeader", 
                "HMISN_DataTable_PrintDialog.CustomFooter",
                "HMISN_DataTable_PrintDialog.NumberOfSections", 
                "HMISN_DataTable_PrintDialog.LeftSection", 
                "HMISN_DataTable_PrintDialog.CenterSection", 
                "HMISN_DataTable_PrintDialog.RightSection",
                "HMISN_DataTable_PrintDialog.InsertPageNumber", 
                "HMISN_DataTable_PrintDialog.InsertNumberOfPages", 
                "HMISN_DataTable_PrintDialog.InsertDate", 
                "HMISN_DataTable_PrintDialog.InsertTime", 
                "HMISN_DataTable_PrintDialog.InsertPicture", 
                "HMISN_DataTable_PrintDialog.HeaderFooterHeight", 
                "HMISN_DataTable_PrintDialog.ShowHeader", 
                "HMISN_DataTable_PrintDialog.ShowFooter", 
                "HMISN_DataTable_PrintDialog.PrintingAdvancedOptions", 
                "HMISN_DataTable_PrintDialog.RemoveAllColorsFromPrintout", 
                "HMISN_DataTable_PrintDialog.PrintoutVisibility", 
                "HMISN_DataTable_PrintDialog.NormalVisibility", 
                "HMISN_DataTable_PrintDialog.AlwaysVisibleVisibility", 
                "HMISN_DataTable_PrintDialog.HiddenVisibility",

                "HMISN_DataTable_HMIDataTable.print", 
                "HMISN_DataTable_Messages.ImageFileToBigMessage", 

                "HMISN_Shared_HeaderFooter.RemovePicture",
                "HMISN_Shared_HeaderFooter.Width", 
                "HMISN_Shared_HeaderFooter.Auto", 
                "HMISN_Shared_HeaderFooter.Stretch", 
                "HMISN_Shared_HeaderFooter.HorizontalAlignment", 
                "HMISN_Shared_HeaderFooter.VerticalAlignment", 
                "HMISN_Shared_HeaderFooter.ImageProperties", 
                
                "HMISN_Shared_Configuration.OK", 
                "HMISN_Shared_Configuration.Cancel"
             }.ToArray();
        }

        public string GetTranslation(string symbolicText)
        {
            return this.translations[symbolicText];
        }
        public void RegisterSymbolicTexts(string[] symbolicTexts)
        {
            string[] distinctNotInDictionarySymbolicTexts = this.GetDistinctNotInDictionarySymbolicTexts(symbolicTexts);
            if (distinctNotInDictionarySymbolicTexts.Length != 0)
            {
                this.AddToTranslationsDictionary(distinctNotInDictionarySymbolicTexts);
                this.translationProvider.RegisterLanguageChangedHandler(distinctNotInDictionarySymbolicTexts, this.onLanguageChangedDelegate);
            }
        }

        private void translationProvider_LanguageChanged(string[] symbolicTexts, string[] translationTexts)
        {
            this.UpdateTranslations(symbolicTexts, translationTexts);
            this.FirePropertyChanged();
            this.OnLanguageChanged();
        }
        private void UpdateTranslations(string[] symbolicTexts, string[] newTranslations)
        {
            for (int i = 0; i < symbolicTexts.Length; i++)
            {
                this.UpdateTranslation(symbolicTexts[i], newTranslations[i]);
            }
        }
        private void UpdateTranslation(string symbolicText, string newTranslation)
        {
            if (this.translations.ContainsKey(symbolicText))
            {
                this.translations[symbolicText] = ((newTranslation == null) || !(newTranslation != "!null!")) ? symbolicText : newTranslation;
            }
        }
        protected virtual void OnLanguageChanged()
        {
            this.LanguageChanged?.Invoke(this, EventArgs.Empty);
        }

        public string AlwaysVisibleVisibility
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.AlwaysVisibleVisibility"];
            }
        }

        public string Auto
        {
            get
            {
                return this.translations["HMISN_Shared_HeaderFooter.Auto"];
            }
        }

        public string Cancel
        {
            get
            {
                return this.translations["HMISN_Shared_Configuration.Cancel"];
            }
        }

        public string CenterSection
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.CenterSection"];
            }
        }

        public CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfoCache.GetCultureInfo(this.CurrentLanguageId);
            }
        }

        public int CurrentLanguageId
        {
            get
            {
                return this.translationProvider.GetLanguageId();
            }
        }

        public string CustomFooter
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.CustomFooter"];
            }
        }

        public string CustomHeader
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.CustomHeader"];
            }
        }

        public string Footer
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.Footer"];
            }
        }

        public string Header
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.Header"];
            }
        }

        public string HeaderFooterHeight
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.HeaderFooterHeight"];
            }
        }

        public string HiddenVisibility
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.HiddenVisibility"];
            }
        }

        public string HorizontalAlignment
        {
            get
            {
                return this.translations["HMISN_Shared_HeaderFooter.HorizontalAlignment"];
            }
        }

        public string ImageFileToBigMessage
        {
            get
            {
                return this.translations["HMISN_DataTable_Messages.ImageFileToBigMessage"];
            }
        }

        public string ImageProperties
        {
            get
            {
                return this.translations["HMISN_Shared_HeaderFooter.ImageProperties"];
            }
        }

        public string InsertDate
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.InsertDate"];
            }
        }

        public string InsertNumberOfPages
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.InsertNumberOfPages"];
            }
        }

        public string InsertPageNumber
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.InsertPageNumber"];
            }
        }

        public string InsertPicture
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.InsertPicture"];
            }
        }

        public string InsertTime
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.InsertTime"];
            }
        }

        public string LeftSection
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.LeftSection"];
            }
        }

        public string NormalVisibility
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.NormalVisibility"];
            }
        }

        public string NumberOfSections
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.NumberOfSections"];
            }
        }

        public string OK
        {
            get
            {
                return this.translations["HMISN_Shared_Configuration.OK"];
            }
        }

        public string PageNumberPosition
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.PageNumberPosition"];
            }
        }

        public string Print
        {
            get
            {
                return this.translations["HMISN_DataTable_HMIDataTable.print"];
            }
        }

        public string PrintingAdvancedOptions
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.PrintingAdvancedOptions"];
            }
        }

        public string PrintoutVisibility
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.PrintoutVisibility"];
            }
        }

        public string RemoveAllColorsFromPrintout
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.RemoveAllColorsFromPrintout"];
            }
        }

        public string RemovePicture
        {
            get
            {
                return this.translations["HMISN_Shared_HeaderFooter.RemovePicture"];
            }
        }

        public string RightSection
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.RightSection"];
            }
        }

        public string ShowFooter
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.ShowFooter"];
            }
        }

        public string ShowHeader
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.ShowHeader"];
            }
        }

        public string ShowPageNumber
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.ShowPageNumber"];
            }
        }

        public string ShowTableHeaderOnEveryPage
        {
            get
            {
                return this.translations["HMISN_DataTable_PrintDialog.ShowTableHeaderOnEveryPage"];
            }
        }

        public string Stretch
        {
            get
            {
                return this.translations["HMISN_Shared_HeaderFooter.Stretch"];
            }
        }

        public string VerticalAlignment
        {
            get
            {
                return this.translations["HMISN_Shared_HeaderFooter.VerticalAlignment"];
            }
        }

        public string Width
        {
            get
            {
                return this.translations["HMISN_Shared_HeaderFooter.Width"];
            }
        }
    }
}

