// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using System.Windows.Data;

namespace HMI.Modules.Design.Views
{

    public interface IEditorView : IBindingContextAware
    {
        Binding ValueBinding
        {
            get;
            set;
        }
        IServiceProvider ServiceLocator
        {
            get;
            set;
        }
    }
}
