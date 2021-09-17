public class DRItem : DataRowBase
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public string IconFileName { get; private set; }
    public int DefaultAmount { get; private set; }

    public override bool ParseDataRow(string dataRowString) {
        string[] columnStrings = dataRowString.Split(DataTableExtension.DataSplitSeparators);
        for (int i = 0; i < columnStrings.Length; i++) {
            columnStrings[i] = columnStrings[i].Trim(DataTableExtension.DataTrimSeparators);
        }

        int index = 0;

        index++;
        Id = int.Parse(columnStrings[index++]);
        Name = columnStrings[index++];
        Description = columnStrings[index++];
       
        IconFileName = columnStrings[index++];
        DefaultAmount = int.Parse(columnStrings[index++]);

        return true;
    }
}