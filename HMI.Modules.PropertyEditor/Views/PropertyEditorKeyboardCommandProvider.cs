// SPDX-License-Identifier: GPL-3.0-only
using System.Windows.Input;
using Telerik.Windows.Controls.GridView;
namespace HMI.Modules.PropertyEditor.Views
{
	public class PropertyEditorKeyboardCommandProvider : DefaultKeyboardCommandProvider
	{
		public PropertyEditorKeyboardCommandProvider(GridViewDataControl grid) : base(grid)
		{
		}
		public override IEnumerable<ICommand> ProvideCommandsForKey(Key key)
		{
			List<ICommand> list = base.ProvideCommandsForKey(key).ToList<ICommand>();
			if (key == Key.Return)
			{
				list.Clear();
			}
			return list;
		}
	}
}
