using System;
using System.Collections.Generic;

public class TextEditor {

  private TextFile _currentFile;
  private Stack<Memento> _history = new Stack<Memento>();

  private class Memento {
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
  }

  public void Open(string path) {
    _currentFile = new TextFile(path);
    SaveSnapshot();
  }

  public void Edit(string newContent) {
    _currentFile.Content = newContent;
    SaveSnapshot();
  }

  private void SaveSnapshot() {
    _history.Push(new Memento { Content = _currentFile.Content, Timestamp = DateTime.Now });
  }

  public void Undo() {
    if (_history.Count > 1) {
      _history.Pop();
      _currentFile.Content = _history.Peek().Content;
    }
  }

  public void Save() => _currentFile.Save();
  public string GetContent() => _currentFile.Content;
}
