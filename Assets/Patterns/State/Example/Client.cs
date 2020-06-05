
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace StateExample
{
  public class Client : MonoBehaviour
  {
    [SerializeField]
    private StateMachine gameStateMachine = null;

    [SerializeField]
    private Button fromPauseToMenuButton = null;

    [SerializeField]
    private Button fromPauseToGameButton = null;

    [SerializeField]
    private Button fromGameToPauseButton = null;

    [SerializeField]
    private Button fromMenuToGameButton = null;

    void Awake()
    {
      PauseActivity pauseActivity = new PauseActivity();
      GameActivity gameActivity = new GameActivity();
      MainMenuActivity menuActitvity = new MainMenuActivity();

      State pauseState = new State(null, pauseActivity, null);
      State gameState = new State(null, gameActivity, null);
      State menuState = new State(null, menuActitvity, null);

      Transition menuToGameTransition = new Transition(new ButtonCondition(fromMenuToGameButton), gameState, null);
      Transition gameToPauseTransition = new Transition(new ButtonCondition(fromGameToPauseButton), pauseState, null);
      Transition pauseToGameTransition = new Transition(new ButtonCondition(fromPauseToGameButton), gameState, null);
      Transition pauseToMenuTransition = new Transition(new ButtonCondition(fromPauseToMenuButton), menuState, null);

      pauseState.Transitions = new List<Transition>() { pauseToGameTransition, pauseToMenuTransition };
      gameState.Transitions = new List<Transition>() { gameToPauseTransition };
      menuState.Transitions = new List<Transition>() { menuToGameTransition };

      gameStateMachine.States = new List<State>() { pauseState, gameState, menuState };
      gameStateMachine.CurrentState = menuState;
    }
  }
}
