using System;
using System.Collections.Generic;

namespace Generics.Tables
{
    public class Table<TRow, TColumn, TValue>
    {
        public readonly List<TRow> Rows = new List<TRow>();
        public readonly List<TColumn> Columns = new List<TColumn>();

        private readonly Dictionary<KeyValuePair<TRow, TColumn>, TValue> _items =
            new Dictionary<KeyValuePair<TRow, TColumn>, TValue>();

        public TableOpen<TRow, TColumn, TValue> Open => new TableOpen<TRow, TColumn, TValue>(this);

        public TableExisted<TRow, TColumn, TValue> Existed => new TableExisted<TRow, TColumn, TValue>(this);

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

        public TValue this[TRow row, TColumn column]
        {
            get
            {
                if (!_items.ContainsKey(new KeyValuePair<TRow, TColumn>(row, column)))
                    _items[new KeyValuePair<TRow, TColumn>(row, column)] = default;

                return _items[new KeyValuePair<TRow, TColumn>(row, column)];
            }
            set
            {
                _items[new KeyValuePair<TRow, TColumn>(row, column)] = value;
                AddColumn(column);
                AddRow(row);
            }
        }
    }

    public class TableOpen<TRow, TColumn, TValue>
    {
        private readonly Table<TRow, TColumn, TValue> _table;

        public TableOpen(Table<TRow, TColumn, TValue> table)
        {
            _table = table;
        }

        public TValue this[TRow row, TColumn column]
        {
            get => _table[row, column];
            set => _table[row, column] = value;
        }
    }

    public class TableExisted<TRow, TColumn, TValue>
    {
        private readonly Table<TRow, TColumn, TValue> _table;

        public TableExisted(Table<TRow, TColumn, TValue> table)
        {
            _table = table;
        }

        public TValue this[TRow row, TColumn column]
        {
            get
            {
                if (_table.Rows.Contains(row) && _table.Columns.Contains(column))
                    return _table[row, column];

                throw new ArgumentException();
            }
            set
            {
                if (_table.Rows.Contains(row) && _table.Columns.Contains(column))
                    _table[row, column] = value;
                else
                    throw new ArgumentException();
            }
        }
    }
}