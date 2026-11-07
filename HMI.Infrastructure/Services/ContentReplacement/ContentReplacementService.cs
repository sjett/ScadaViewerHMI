// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Metadata;
using NLog;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Text.RegularExpressions;

namespace HMI.Infrastructure.Services.ContentReplacement
{
    public class ContentReplacementService : IContentReplacementService
    {
        private static readonly Logger Logger = LogManager.GetLogger("ContentReplacementService");
        private readonly IFileSystemService _fileSystemService;
        private string _replacementPattern = "[{0}]";
        public string ReplacementPattern
        {
            get
            {
                return _replacementPattern;
            }
            set
            {
                _replacementPattern = value;
            }
        }
        public ContentReplacementService(IFileSystemService fileSystemService)
        {
            Contract.Requires<ArgumentNullException>(fileSystemService != null);
            _fileSystemService = fileSystemService;
        }
        public string ReplaceStringContent(string content, object valueHolder)
        {
            Contract.Requires<ArgumentNullException>(valueHolder != null);
            if (string.IsNullOrEmpty(content) || string.IsNullOrWhiteSpace(content))
            {
                return content;
            }
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(valueHolder);
            if (properties.Count == 0)
            {
                return content;
            }
            foreach (PropertyDescriptor propertyDescriptor in properties)
            {
                try
                {
                    object propertyValue = Reflection.GetPropertyValue(valueHolder, propertyDescriptor.Name);
                    string replacement = string.Empty;
                    if (propertyValue != null)
                    {
                        replacement = propertyValue.ToString();
                    }
                    content = Regex.Replace(content, Regex.Escape(string.Format(ReplacementPattern, propertyDescriptor.Name)), replacement, RegexOptions.CultureInvariant);
                }
                catch (Exception exception)
                {
                    Logger.WarnException("Error performing replacement", exception);
                }
            }
            return content;
        }
        public string ReplaceFileContent(string contentFilePath, object valueHolder)
        {
            Contract.Requires<ArgumentNullException>(valueHolder != null);
            string fileContent = _fileSystemService.GetFileContent(contentFilePath);
            return ReplaceStringContent(fileContent, valueHolder);
        }
        public void ReplaceFileContent(string contentFilePath, string targetFilePath, object valueHolder)
        {
            Contract.Requires<ArgumentNullException>(valueHolder != null);
            string fileContent = _fileSystemService.GetFileContent(contentFilePath);
            string fileContent2 = ReplaceStringContent(fileContent, valueHolder);
            _fileSystemService.WriteFileContent(targetFilePath, fileContent2);
        }
    }
}

