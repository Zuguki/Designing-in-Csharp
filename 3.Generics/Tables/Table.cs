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

        public TValue this[TRow row, TColumn column]
        {
            get => _items.First(item => item.Row.Equals(row) && item.Column.Equals(column)).Value;
            set
            {
                if (!_items.Any(item => item.Row.Equals(row) && item.Column.Equals(column)))
                {
                    _items.Add(new TableData<TRow, TColumn, TValue>(row, column, value));
                    Rows.Add(default);
                    Columns.Add(default);
                }
                else
                {
                    _items.First(item 
                        => item.Row.Equals(row) && item.Column.Equals(column)).Value = value;
                }
            }
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
