using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    public GameEvent game_event;
    public UnityEvent on_event_triggered;

    void OnEnable() {
        game_event.AddListener(this);
    }

    void OnDisable() {
        game_event.RemoveListener(this);
    }

    public void OnEventTriggered(){
        on_event_triggered.Invoke();
    }
}
