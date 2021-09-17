using System.Collections.Generic;
using UnityEngine;

public abstract class DataTableBase
{
    protected IDataTableHelper dataTableHelper;

    public abstract bool HasData(int index);
    public abstract void ReadData();
    public abstract bool AddDataRow(string rowString);
    public void SetDataTableHelper(IDataTableHelper dataTableHelper) {
        this.dataTableHelper = dataTableHelper;
    }

    public abstract int Count { get; }
}