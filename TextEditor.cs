using System;
using System.Collections.Generic;

public class TextEditor {

  private TextFile _currentFile;
  private Stack<Memento> _history = new Stack<Memento>();
  private bool _isFileOpened = false;

  private class Memento {
    public string Content { get; set; }
    public DateTime Timestamp { get; set; }
  }

  public bool Open(string path) {

    try {
      _currentFile = new TextFile(path);
      _history.Clear();
      SaveSnapshot();

      _isFileOpened = true;
      return true;

    } catch (Exception exception) {
      Console.WriteLine($"Error opening file: {exception.Message}");
      _isFileOpened = false;
      return false;
    }
  }

  public void Edit(string newContent) {

    if (!_isFileOpened) {
      Console.WriteLine("You haven’t opened a text file to edit it");
      return;
    }

    _currentFile.Content = newContent;
    SaveSnapshot();
  }

  private void SaveSnapshot() {
    _history.Push(new Memento {
      Content = _currentFile.Content,
      Timestamp = DateTime.Now
    });
  }

  public void Undo() {

    if (!_isFileOpened) {
      Console.WriteLine("Open the file first! >:O");
      return;
    }

    if (_history.Count > 1) {
      _history.Pop();
      _currentFile.Content = _history.Peek().Content;
      Console.WriteLine($"Undo to version from {_history.Peek().Timestamp}");

    } else {
      Console.WriteLine("No changes to undo");
    }
  }

  public void Save() {

    if (!_isFileOpened) {
      Console.WriteLine("Open the file first! >:O");
      return;
    }

    try {
      _currentFile.Save();
      Console.WriteLine("File saved successfully");

    } catch (Exception exception) {
      Console.WriteLine($"Save error: {exception.Message}");
    }
  }

  public string GetContent() {
    return _isFileOpened ? _currentFile.Content : "! FILE NOT OPENED !";
  }

  public bool IsFileOpened() {
    return _isFileOpened;
  }
}
