using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateExample
{
  public class MainMenuActivity : Activity
  {
    override public void OnStart() { Debug.Log("MainMenuActivity Start"); }
    override public void OnUpdate() { Debug.Log("MainMenuActivity OnUpdate"); }
    override public void OnExit() { Debug.Log("MainMenuActivity OnExit"); }
  }
}
