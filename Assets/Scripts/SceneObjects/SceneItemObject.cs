using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemEventArgs : GameEventArgs
{
    public static int EventId => typeof(GetItemEventArgs).GetHashCode();
    public override int Id => EventId;

    public readonly int ItemId;

    public GetItemEventArgs(int itemId) {
        ItemId = itemId;
    }

    public static GetItemEventArgs Create(int itemId) {
        return new GetItemEventArgs(itemId);
    }
}

public class SceneItemObject : SceneObject
{
    [SerializeField]
    private int itemId;

    protected override void OnPlayerTriggerEnter(Collider2D collision) {
        EventManager.Instance.Fire(this, GetItemEventArgs.Create(itemId));

        gameObject.SetActive(false);
    }
}
