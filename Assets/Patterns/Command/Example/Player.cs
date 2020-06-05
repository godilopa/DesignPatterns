
using System.Collections.Generic;
using UnityEngine;

namespace CommandPatternExample
{
  public class Player : MonoBehaviour
  {
    SeveralCommands commands = new SeveralCommands();

    private void Start()
    {
      Shoot shoot = new Shoot();
      Jump jump = new Jump();

      commands.Commands = new Dictionary<string, Command>(){
        { "shoot", shoot },
        { "jump", jump}
      };
    }

    private void Update()
    {
      if (Input.GetKeyDown(KeyCode.Space))
        commands.Execute("jump");

      if (Input.GetKeyDown(KeyCode.F))
        commands.Execute("shoot");
    }
  }
}
