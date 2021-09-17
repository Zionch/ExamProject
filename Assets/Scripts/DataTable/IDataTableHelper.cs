public interface IDataTableHelper
{
    bool ParseData(DataTableBase dataTable, string dataTableString);
    string ReadData(string dataTableName);
}