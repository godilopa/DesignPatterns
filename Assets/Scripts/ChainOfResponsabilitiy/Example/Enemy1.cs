
using UnityEngine;

namespace ChainOfResponsabilityExample
{
  public class Enemy1 : Handler
  {
    public override void ReplyMessage()
    {
      Debug.Log("Enemy1 answering");
    }
  }
}
