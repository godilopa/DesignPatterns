
using System.Collections.Generic;

public class SeveralCommands
{
  public Dictionary<string, Command> Commands { get => commands; set => commands = value; }

  private Dictionary<string, Command> commands = null;

  public void Execute(string actionName, params object[] paramList)
  {
    commands[actionName].Execute(paramList);
  }
}
