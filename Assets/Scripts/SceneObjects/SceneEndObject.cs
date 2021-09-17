using UnityEngine;

public class TriggerEndGameEventArgs : GameEventArgs
{
    public static int EventId => typeof(TriggerEndGameEventArgs).GetHashCode();
    public override int Id => EventId;

    public static TriggerEndGameEventArgs Create() {
        return new TriggerEndGameEventArgs();
    }
}

public class SceneEndObject : SceneObject
{
    protected override void OnPlayerTriggerEnter(Collider2D collision) {
        EventManager.Instance.Fire(this, TriggerEndGameEventArgs.Create());
    }
}