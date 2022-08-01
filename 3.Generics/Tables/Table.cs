using System.Collections.Generic;
using System.Linq;

namespace Generics.Tables
{
    public class Table<TRow, TColumn, TValue>
    {
        public List<TRow> Rows = new List<TRow>();
        public List<TColumn> Columns = new List<TColumn>();
        
        public List<TableData<TRow, TColumn, TValue>> Items = new List<TableData<TRow, TColumn, TValue>>();

        public TableOpen<TRow, TColumn, TValue> Open => new TableOpen<TRow, TColumn, TValue>(this);

        public Table<TRow, TColumn, TValue> Existed => this;

        public void AddRow(TRow row)
        {
            if (!Rows.Contains(row))
                Rows.Add(row);
        }

        public void AddColumn(TColumn column)
        {
            if (!Columns.Contains(column))
                Columns.Add(column);
        }
    }

    public class TableData<TRow, TColumn, TValue>
    {
        public TRow Row;
        public TColumn Column;
        public TValue Value;

        public TableData(TRow row, TColumn column, TValue value)
        {
            Row = row;
            Column = column;
            Value = value;
        }
    }

    public class TableOpen<TRow, TColumn, TValue>
    {
        private Table<TRow, TColumn, TValue> _table;

        public TableOpen(Table<TRow, TColumn, TValue> table)
        {
            _table = table;
        }
        
        public TValue this[TRow row, TColumn column]
        {
            get
            {
                if (_table.Rows.Contains(row) && _table.Columns.Contains(column) 
                    && _table.Items.Any(item => item.Row.Equals(row) && item.Column.Equals(column)))
                    return _table.Items.First(item 
                        => item.Row.Equals(row) && item.Column.Equals(column)).Value;
                
                return default;
            }
            set
            {
                if (_table.Rows.Contains(row) && _table.Columns.Contains(column))
                {
                    if (_table.Items.Any(item => item.Row.Equals(row) && item.Column.Equals(column)))
                        _table.Items.First(item => item.Row.Equals(row) && item.Column.Equals(column))
                            .Value = value;
                    else
                        _table.Items.Add(new TableData<TRow, TColumn, TValue>(row, column, value));
                }
                else
                {
                    _table.Items.Add(new TableData<TRow, TColumn, TValue>(row, column, value));
                    _table.AddRow(row);
                    _table.AddColumn(column);
                }
            }
        }

    }
}
