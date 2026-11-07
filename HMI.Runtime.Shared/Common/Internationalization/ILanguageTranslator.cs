// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Runtime.Shared.Common.Internationalization
{
    using System;
    using System.Globalization;

    public interface ILanguageTranslator : IDisposable
    {
        event EventHandler LanguageChanged;

        string GetTranslation(string symbolicText);
        void RegisterSymbolicTexts(string[] symbolicTexts);

        string AlwaysVisibleVisibility { get; }

        string Auto { get; }

        string Cancel { get; }

        string CenterSection { get; }

        CultureInfo CurrentCulture { get; }

        int CurrentLanguageId { get; }

        string CustomFooter { get; }

        string CustomHeader { get; }

        string Footer { get; }

        string Header { get; }

        string HeaderFooterHeight { get; }

        string HiddenVisibility { get; }

        string HorizontalAlignment { get; }

        string ImageFileToBigMessage { get; }

        string ImageProperties { get; }

        string InsertDate { get; }

        string InsertNumberOfPages { get; }

        string InsertPageNumber { get; }

        string InsertPicture { get; }

        string InsertTime { get; }

        string LeftSection { get; }

        string NormalVisibility { get; }

        string NumberOfSections { get; }

        string OK { get; }

        string PageNumberPosition { get; }

        string Print { get; }

        string PrintingAdvancedOptions { get; }

        string PrintoutVisibility { get; }

        string RemoveAllColorsFromPrintout { get; }

        string RemovePicture { get; }

        string RightSection { get; }

        string ShowFooter { get; }

        string ShowHeader { get; }

        string ShowPageNumber { get; }

        string ShowTableHeaderOnEveryPage { get; }

        string Stretch { get; }

        string VerticalAlignment { get; }

        string Width { get; }
    }
}

