using UnityEngine;
using UnityEngine.UI;

public class ButtonCondition : ICondition
{
  public ButtonCondition(Button button)
  {
    button.onClick.AddListener(Pressed);
  }

  private Button button;

  private bool buttonPressed = false;

  private void Pressed()
  {
    Debug.Log("ButtonCondition Pressed");
    buttonPressed = true;
  }

  public bool Check()
  {
    return buttonPressed;
  }

  public void Reset()
  {
    Debug.Log("ButtonCondition Reset");
    buttonPressed = false;
  }
}
