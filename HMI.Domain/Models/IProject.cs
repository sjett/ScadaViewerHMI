// SPDX-License-Identifier: GPL-3.0-only
using System.Collections.ObjectModel;

namespace HMI.Domain.Models
{
    /// <summary>
    /// 项目接口
    /// </summary>
    public interface IProject
    {
        /// <summary>
        /// Web服务基础URI, 
        /// 通信服务地址，这里已准备放弃使用，后期改为本地服务/Mqtt/OpcUa等
        /// </summary>
        //Uri WebServicesBaseUri
        //{
        //    get;
        //}
        /// <summary>
        /// 服务器名称
        /// </summary>
        string ServerName
        {
            get;
        }
        /// <summary>
        /// 项目媒体文件夹，
        /// 后期用Sqlite加密存储，包括图片、视频、SVG等
        /// </summary>
        string ProjectMediaFolder
        {
            get;
        }
        /// <summary>
        /// 项目文件夹
        /// </summary>
        string ProjectFolder
        {
            get;
        }
        /// <summary>
        /// 项目图片文件夹
        /// </summary>
        string ProjectImagesFolder
        {
            get;
        }

        /// <summary>
        /// 项目XAML文件夹
        /// </summary>
        string ProjectXamlsFolder
        {
            get;
        }

        /// <summary>
        /// 项目图片列表
        /// </summary>
        ObservableCollection<string> ProjectImages
        {
            get;
        }
        /// <summary>
        /// 项目媒体列表
        /// </summary>
        ObservableCollection<string> ProjectMedia
        {
            get;
        }
        /// <summary>
        /// 项目XAML列表
        /// </summary>
        ObservableCollection<string> ProjectXamls
        {
            get;
        }
    }
}
