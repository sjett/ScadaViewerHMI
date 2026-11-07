// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.ErrorHandling;
using System.IO;
using System.IO.Compression;

namespace HMI.Infrastructure.Services
{
    public class FileSystemService : IFileSystemService
    {
        
        private static readonly Func<FileInfo, string> _getFileName = GetFileName;
        
        private static readonly Func<DirectoryInfo, string> _getDirectoryName = GetDirectoryName;
        public string DefaultProjectFolder
        {
            get
            {
                string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
                string path = "HMI Editor Projects";
                return Path.Combine(folderPath, path);
            }
        }
        public bool IsValidFolder(string folderPath)
        {
            try
            {
                new DirectoryInfo(folderPath);
                return true;
            }
            catch (ArgumentException)
            {
            }
            catch (PathTooLongException)
            {
            }
            catch (NotSupportedException)
            {
            }
            return false;
        }
        public bool FolderExists(string folderPath)
        {
            return Directory.Exists(folderPath);
        }
        public void EnsureFolderExists(string folder)
        {
            if (!FolderExists(folder))
            {
                Directory.CreateDirectory(folder);
                File.SetAttributes(folder, FileAttributes.Normal);
            }
        }
        public string GetFileNameWithoutExtension(string filePath)
        {
            return Path.GetFileNameWithoutExtension(filePath);
        }
        public string GetFolder(string filePath)
        {
            return Path.GetDirectoryName(filePath);
        }
        public void Copy(string sourceFilePath, string destFolder)
        {
            Contract.Requires<ArgumentNullException>(sourceFilePath != null);
            Contract.Requires<ArgumentException>(sourceFilePath.Length > 0);
            Contract.Requires<ArgumentNullException>(destFolder != null);
            Contract.Requires<ArgumentException>(destFolder.Length > 0);
            EnsureFolderExists(destFolder);
            string fileName = Path.GetFileName(sourceFilePath);
            string destFileName = Path.Combine(destFolder, fileName);
            File.Copy(sourceFilePath, destFileName, true);
        }
        public void CopyFolder(string sourceFolder, string destFolder, bool ignoreMissingSourceFolder = true)
        {
            if (!Directory.Exists(sourceFolder) && ignoreMissingSourceFolder)
            {
                return;
            }
            CopyAllDirectoryFiles(sourceFolder, destFolder);
        }
        public void CopyFiles(IEnumerable<string> files, string sourceDirectory, string destinationDirectory)
        {
            Contract.Requires<ArgumentNullException>(files != null);
            Contract.Requires<ArgumentNullException>(sourceDirectory != null);
            Contract.Requires<ArgumentException>(sourceDirectory.Length > 0);
            Contract.Requires<ArgumentNullException>(destinationDirectory != null);
            Contract.Requires<ArgumentException>(destinationDirectory.Length > 0);
            Contract.Requires<ArgumentException>(Directory.Exists(sourceDirectory));
            EnsureFolderExists(destinationDirectory);
            foreach (string current in files)
            {
                string sourceFilePath = sourceDirectory + "\\" + current;
                Copy(sourceFilePath, destinationDirectory);
            }
        }
        public void CopyZippedFiles(IEnumerable<string> files, string sourceZip, string destinationDirectory)
        {
            
            using (var zipFile = ZipFile.OpenRead(sourceZip))
            {
                foreach (string current in files)
                {
                    ExtractZipEntry(zipFile, current, destinationDirectory);
                }
            }
        }
        private void ExtractZipEntry(ZipArchive archive, string entryName, string destinationDirectory)
        {
            var entry = archive.GetEntry(entryName);
            if (entry == null)
            {
                throw new FileNotFoundException("File not found in ZipFile", entryName);
            }
            string filePath = Path.Combine(destinationDirectory, entryName);
            entry.ExtractToFile(filePath, true);
        }
        public void CopyAllDirectoryFiles(string sourceDirectory, string destinationDirectory)
        {
            Contract.Requires<ArgumentNullException>(sourceDirectory != null);
            Contract.Requires<ArgumentException>(sourceDirectory.Length > 0);
            Contract.Requires<ArgumentNullException>(destinationDirectory != null);
            Contract.Requires<ArgumentException>(destinationDirectory.Length > 0);
            Contract.Requires<ArgumentException>(Directory.Exists(sourceDirectory));
            IEnumerable<string> filesInFolder = GetFilesInFolder(sourceDirectory);
            CopyFiles(filesInFolder, sourceDirectory, destinationDirectory);
        }
        public IEnumerable<string> GetFilesInFolder(string folderPath)
        {
            Contract.Requires<ArgumentNullException>(folderPath != null);
            Contract.Requires<ArgumentException>(folderPath.Length != 0);
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            if (!directoryInfo.Exists)
            {
                return Enumerable.Empty<string>();
            }
            return directoryInfo.EnumerateFiles().Select(_getFileName);
        }
        public IEnumerable<string> GetDirectoriesInFolder(string folderPath)
        {
            Contract.Requires<ArgumentNullException>(folderPath != null);
            Contract.Requires<ArgumentException>(folderPath.Length != 0);
            DirectoryInfo directoryInfo = new DirectoryInfo(folderPath);
            if (!directoryInfo.Exists)
            {
                return Enumerable.Empty<string>();
            }
            return directoryInfo.EnumerateDirectories().Select(_getDirectoryName);
        }
        public Stream OpenRead(string filePath)
        {
            return File.OpenRead(filePath);
        }
        public Stream Create(string filePath)
        {
            return File.Create(filePath);
        }
        public string GetFileContent(string filePath)
        {
            Contract.Requires<ArgumentException>(ExistsFile(filePath));
            return File.ReadAllText(filePath);
        }
        public void WriteFileContent(string filePath, string fileContent)
        {
            if (!ExistsFile(filePath))
            {
                File.Create(filePath).Close();
            }
            File.WriteAllText(filePath, fileContent);
        }
        public void Delete(string path, string name)
        {
            Contract.Requires<ArgumentNullException>(path != null);
            Contract.Requires<ArgumentException>(path.Length > 0);
            Contract.Requires<ArgumentNullException>(name != null);
            Contract.Requires<ArgumentException>(name.Length > 0);
            string fullPath = Path.Combine(path, name);
            Delete(fullPath);
        }
        public void Delete(string fullPath)
        {
            if (Directory.Exists(fullPath))
            {
                File.SetAttributes(fullPath, FileAttributes.Normal);
                Directory.Delete(fullPath, true);
                return;
            }
            if (ExistsFile(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        public void Rename(string path, string newName, string oldName)
        {
            Contract.Requires<ArgumentNullException>(path != null);
            Contract.Requires<ArgumentException>(path.Length > 0);
            Contract.Requires<ArgumentNullException>(newName != null);
            Contract.Requires<ArgumentException>(newName.Length > 0);
            Contract.Requires<ArgumentNullException>(oldName != null);
            Contract.Requires<ArgumentException>(oldName.Length > 0);
            string sourcePath = Path.Combine(path, oldName);
            string destinationPath = Path.Combine(path, newName);
            if (Directory.Exists(sourcePath))
            {
                Directory.Move(sourcePath, destinationPath);
                return;
            }
            if (ExistsFile(sourcePath))
            {
                File.Move(sourcePath, destinationPath);
            }
        }
        public bool ExistsFile(string filePath)
        {
            return File.Exists(filePath);
        }
        
        private static string GetFileName(FileInfo fileInfo)
        {
            return fileInfo.Name;
        }
        
        private static string GetDirectoryName(DirectoryInfo directoryInfo)
        {
            return directoryInfo.Name;
        }
    }
}
