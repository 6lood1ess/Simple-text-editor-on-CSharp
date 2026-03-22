using System;
using System.IO;

public class TextEditor : IOriginator {

  public TextFile currentFile;
  public Caretaker caretaker = new Caretaker();
  public bool isFileOpened = false;

  public bool Open(string path) {

    try {

      if (!File.Exists(path)) {
        Console.WriteLine($"\nFile not found: {path}");
        return false;
      }

      currentFile = new TextFile(path);
      caretaker.Clear();
      caretaker.SaveState(this);
      isFileOpened = true;
      return true;

    } catch (Exception exception) {
      Console.WriteLine($"\nError opening file: {exception.Message}");
      isFileOpened = false;
      return false;
    }
  }

  public void SetContent(string newContent) {

    if (!isFileOpened) {
      Console.WriteLine("\nOpen the file first! >:O");
      return;
    }

    currentFile.Content = newContent;
    caretaker.SaveState(this);
    Console.WriteLine("Content updated :)");
  }

  public string GetContent() {

    if (!isFileOpened) {
      return "! FILE NOT OPENED !";
    }
        
    return currentFile.Content;
  }

  public void Undo() {

    if (!isFileOpened) {
      Console.WriteLine("\nOpen the file first! >:O");
      return;
    }

    if (caretaker.CanUndo()) {
      caretaker.RestoreState(this);
      Memento memento = (Memento)caretaker.GetCurrentState();
      Console.WriteLine($"\nUndo to version from {memento.Timestamp}");

    } else {
      Console.WriteLine("\nNo changes to undo");
    }
  }

  public void Save() {

    if (!isFileOpened) {
      Console.WriteLine("\nOpen the file first! >:O");
      return;
    }

    try {
      currentFile.Save();
      Console.WriteLine("\nFile saved successfully :)");

    } catch (Exception exception) {
      Console.WriteLine($"\nSave error: {exception.Message}");
    }
  }

  public void DisplayContent() {

    if (!isFileOpened) {
      Console.WriteLine("! FILE NOT OPENED !");
      return;
    }
    
    Console.WriteLine($"FILE CONTENT:\n" +
                      new string('-', 50) +
                      $"{currentFile.Content}" +
                      new string('-', 50));
  }


  public bool IsFileOpened() {
    return isFileOpened;
  }

  public TextFile GetCurrentTextFile() {
    return currentFile;
  }

  public object GetMemento() {
    return new Memento(currentFile.Content);
  }

  public void SetMemento(object memento) {
    if (memento is Memento mem) {
      currentFile.Content = mem.Content;
    }
  }
}