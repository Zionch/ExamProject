public static class AssetUtility
{
    public static string GetTexture(string category, string assetName) {
        return string.Format("Texture/{0}/{1}", category, assetName);
    }

    public static string GetDataTable(string dataTable) {
        return string.Format("DataTable/{0}", dataTable);
    }

    public static string GetPrefab(string prefab) {
        return string.Format("Prefab/{0}", prefab);
    }
}