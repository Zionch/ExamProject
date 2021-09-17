using UnityEngine;

public abstract class SceneObject : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer != GameSettings.PlayerLayer) {
            return;
        }

        OnPlayerTriggerEnter(collision);
    }

    protected abstract void OnPlayerTriggerEnter(Collider2D collision);
}