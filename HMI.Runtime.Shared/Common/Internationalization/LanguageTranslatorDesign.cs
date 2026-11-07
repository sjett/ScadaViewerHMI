// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Internationalization
{
    using System;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Threading;

    public class LanguageTranslatorDesign : IDisposable, ILanguageTranslator
    {
        public event EventHandler LanguageChanged;

        public void Dispose()
        {
        }

        public string GetTranslation(string symbolicText)
        {
            return string.Empty;
        }

        public void RegisterSymbolicTexts(string[] symbolicTexts)
        {
        }

        public string AlwaysVisibleVisibility { get; private set; }

        public string Auto { get; private set; }

        public string Cancel { get; private set; }

        public string CenterSection { get; private set; }

        public CultureInfo CurrentCulture
        {
            get
            {
                return CultureInfo.CurrentCulture;
            }
        }

        public int CurrentLanguageId
        {
            get
            {
                return 9;
            }
        }

        public string CustomFooter { get; private set; }

        public string CustomHeader { get; private set; }

        public string Footer { get; private set; }

        public string Header { get; private set; }

        public string HeaderFooterHeight { get; private set; }

        public string HiddenVisibility { get; private set; }

        public string HorizontalAlignment { get; private set; }

        public string ImageFileToBigMessage { get; private set; }

        public string ImageProperties { get; private set; }

        public string InsertDate { get; private set; }

        public string InsertNumberOfPages { get; private set; }

        public string InsertPageNumber { get; private set; }

        public string InsertPicture { get; private set; }

        public string InsertTime { get; private set; }

        public string LeftSection { get; private set; }

        public string NormalVisibility { get; private set; }

        public string NumberOfSections { get; private set; }

        public string OK { get; private set; }

        public string PageNumberPosition { get; private set; }

        public string Print { get; private set; }

        public string PrintingAdvancedOptions { get; private set; }

        public string PrintoutVisibility { get; private set; }

        public string RemoveAllColorsFromPrintout { get; private set; }

        public string RemovePicture { get; private set; }

        public string RightSection { get; private set; }

        public string ShowFooter { get; private set; }

        public string ShowHeader { get; private set; }

        public string ShowPageNumber { get; private set; }

        public string ShowTableHeaderOnEveryPage { get; private set; }

        public string Stretch { get; private set; }

        public string VerticalAlignment { get; private set; }

        public string Width { get; private set; }
    }
}

