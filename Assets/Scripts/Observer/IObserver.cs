

public enum NotifyType { NOTIFY1, NOTIFY2, NOTIFY3, NOTIFY4 }

public interface IObserver
{
  void OnNotify(NotifyType type);
}