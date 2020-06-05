

public class Transition
{
  public Transition(ICondition condition, State targetState, Activity triggerActivity)
  {
    this.condition = condition;
    this.targetState = targetState;
    this.triggerActivity = triggerActivity;
  }

  private ICondition condition;

  private State targetState;

  private Activity triggerActivity;

  public bool CanTrigger()
  {
    return condition.Check();
  }

  public State Trigger()
  {
    if (triggerActivity != null)
      triggerActivity.OnStart();

    return targetState;
  }

  public void Reset()
  {
    condition.Reset();
  }
}
