
using UnityEngine;

namespace CommandVisitorExample
{
  public class CharacterShield : MonoBehaviour, IElement
  {
    public void Accept(IVisitor elementVisitor)
    {
      elementVisitor.Visit(this);
    }
  }
}
