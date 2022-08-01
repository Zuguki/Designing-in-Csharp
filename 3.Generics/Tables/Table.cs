using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Generics.Tables
{
    public class Table<TRow, TColumn, TValue>
    {
        public List<TRow> Rows = new List<TRow>();
        public List<TColumn> Columns = new List<TColumn>();
        
        private List<TableData<TRow, TColumn, TValue>> _items = new List<TableData<TRow, TColumn, TValue>>();

        public Table<TRow, TColumn, TValue> Open => this;

        public Table<TRow, TColumn, TValue> Existed => this;

        public TValue this[TRow row, TColumn column]
        {
            get
            {
                if (Rows.Contains(row) && Columns.Contains(column) 
                    && _items.Any(item => item.Row.Equals(row) && item.Column.Equals(column)))
                    return _items.First(item 
                        => item.Row.Equals(row) && item.Column.Equals(column)).Value;
                
                return default;
            }
            set
            {
                if (Rows.Contains(row) && Columns.Contains(column))
                {
                    if (_items.Any(item => item.Row.Equals(row) && item.Column.Equals(column)))
                        _items.First(item => item.Row.Equals(row) && item.Column.Equals(column))
                            .Value = value;
                    else
                        _items.Add(new TableData<TRow, TColumn, TValue>(row, column, value));
                }
                else
                {
                    _items.Add(new TableData<TRow, TColumn, TValue>(row, column, value));
                    AddRow(row);
                    AddColumn(column);
                }
            }
        }

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
}
