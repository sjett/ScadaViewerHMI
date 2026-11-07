// SPDX-License-Identifier: GPL-3.0-only
using HMI.Infrastructure.Models;
using System.Windows.Controls;

namespace HMI.ProjectDesigner.DesignControls.OperationDiary
{
	public class ToolkitDataGridColumnsSetter
	{
		private DataGrid grid;
		private System.Collections.Generic.List<DataGridColumn> backupColumns;
		public ToolkitDataGridColumnsSetter(DataGrid grid)
		{
			this.grid = grid;
		}
		public void SetColumns(System.Collections.Generic.IEnumerable<ColumnSettingsBase> columnSettings)
		{
			this.HideAllColumns();
			this.RemoveAllColumns();
			this.RestoreColumns(columnSettings);
			if (!this.AllColumnsWereRestored())
			{
				this.RestoreRemainingColumns();
			}
		}
		public void HideAllColumns()
		{
			foreach (DataGridColumn current in this.grid.Columns)
			{
				current.Visibility=(System.Windows.Visibility.Collapsed);
			}
		}
		private void RemoveAllColumns()
		{
			this.backupColumns = new System.Collections.Generic.List<DataGridColumn>(this.grid.Columns);
			this.grid.Columns.Clear();
		}
		private void RestoreColumns(System.Collections.Generic.IEnumerable<ColumnSettingsBase> columnSettings)
		{
			foreach (ColumnSettingsBase current in columnSettings)
			{
				this.RestoreColumn(current);
			}
		}
		private void RestoreColumn(ColumnSettingsBase item)
		{
			DataGridColumn dataGridColumn = this.FindBackupColumn(item.TypeName);
			if (dataGridColumn != null)
			{
				this.RestoreColumn(item, dataGridColumn);
			}
		}
		private void RestoreColumn(ColumnSettingsBase item, DataGridColumn column)
		{
			this.SetColumn(item, column);
			this.grid.Columns.Add(column);
			this.backupColumns.Remove(column);
		}
		private DataGridColumn FindBackupColumn(string columnType)
		{
			foreach (DataGridColumn current in this.backupColumns)
			{
				if (this.ColumnIsOfType(current, columnType))
				{
					return current;
				}
			}
			return null;
		}
		private bool ColumnIsOfType(DataGridColumn column, string columnType)
		{
			return column.Header.ToString() == columnType;
		}
		private void SetColumn(ColumnSettingsBase item, DataGridColumn column)
		{
			if (!item.IsAutoSize)
			{
				column.Width=(new DataGridLength(item.Width));
			}
			else
			{
				column.Width=(new DataGridLength(1.0));
				column.Width=(DataGridLength.Auto);
			}
			column.DisplayIndex=(this.grid.Columns.Count);
			column.Visibility=(item.IsVisible ? System.Windows.Visibility.Visible : System.Windows.Visibility.Collapsed);
		}
		private bool AllColumnsWereRestored()
		{
			return this.backupColumns.Count == 0;
		}
		private void RestoreRemainingColumns()
		{
			foreach (DataGridColumn current in this.backupColumns)
			{
				current.DisplayIndex=(this.grid.Columns.Count);
				this.grid.Columns.Add(current);
			}
		}
	}
}
