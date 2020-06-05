
using System.Collections.Generic;
using UnityEngine.Events;

namespace SingletonExample
{
  public class EventManager : Singleton<EventManager>
  {
    private Dictionary<string, UnityEvent> events = new Dictionary<string, UnityEvent>();

    public static void StartListening(string eventName, UnityAction listener)
    {
      UnityEvent thisEvent = null;

      if (Instance.events.TryGetValue(eventName, out thisEvent))
        thisEvent.AddListener(listener);
      else
      {
        thisEvent = new UnityEvent();
        thisEvent.AddListener(listener);
        Instance.events.Add(eventName, thisEvent);
      }
    }

    public static void StopListening(string eventName, UnityAction listener)
    {
      UnityEvent thisEvent = null;

      if (Instance.events.TryGetValue(eventName, out thisEvent))
        thisEvent.RemoveListener(listener);
    }

    public static void TriggerEvent(string eventName)
    {
      UnityEvent thisEvent = null;

      if (Instance.events.TryGetValue(eventName, out thisEvent))
        thisEvent.Invoke();
    }
  }
}
