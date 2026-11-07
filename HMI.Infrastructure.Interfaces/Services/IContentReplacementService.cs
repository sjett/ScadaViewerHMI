// SPDX-License-Identifier: GPL-3.0-only
namespace HMI.Infrastructure.Services
{
    public interface IContentReplacementService
    {
        string ReplaceStringContent(string content, object valueHolder);
        string ReplaceFileContent(string contentFilePath, object valueHolder);
        void ReplaceFileContent(string contentFilePath, string targetFilePath, object valueHolder);
    }

}
