// SPDX-License-Identifier: GPL-3.0-only
using System.IO;

namespace HMI.Infrastructure.Services
{
    /// <summary>
    /// 文件系统服务接口
    /// </summary>
    public interface IFileSystemService
    {
        /// <summary>
        /// 默认项目文件夹
        /// </summary>
        string DefaultProjectFolder
        {
            get;
        }
        /// <summary>
        /// 文件夹是否存在
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        bool FolderExists(string folderPath);
        /// <summary>
        /// 文件夹路径是否合法
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        bool IsValidFolder(string folderPath);
        /// <summary>
        /// 获取文件所在文件夹
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string GetFolder(string filePath);
        /// <summary>
        /// 复制文件夹
        /// </summary>
        /// <param name="sourceFolder"></param>
        /// <param name="destFolder"></param>
        /// <param name="ignoreMissingSourceFolder"></param>
        void CopyFolder(string sourceFolder, string destFolder, bool ignoreMissingSourceFolder = true);
        /// <summary>
        /// 获取文件夹中的所有文件
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        IEnumerable<string> GetFilesInFolder(string folderPath);
        /// <summary>
        /// 确保文件夹存在
        /// </summary>
        /// <param name="folder"></param>
        void EnsureFolderExists(string folder);
        /// <summary>
        /// 获取文件夹中的所有子文件夹
        /// </summary>
        /// <param name="folderPath"></param>
        /// <returns></returns>
        IEnumerable<string> GetDirectoriesInFolder(string folderPath);
        /// <summary>
        /// 复制文件夹中的所有文件
        /// </summary>
        /// <param name="sourceDirectory"></param>
        /// <param name="destinationDirectory"></param>
        void CopyAllDirectoryFiles(string sourceDirectory, string destinationDirectory);
        /// <summary>
        /// 以只读方式打开文件流
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Stream OpenRead(string filePath);
        /// <summary>
        /// 创建文件流
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        Stream Create(string filePath);
        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="sourcePath"></param>
        /// <param name="destPath"></param>
        void Copy(string sourcePath, string destPath);
        /// <summary>
        /// 获取不带扩展名的文件名
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string GetFileNameWithoutExtension(string filePath);
        /// <summary>
        /// 获取文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        string GetFileContent(string filePath);
        /// <summary>
        /// 写入文件内容
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileContent"></param>
        void WriteFileContent(string filePath, string fileContent);
        /// <summary>
        /// 复制文件集合
        /// </summary>
        /// <param name="files"></param>
        /// <param name="sourceDirectory"></param>
        /// <param name="destinationDirectory"></param>
        void CopyFiles(IEnumerable<string> files, string sourceDirectory, string destinationDirectory);
        /// <summary>
        /// 复制压缩包中的文件集合
        /// </summary>
        /// <param name="files"></param>
        /// <param name="sourceZip"></param>
        /// <param name="destinationDirectory"></param>
        void CopyZippedFiles(IEnumerable<string> files, string sourceZip, string destinationDirectory);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        void Delete(string path, string name);
        /// <summary>
        /// 删除文件
        /// </summary>
        /// <param name="fullPath"></param>
        void Delete(string fullPath);
        /// <summary>
        /// 重命名文件
        /// </summary>
        /// <param name="path"></param>
        /// <param name="newName"></param>
        /// <param name="oldName"></param>
        void Rename(string path, string newName, string oldName);
        /// <summary>
        /// 文件是否存在
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        bool ExistsFile(string filePath);
    }

}
