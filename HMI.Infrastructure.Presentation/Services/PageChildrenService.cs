// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Presentation.Views;
using HMI.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using NLog;
using HMI.Common.Instrumentation.ErrorHandling;
using System.Windows;
namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 页面子项服务
    /// </summary>
	public class PageChildrenService : BasePageService, IPageService, IPageChildrenService
	{
		private readonly Logger logger = LogManager.GetLogger("PageChildrenService");
		 readonly ISerializerFactory serializerFactory;
		 readonly IZOrderService zOrderService;
		private readonly IActionMonitor actionMonitor;
		public PageChildrenService(
			ISerializerFactory serializerFactory, 
			IZOrderService zOrderService, 
			IActionMonitor actionMonitor)
		{
			Contract.Requires<ArgumentNullException>(zOrderService != null, "zOrderService");
			Contract.Requires<ArgumentNullException>(serializerFactory != null, "serializerFactory");
			Contract.Requires<ArgumentNullException>(actionMonitor != null, "actionMonitor");

            this.zOrderService = zOrderService;
            this.actionMonitor = actionMonitor;
            this.serializerFactory = serializerFactory;
		}
		public void AddControlFromDragDropOperation(IDataObject dragDropData, Point dropPosition)
		{
			string[] formats = dragDropData.GetFormats();
			if (formats == null || formats.Length == 0)
			{
				return;
			}
			object data = dragDropData.GetData(formats[0]);
			if (!(data is CommonControl))
			{
				return;
			}
            actionMonitor.StartGrouping();
			ICommonControlViewModel controlViewModel = CreateDraggedControl(data);
			if (controlViewModel != null)
			{
                Page.Children.Add(controlViewModel);
				PositionAndSelectControl(controlViewModel, dropPosition);
			}
            actionMonitor.EndGrouping();
		}
        ICommonControlViewModel CreateDraggedControl(object draggedControl)
        {
            Type controlType = draggedControl.GetType();
            return DeserializeDraggedControl(draggedControl, controlType);
        }
        ICommonControlViewModel DeserializeDraggedControl(object draggedControl, Type controlType)
        {
            JProperty? staticSymbol = CreateStaticSymbolProperty(draggedControl);
            ISerializer serializer = serializerFactory.GetSerializer(controlType);
            JObject root = CreateControlSerializationPayload(staticSymbol, controlType);
            return serializer.Deserialize(this.Page, root, ApplicationVersion.Current) as ICommonControlViewModel;
        }
        JProperty? CreateStaticSymbolProperty(object draggedControl)
        {
            FrameworkElement frameworkElement = draggedControl as FrameworkElement;
            IStaticSymbolViewModel staticSymbol = frameworkElement != null ? frameworkElement.DataContext as IStaticSymbolViewModel : null;
            if (staticSymbol == null)
            {
                return null;
            }
            return new JProperty("ContentString", staticSymbol.ContentString);
        }
        JObject CreateControlSerializationPayload(JProperty? staticSymbol, Type controlType)
        {
            var attr = new JObject
                            {
                                new JProperty("Name", controlType.Name)
                            };
            if (staticSymbol != null)
                attr.Add(staticSymbol);

            return new JObject
                    {
                        new JProperty("Definition", new JObject
                        {
                            new JProperty("TypeName", controlType.AssemblyQualifiedName) 
                        }),
                        new JProperty("Properties", new JObject
                        {
                            new JProperty("Attributes", attr) 
                        }),
                        new JProperty("ID", Guid.NewGuid())
                    };
        }
        void PositionAndSelectControl(ICommonControlViewModel control, Point dropPosition)
        {
            PlaceControlAtDropPosition(control, dropPosition);
            this.zOrderService.BringToFront(this.Page, new ICommonControlViewModel[]
            {
                control
            });
            control.Select();
        }
        void PlaceControlAtDropPosition(ICommonControlViewModel control, Point dropPosition)
        {
            control.Height = control.DesignerDesiredSize.Height;
            control.Width = control.DesignerDesiredSize.Width;
            control.ControlHeight = control.DesignerDesiredSize.Height;
            control.ControlWidth = control.DesignerDesiredSize.Width;
            control.ZIndex = -1000000;
            control.XTranslation = dropPosition.X - control.Width / 2.0;
            control.YTranslation = dropPosition.Y - control.Height / 2.0;
        }
    }
}
