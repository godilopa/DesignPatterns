
using UnityEngine;

namespace CommandVisitorExample
{
  public class CharacterSword : MonoBehaviour, IElement
  {
    public void Accept(IVisitor elementVisitor)
    {
      elementVisitor.Visit(this);
    }
  }
}
