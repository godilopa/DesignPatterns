
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
  public List<State> States { get => states; set => states = value; }

  public State CurrentState { get => currentState; set => currentState = value; }

  private State currentState;

  private List<State> states;

  /// <summary>
  /// Update is called every frame, if the MonoBehaviour is enabled.
  /// </summary>
  private void Update()
  {
    if (currentState != null)
      currentState.OnUpdate();
  }

  /// <summary>
  /// Comprueba las transiciones del estado actual y cambia de estado si debe hacerlo.
  /// </summary>
  private void LateUpdate()
  {
    if (currentState != null)
    {
      List<Transition> transitions = currentState.Transitions;

      for (int i = 0; i < transitions.Count; i++)
      {
        if (transitions[i].CanTrigger() == true)
        {
          currentState.OnExit();
          State nextState = transitions[i].Trigger();
          nextState.OnStart();
          currentState = nextState;
          break;
        }
      }
    }
  }
}
