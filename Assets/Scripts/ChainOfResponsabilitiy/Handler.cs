
using UnityEngine;

public abstract class Handler : MonoBehaviour
{
  [SerializeField]
  private Handler nextHandler;

  public void SetNextHandler(Handler handler)
  {
    nextHandler = handler;
  }

  [ContextMenu("Handle")]
  protected void Handle()
  {
    ReplyMessage();

    if (nextHandler != null)
      nextHandler.Handle();
  }

  public abstract void ReplyMessage();
}
