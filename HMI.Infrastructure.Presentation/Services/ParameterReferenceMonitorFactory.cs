// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Services;
using HMI.Infrastructure.Presentation.Factories;
using HMI.Infrastructure.Presentation.ViewModels;
using HMI.Infrastructure.Services;
using HMI.Common.Instrumentation.ErrorHandling;

namespace HMI.Infrastructure.Presentation.Services
{
    /// <summary>
    /// 参数引用监视器工厂
    /// </summary>
    public class ParameterReferenceMonitorFactory : IParameterReferenceMonitorFactory
	{
		private readonly IPropertyInspectorService _propertyInspectorService;
		public ParameterReferenceMonitorFactory(IPropertyInspectorService propertyInspectorService)
		{
			Contract.Requires<ArgumentNullException>(propertyInspectorService != null, "propertyInspectorService");
            _propertyInspectorService = propertyInspectorService;
		}
        /// <summary>
        /// 创建监视器
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public IParameterReferenceMonitor CreateMonitor(IHierarchicalNode node)
		{
			Contract.Requires<ArgumentNullException>(node != null, "node");
			return new ParameterReferenceMonitor(node, _propertyInspectorService);
		}
	}
}
