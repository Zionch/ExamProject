using System.IO;
using UnityEngine;

public class ResourceManager
{
    public static ResourceManager Instance { get; private set; }

    public void Init() {
        Instance = this;
    }

    public Object LoadAsset(string filepath) {
        return Resources.Load(filepath);
    }
}