
using UnityEngine;

namespace CommandPatternExample
{
  public class Jump : Command
  {
    public override void Execute(params object[] paramList)
    {
      Debug.Log("Player Jumping");
    }
  }
}
