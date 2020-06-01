
using UnityEngine;

namespace CommandPatternExample
{
  public class Shoot : Command
  {
    public override void Execute(params object[] paramList)
    {
      Debug.Log("Player Shooting");
    }
  }
}
