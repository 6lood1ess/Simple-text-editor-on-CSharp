using System.Collections.Generic;

public class Caretaker {
  
  Stack<object> history = new Stack<object>();

  public void SaveState(IOriginator originator) {
    history.Push(originator.GetMemento());
  }

  public void RestoreState (IOriginator originator) {
    if (history.Count > 1) {
      history.Pop();
      originator.SetMemento(history.Peek());
    }
  }

  public bool CanUndo() {
    return history.Count > 1;
  }

  public void Clear() {
    history.Clear();
  }

  public object GetCurrentState() {
    return history.Count > 0 ? history.Peek() : null;
  }
}

