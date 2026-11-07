// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.Serialization;
using HMI.Infrastructure.Presentation.ViewModels;
using Newtonsoft.Json.Linq;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Windows;

namespace HMI.Infrastructure.Presentation.Services
{

    /// <summary>
    /// 剪贴板管理服务
    /// </summary>
    public class ClipboardManagerService : IClipboardManagerService
    {
        private readonly DataFormat dataFormat = DataFormats.GetDataFormat("EditorClipboardFormat");
        private readonly ISerializerFactory serializerFactory;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serializerFactory"></param>
        public ClipboardManagerService(ISerializerFactory serializerFactory)
        {
            Contract.Requires<ArgumentNullException>(serializerFactory != null);
            this.serializerFactory = serializerFactory;
        }
        /// <summary>
        /// 设置剪贴板数据
        /// </summary>
        /// <param name="controls"></param>
        public void SetData(IEnumerable<ICommonControlViewModel> controls)
        {
            if (controls == null)
            {
                return;
            }
            JArray jarr = new JArray();
            var jobjArr = controls.Select(t =>
            {
                ISerializer serializer = this.serializerFactory.GetSerializer(t.ControlType);
                return serializer.Serialize(t);
            });
            foreach (var item in jobjArr)
            {
                jarr.Add(item);
            }
            Clipboard.Clear();// 清空剪贴板，防止残留数据干扰
            var data = jarr.ToString();
            DataObject dataObject = new DataObject(this.dataFormat.Name, data );
            Clipboard.SetDataObject(dataObject);// 设置剪贴板数据
        }
        /// <summary>
        /// 是否有数据
        /// </summary>
        /// <returns></returns>
        public bool HasData()
        {
            return Clipboard.ContainsData(this.dataFormat.Name);
        }
        /// <summary>
        /// 获取剪贴板数据
        /// </summary>
        /// <param name="parentPage"></param>
        /// <returns></returns>
        public ICollection<ICommonControlViewModel> GetData(IProjectPageViewModel parentPage)
        {
            IDataObject dataObject = Clipboard.GetDataObject();
            var clipboardObject =(string)dataObject.GetData(this.dataFormat.Name);
            
            var xElement = JArray.Parse(clipboardObject);
            return xElement.Select(t =>
            {
                Type controlType = CommonControlSerializer.GetControlType((t["Definition"] as JObject)!);
                ISerializer serializer = this.serializerFactory.GetSerializer(controlType);
                return (ICommonControlViewModel)serializer.Deserialize(parentPage, t as JObject, ApplicationVersion.Current);
            }).ToList<ICommonControlViewModel>();
        }

    }
}
