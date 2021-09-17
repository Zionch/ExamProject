using UnityEngine;

public class SceneObjSystem
{
    private GameObject choco_d;

    public void Init() {
        var choco_dPrefab = ResourceManager.Instance.LoadAsset(AssetUtility.GetPrefab("Choco_D")) as GameObject;

        choco_d = GameObject.Instantiate(choco_dPrefab, new Vector2(1.8f, -2.8f), Quaternion.identity);
    }

    public void Shutdown() {
        if (choco_d) {
            GameObject.Destroy(choco_d);
        }
    }
}