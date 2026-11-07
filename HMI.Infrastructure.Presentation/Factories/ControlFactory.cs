// SPDX-License-Identifier: GPL-3.0-only
using System;
using System.Diagnostics;
using HMI.Infrastructure.Presentation.ViewModels;

namespace HMI.Infrastructure.Presentation.Factories
{
    public class ControlFactory : IControlFactory
    {
        public IDesignerControl CreateControl(Type type)
        {
            Trace.WriteLine("Creating control " + type.ToString());
            IDesignerControl result = Activator.CreateInstance(type) as IDesignerControl;
            Trace.WriteLine("Created control " + type.ToString());
            return result;
        }
        public IDesignerControl CreateControl(string controlTypeFullName)
        {
            Type type = Type.GetType(controlTypeFullName, false);
            return this.CreateControl(type);
        }
    }
}
