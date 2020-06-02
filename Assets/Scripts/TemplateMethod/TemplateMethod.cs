
using UnityEngine;

public abstract class TemplateMethod : MonoBehaviour
{
  //The skeleton of an algorithm.
  public void ExecuteMethod()
  {
    this.BaseOperation1();
    this.RequiredOperation1();
    this.BaseOperation2();
    this.OptionalOperation1();
    this.RequiredOperation2();
    this.OptionalOperation2();
  }

  // These operations could have implementations.
  protected void BaseOperation1()
  {
    Debug.Log("Template Method BaseOperation1 running.");
  }

  protected void BaseOperation2()
  {
    Debug.Log("Template Method BaseOperation2 running.");
  }

  // These operations have to be implemented in subclasses.
  protected abstract void RequiredOperation1();

  protected abstract void RequiredOperation2();

  // Subclasses may override them, but it's not
  // mandatory. Provide additional extension points in some
  // crucial places of the algorithm.
  protected virtual void OptionalOperation1() { }

  protected virtual void OptionalOperation2() { }
}