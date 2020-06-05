using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CommandVisitorExample
{
  public class Character : MonoBehaviour, IElement
  {
    [SerializeField]
    private CharacterSword sword = null;

    [SerializeField]
    private CharacterShield shield = null;

    [ContextMenu("Test")]
    public void Test()
    {
      TavernVisitor tavern = FindObjectOfType<TavernVisitor>();
      Accept(tavern);
    }

    public void Accept(IVisitor visitor)
    {
      visitor.Visit(this);

      if (sword != null)
        sword.Accept(visitor);

      if (shield != null)
        shield.Accept(visitor);
    }
  }
}
