using UnityEngine;

public static class GameObjectUtility 
{
    public static GameObject GetChild(this GameObject instance, string name) {
        Transform transform = instance.transform;

        for (int i = 0; i < instance.transform.childCount; i++) {
            if (transform.GetChild(i).name == name) return transform.GetChild(i).gameObject;
        }

        return null;
    }
}