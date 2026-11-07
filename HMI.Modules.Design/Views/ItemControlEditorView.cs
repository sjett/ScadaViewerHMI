// SPDX-License-Identifier: GPL-3.0-only
using HMI.Domain.Models;
using System.Windows.Controls;
using System.Windows.Data;
namespace HMI.Modules.Design.Views
{
	public abstract class ItemControlEditorView : ItemsControl, IEditorView
	{
		
		public Binding ValueBinding
		{
			get;
			set;
		}
		public IServiceProvider ServiceLocator
		{
			get;
			set;
		}
		public IBindingSource Context
		{
			get;
			set;
		}
		public abstract void SetErrorTemplate(object template);
	}
}
