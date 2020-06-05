using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateExample
{
  public class PauseActivity : Activity
  {
    override public void OnStart() { Debug.Log("PauseActivity Start"); }
    override public void OnUpdate() { Debug.Log("PauseActivity OnUpdate"); }
    override public void OnExit() { Debug.Log("PauseActivity OnExit"); }
  }
}
