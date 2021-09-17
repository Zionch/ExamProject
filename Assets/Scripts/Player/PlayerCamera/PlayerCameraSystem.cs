using UnityEngine;

public class PlayerCameraSystem
{
    private PlayerCamera playerCam;
    private Transform target;

    public void Init() {
        playerCam = GameObject.FindObjectOfType<PlayerCamera>();
        target = GameObject.FindObjectOfType<PlayerController>().transform;
    }

    public void Update() {
        if (!target || !playerCam) return;

        var pos = playerCam.transform.position;
        pos.x = target.position.x;

        playerCam.transform.position = pos;
    }

    public void Shutdown() {
        target = null;
    }
}