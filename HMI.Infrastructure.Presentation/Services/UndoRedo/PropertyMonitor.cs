// SPDX-License-Identifier: GPL-3.0-only
using HMI.Common.Instrumentation.Events;
using HMI.Common.Instrumentation.Metadata;
using HMI.Common.Presentation.Events;
using HMI.Infrastructure.Services;
using System.ComponentModel;
using HMI.Common.Instrumentation.ErrorHandling;
namespace HMI.Infrastructure.Presentation.Services.UndoRedo
{
    /// <summary>
    /// 属性监视器实现
    /// </summary>
    public class PropertyMonitor : IPropertyMonitor
	{
		private readonly IActionMonitor actionMonitor;
        private readonly Dictionary<string, object> propDict = new Dictionary<string, object>();
		private INotifyProperty notifyProperty;
        /// <summary>
        ///		属性监视器构造函数
        /// </summary>
        /// <param name="actionMonitor"></param>
        public PropertyMonitor(IActionMonitor actionMonitor)
		{
			Contract.Requires<ArgumentNullException>(actionMonitor != null, "actionMonitor");
			this.actionMonitor = actionMonitor;
		}
        /// <summary>
        /// 开始监视
        /// </summary>
        /// <param name="target"></param>
        public void Start(INotifyProperty target)
		{
			Contract.Requires<ArgumentNullException>(target != null, "target");
			Contract.Requires<InvalidOperationException>(this.notifyProperty == null, "target");
			this.notifyProperty = target;
			target.PropertyChanging += new PropertyChangingEventHandler(this.onPropertyChanging);
			target.PropertyChanged += new PropertyChangedEventHandler(this.onPropertyChanged);
		}
        /// <summary>
        /// 停止监视
        /// </summary>
        public void Stop()
		{
			Contract.Requires<InvalidOperationException>(this.notifyProperty != null, "target");
			this.notifyProperty!.PropertyChanged -= new PropertyChangedEventHandler(this.onPropertyChanged);
			this.notifyProperty.PropertyChanging -= new PropertyChangingEventHandler(this.onPropertyChanging);
			this.notifyProperty = null;
		}
		private void onPropertyChanging(object? sender, PropertyChangingEventArgs e)
		{
			string propertyName = e.PropertyName!;
			object propertyValue = Reflection.GetPropertyValue(this.notifyProperty, propertyName);
            this.propDict[propertyName] = propertyValue;
        }
		private void onPropertyChanged(object? sender, PropertyChangedEventArgs e)
		{
			if (!this.propDict.ContainsKey(e.PropertyName!))
			{
				return;
			}
			object propertyValue = Reflection.GetPropertyValue(this.notifyProperty, e.PropertyName!);
			IAction action = new PropertyChangedAction(this.notifyProperty, e.PropertyName!, this.propDict[e.PropertyName!], propertyValue);
			this.actionMonitor.RecordAction(action);
            this.propDict[e.PropertyName!] = propertyValue;
        }
	}
}
