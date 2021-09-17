using System.Collections.Generic;

public abstract class GameEventArgs
{
    public abstract int Id { get; }
}

public class EventManager
{
    public static EventManager Instance { get; private set; }

    private Dictionary<int, System.EventHandler<GameEventArgs>> eventDict;

    public void Init() {
        Instance = this;
        eventDict = new Dictionary<int, System.EventHandler<GameEventArgs>>();
    }

    public void Subscribe(int id, System.EventHandler<GameEventArgs> handler) {
        if (!eventDict.ContainsKey(id)) {
            eventDict.Add(id, handler);
        } else {
            eventDict[id] += handler;
        }
    }

    public void Unsubscribe(int id, System.EventHandler<GameEventArgs> handler) {
        if (!eventDict.ContainsKey(id)) return;

        eventDict[id] -= handler;
        
        if(eventDict[id].GetInvocationList().Length == 0) {
            eventDict.Remove(id);
        }
    }

    public void Fire(object sender, GameEventArgs gameEventArgs) {
        if (!eventDict.ContainsKey(gameEventArgs.Id)) {
            return;
        }

        eventDict[gameEventArgs.Id]?.Invoke(sender, gameEventArgs);
    }
}