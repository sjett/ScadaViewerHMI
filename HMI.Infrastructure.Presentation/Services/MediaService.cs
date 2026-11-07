// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using HMI.Domain.Services;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Common.Instrumentation.ErrorHandling;
using System.IO;
using System.Reactive.Linq;//

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 媒体服务实现类，负责媒体文件的添加和获取
    /// </summary>
    public class MediaService : IMediaService
    {
        // 文件系统服务，用于文件操作
        private readonly IFileSystemService fileSystemService;
        // 项目管理器视图模型，用于访问项目相关数据
        private readonly IProjectManagerViewModel projectManagerViewModel;

        /// <summary>
        /// 构造函数，初始化依赖项
        /// </summary>
        /// <param name="fileSystemService">文件系统服务</param>
        /// <param name="solution">项目管理器视图模型</param>
        public MediaService(IFileSystemService fileSystemService, IProjectManagerViewModel solution)
        {
            Contract.Requires<ArgumentNullException>(solution != null, "solution");
            Contract.Requires<ArgumentNullException>(fileSystemService != null);
            this.fileSystemService = fileSystemService;
            projectManagerViewModel = solution;
        }

        /// <summary>
        /// 添加媒体文件到项目媒体文件夹，并更新项目媒体集合
        /// </summary>
        /// <param name="mediaPath">媒体文件路径</param>
        /// <param name="projectMediaFolder">项目媒体文件夹路径</param>
        /// <param name="mediaType">媒体类型</param>
        public void AddMedia(string mediaPath, string projectMediaFolder, MediaType mediaType)
        {
            Contract.Requires<ArgumentNullException>(mediaPath != null);
            Contract.Requires<ArgumentException>(mediaPath.Length > 0);
            Contract.Requires<ArgumentNullException>(projectMediaFolder != null);
            Contract.Requires<ArgumentException>(projectMediaFolder.Length > 0);

            // 复制媒体文件到项目媒体文件夹
            fileSystemService.Copy(mediaPath, projectMediaFolder);
            string fileName = Path.GetFileName(mediaPath);

            // 获取当前项目视图模型
            IProjectViewModel project = projectManagerViewModel.Project;

            // 根据媒体类型将文件名插入到对应集合
            switch (mediaType)
            {
                case MediaType.Image:
                    project.ProjectImages.Insert(0, fileName);
                    return;
                case MediaType.Xaml:
                    // project.ProjectXamls.Insert(0, fileName);
                    return;
                case MediaType.Audio:
                    project.ProjectMedia.Insert(0, fileName);
                    return;
                default:
                    return;
            }
        }

        /// <summary>
        /// 获取指定类型的媒体文件集合（异步可观察序列）
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="mediaFolderPath">媒体文件夹路径</param>
        /// <returns>媒体文件路径的可观察序列</returns>
        public IObservable<string> GetMediaFiles(MediaType mediaType, string mediaFolderPath)
        {
            Contract.Requires<ArgumentNullException>(mediaFolderPath != null);
            Contract.Requires<ArgumentException>(mediaFolderPath.Length > 0);

            // 获取文件夹中的所有文件并转换为可观察序列
            IObservable<string> iobservable_ = fileSystemService.GetFilesInFolder(mediaFolderPath).ToObservable<string>();

            // 根据媒体类型筛选文件
            switch (mediaType)
            {
                case MediaType.Image:
                    return iobservable_.Where(t=> checkExtensions(t,
                    [
                        ".png",
                        ".jpg",
                        ".gif",
                        ".bmp"
                    ]));
                case MediaType.Xaml:
                    return iobservable_.Where(t=> checkExtensions(t,
                    [
                        ".xaml"
                    ]));
                case MediaType.Audio:
                    return iobservable_.Where(t=> checkExtensions(t,
                    [
                        ".mp3",
                        ".wma"
                    ]));
                default:
                    return null;
            }
        }
        bool checkExtensions(string fileName, params string[] extList)
        {
            string extension = Path.GetExtension(fileName);
            return extList.Contains(extension, StringComparer.InvariantCultureIgnoreCase);
        }

    }
}
