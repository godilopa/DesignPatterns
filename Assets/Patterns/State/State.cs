
using System.Collections.Generic;

public class State
{
  public State(Activity enterActivity, Activity stateActivity, Activity exitActivity)
  {
    this.enterActivity = enterActivity;
    this.stateActivity = stateActivity;
    this.exitActivity = exitActivity;
  }

  public List<Transition> Transitions { get => transitions; set => transitions = value; }

  private List<Transition> transitions;

  private Activity enterActivity;
  private Activity stateActivity;
  private Activity exitActivity;

  void SetEnterAction(Activity enterActivity) { this.enterActivity = enterActivity; }
  void SetStateAction(Activity stateActivity) { this.stateActivity = stateActivity; }
  void SetExitAction(Activity exitActivity) { this.exitActivity = exitActivity; }

  public void OnStart()
  {
    if (enterActivity != null) enterActivity.OnStart();

    for (int i = 0; i < transitions.Count; i++)
      transitions[i].Reset();
  }

  public void OnUpdate()
  {
    if (stateActivity != null) stateActivity.OnUpdate();
  }

  public void OnExit()
  {
    if (exitActivity != null) exitActivity.OnExit();
  }
}
