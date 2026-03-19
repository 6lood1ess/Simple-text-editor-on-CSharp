using System;
using System.Collections.Generic;

public class TextEditor {

  private TextFile _currentFile;
  private Stack<Memento> _history = new Stack<Memento>();
  private bool _isFileOpened = false;

  private class Memento {
    public List<string> LinesContent { get; set; }
    public DateTime Timestamp { get; set; }
  }

  public bool Open(string path) {

    try {

      if (!File.Exists(path)) {
        Console.WriteLine($"File not found: {path}");
        return false;
      }

      _currentFile = new TextFile(path);
      _history.Clear();

      var initialLines = new List<string>(_currentFile.Content.Split(new[] { Environment.NewLine }, StringSplitOptions.None));
      _history.Push(new Memento { 
        LinesContent = new List<string>(initialLines),
        Timestamp = DateTime.Now 
      });

      _isFileOpened = true;
      return true;

    } catch (Exception exception) {
      Console.WriteLine($"Error opening file: {exception.Message}");
      _isFileOpened = false;
      return false;
    }
  }

  public void AddLine(string newLine) {

    if (!_isFileOpened) {
      Console.WriteLine("Open a file first!");
      return;
    }
    
    var currentLines = new List<string>(_history.Peek().LinesContent);
    
    currentLines.Add(newLine);
    _currentFile.Content = string.Join(Environment.NewLine, currentLines);
    SaveSnapshot(new List<string>(currentLines));
  }

  private void SaveSnapshot(List<string> lines) {
    _history.Push(new Memento {
      LinesContent = new List<string>(lines),
      Timestamp = DateTime.Now
    });
  }

  public void Undo() {

    if (!_isFileOpened) {
      Console.WriteLine("\nOpen the file first! >:O");
      return;
    }

    if (_history.Count > 1) {

      _history.Pop();

      var previousState = _history.Peek();
      _currentFile.Content = string.Join(Environment.NewLine, previousState.LinesContent);
      Console.WriteLine($"\nUndo to version from {_history.Peek().Timestamp}");

    } else {
      Console.WriteLine("\nNo changes to undo");
    }
  }

  public void Save() {

    if (!_isFileOpened) {
      Console.WriteLine("\nOpen the file first! >:O");
      return;
    }

    try {
      _currentFile.Save();
      Console.WriteLine("\nFile saved successfully");

    } catch (Exception exception) {
      Console.WriteLine($"\nSave error: {exception.Message}");
    }
  }

  public string GetContent() {
    return _isFileOpened ? _currentFile.Content : "! FILE NOT OPENED !";
  }

  public void DisplayLines() {

    if (!_isFileOpened) {
      Console.WriteLine("! FILE NOT OPENED !");
      return;
    }
    
    var lines = _history.Peek().LinesContent;
    Console.WriteLine($"File content ({lines.Count} lines):\n" +
                      new string('-', 50));
    
    for (int lineIndex = 0; lineIndex < lines.Count; ++lineIndex) {
      Console.WriteLine($"{lineIndex + 1}: {lines[lineIndex]}");
    }
    
    Console.WriteLine(new string('-', 50));
  }


  public bool IsFileOpened() {
    return _isFileOpened;
  }

  public TextFile GetCurrentTextFile() {
    return _currentFile;
  }
}