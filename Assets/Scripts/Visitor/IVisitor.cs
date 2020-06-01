
public interface IVisitor
{
  void Visit<T>(T robot) where T : IElement;
}
