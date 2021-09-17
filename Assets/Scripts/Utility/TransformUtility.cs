using System.Collections.Generic;
using UnityEngine;

public static class TransformUtility
{
    public static Transform GetChild(this Transform instance, string name) {
        Transform transform = instance.transform;

        for (int i = 0; i < instance.transform.childCount; i++) {
            if (transform.GetChild(i).name == name) return transform.GetChild(i);
        }

        return null;
    }

    public static void DestroyAllChildren(this Transform transform) {
        List<Transform> toDestroy = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++) {
            toDestroy.Add(transform.GetChild(i));
        }

        foreach (var item in toDestroy) {
            GameObject.Destroy(item.gameObject);
        }
    }
}