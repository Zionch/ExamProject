using System;
using System.Collections.Generic;

public class DataTableManager
{
    public static DataTableManager Instance { get; private set; }
    private IDataTableHelper dataTableHelper;

    private Dictionary<string, DataTableBase> dataTables;

    private const string DataRowClassPrefixName = "DR";

    public void Init() {
        Instance = this;

        dataTables = new Dictionary<string, DataTableBase>();
    }

    public void SetDataTableHelper(IDataTableHelper dataTableHelper) {
        this.dataTableHelper = dataTableHelper;
    }

    public void LoadDataTable(string dataTableName) {
        var dataTableClassName = string.Concat(DataRowClassPrefixName, dataTableName);
        var dataRowType = Type.GetType(dataTableClassName);
        if (dataRowType == null) {
            return;
        }

        Type dataTableType = typeof(DataTable<>).MakeGenericType(dataRowType);
        DataTableBase dataTableBase = (DataTableBase)Activator.CreateInstance(dataTableType);
        dataTableBase.SetDataTableHelper(dataTableHelper);
        dataTableBase.ReadData();

        dataTables.Add(dataTableName, dataTableBase);
    }

    public bool HasDataTable(string dataTableName) {
        return dataTables.ContainsKey(dataTableName);
    }

    public DataTable<T> GetDataTable<T>() where T : DataRowBase, new() {
        var key = typeof(T).ToString().StartsWith(DataRowClassPrefixName) ?
            typeof(T).ToString().Substring(2) : typeof(T).ToString();
        if (!dataTables.ContainsKey(key)) {
            return null;
        }

        return (DataTable<T>)dataTables[key];
    }

    public void UnloadDataTable(string dataTableName) {
        if (!HasDataTable(dataTableName)) return;

        dataTables.Remove(dataTableName);
    }

    public void UnloadDataTable<T>() where T : DataRowBase {
        var key = typeof(T).ToString().StartsWith(DataRowClassPrefixName) ?
            typeof(T).ToString().Substring(2) : typeof(T).ToString();

        if (!HasDataTable(key)) return;

        dataTables.Remove(key);
    }

    public class DataTable<T> : DataTableBase where T : DataRowBase, new()
    {
        private Dictionary<int, T> dataRows;

        public override void ReadData() {
            dataRows = new Dictionary<int, T>();

            ParseData(dataTableHelper.ReadData(typeof(T).ToString()));
        }

        public T GetData(int index) {
            if (!dataRows.ContainsKey(index)) return null;

            return dataRows[index];
        }

        private void ParseData(string data) {
            dataTableHelper.ParseData(this, data);
        }

        public override bool AddDataRow(string rowString) {
            T row = new T();

            if (!row.ParseDataRow(rowString)) {
                return false;
            }
            dataRows.Add(row.Id, row);

            return true;
        }

        public override int Count => dataRows.Count;

        public override bool HasData(int i) {
            return dataRows.ContainsKey(i);
        }

        public T[] GetAllDataRows() {
            List<T> rows = new List<T>();

            foreach (var item in dataRows.Values) {
                rows.Add(item);
            }

            return rows.ToArray();
        }
    }
}