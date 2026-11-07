// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;

namespace HMI.Domain.Services
{
    /// <summary>
    /// 媒体服务接口
    /// </summary>
    public interface IMediaService
    {
        /// <summary>
        /// 添加媒体文件
        /// </summary>
        /// <param name="mediaPath">媒体文件路径</param>
        /// <param name="projectMediaFolder">项目媒体文件夹</param>
        /// <param name="mediaType">媒体类型</param>
        void AddMedia(string mediaPath, string projectMediaFolder, MediaType mediaType);
        /// <summary>
        /// 获取媒体文件
        /// </summary>
        /// <param name="mediaType">媒体类型</param>
        /// <param name="mediaFolderPath">媒体文件夹</param>
        /// <returns>媒体文件路径</returns>
        IObservable<string> GetMediaFiles(MediaType mediaType, string mediaFolderPath);
    }
}
