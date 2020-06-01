
using UnityEngine;

namespace CommandVisitorExample
{
  public class TavernVisitor : MonoBehaviour, IVisitor
  {
    //You can also change the interface so there exists one different visit per concrete IElement
    public void Visit<T>(T concreteElement) where T : IElement
    {
      if (typeof(T) == typeof(Character))
        Debug.Log("Hello player");

      if (typeof(T) == typeof(CharacterShield))
        Debug.Log("Beautiful shield");

      if (typeof(T) == typeof(CharacterSword))
        Debug.Log("Beautiful sword");
    }
  }
}
