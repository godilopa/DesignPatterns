
using UnityEngine;

namespace StateExample
{
  public class GameActivity : Activity
  {
    override public void OnStart() { Debug.Log("GameActivity Start"); }
    override public void OnUpdate() { Debug.Log("GameActivity OnUpdate"); }
    override public void OnExit() { Debug.Log("GameActivity OnExit"); }
  }
}
