using System;
using System.IO;

class Program {
  static void Main() {

    while (true) {

      try {

        Console.WriteLine("\n--=| Text file processing program |=--" +
                          "\n\nMain menu:\n1. File Editor\n2. File Search\n3. Index files\n4. Exit");

        Console.Write("\nYour choice: ");
        var userChoice = Console.ReadLine();

        if (userChoice == "1") {
          RunTextEditor();
        } else if (userChoice == "2") {
          RunFileSearcher();
        } else if (userChoice == "3") {
          RunFileIndexer();
        } else if (userChoice == "4") {
          Console.WriteLine("\nProgram terminated");
          break;
        } else {
          Console.WriteLine("\nInvalid choice. Please enter 1-4");
        }
      } catch (Exception exception) {
        Console.WriteLine($"\nUnexpected error: {exception.Message}");
      }
    }
  }

  static void RunTextEditor() {

    var textEditor = new TextEditor();

    Console.Write("\nFile path: ");
    var filePath = Console.ReadLine();

    textEditor.Open(filePath);

    while (true) {

      try {

        Console.WriteLine("\n-- Editor --" +
                          "\nFile content:" +
                          $"\n{new string('-', 50)}" +
                          $"\n{textEditor.GetContent()}" +
                          $"\n{new string('-', 50)}");

        Console.WriteLine("\nCommands:" +
                          "\n1. Add new line" +
                          "\n2. Undo" +
                          "\n3. Save as TXT and exit" +
                          "\n4. Save as Binary and exit" +
                          "\n5. Save as XML and exit" +
                          "\n6. Load from Binary" +
                          "\n7. Load from XML" +
                          "\n8. Exit without saving");

        Console.Write("\nYour command: ");
        var editorCommand = Console.ReadLine();

        if (editorCommand == "1") {

          Console.WriteLine("\nEnter new line:");
          var newLine = Console.ReadLine();

          if (!string.IsNullOrEmpty(newLine)) {
            textEditor.AddLine(newLine);
            Console.WriteLine("\nLine added");
          }
        } else if (editorCommand == "2") {
          textEditor.Undo();
        } else if (editorCommand == "3") {
          textEditor.Save();
          break;
        } else if (editorCommand == "4") {
          SaveFileAsBinary(textEditor, filePath);
          break;
        } else if (editorCommand == "5") {
          SaveFileAsXml(textEditor, filePath);
          break;
        } else if (editorCommand == "6") {
          LoadFileFromBinary(textEditor, ref filePath);
        } else if (editorCommand == "7") {
          LoadFileFromXml(textEditor, ref filePath);
        } else if (editorCommand == "8") {
          Console.WriteLine("Exit without saving . . .");
          break;
        } else {
          Console.WriteLine("Invalid command! >:O");
        }
      } catch (Exception exception) {
        Console.WriteLine($"Editor error: {exception.Message}");
      }
    }
  }

  static void SaveFileAsBinary(TextEditor textEditor, string originalPath) {

    try {

      Console.Write("\nEnter binary file path (or press Enter for default): ");
      string input = Console.ReadLine();
      
      string savePath = string.IsNullOrEmpty(input) ? Path.ChangeExtension(originalPath, ".bin") : input;
      
      TextFile currentFile = textEditor.GetCurrentTextFile();
      currentFile.BinarySerialize(savePath);
      
      Console.WriteLine($"\nFile saved as binary: {savePath}");

    } catch (Exception exception) {
      Console.WriteLine($"\nError saving binary: {exception.Message}");
    }
  }

  static void SaveFileAsXml(TextEditor textEditor, string originalPath) {

    try {

      Console.Write("\nEnter XML file path (or press Enter for default): ");
      string input = Console.ReadLine();
      
      string savePath = string.IsNullOrEmpty(input) ? Path.ChangeExtension(originalPath, ".xml") : input;
      
      TextFile currentFile = textEditor.GetCurrentTextFile();
      currentFile.XmlSerialize(savePath);
      
      Console.WriteLine($"\nFile saved as XML: {savePath}");

    } catch (Exception exception) {
      Console.WriteLine($"\nError saving XML: {exception.Message}");
    }
  }

  static void LoadFileFromBinary(TextEditor textEditor, ref string filePath) {

    try {

      Console.Write("\nEnter binary file path to load: ");
      string loadPath = Console.ReadLine();
      
      if (string.IsNullOrEmpty(loadPath)) {
        Console.WriteLine("\nPath cannot be empty");
        return;
      }
      
      TextFile loadedFile = TextFile.BinaryDeserialize(loadPath);
      
      filePath = loadedFile.FilePath;
      
      if (textEditor.Open(filePath)) {
        
        Console.WriteLine("\nNote: opening file, then setting content from binary . . .");
        
        var lines = loadedFile.Content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        foreach (var line in lines) {
          if (!string.IsNullOrEmpty(line)) {
            textEditor.AddLine(line);
          }
        }
        
        Console.WriteLine($"Loaded from binary: {loadPath}");
      }
    } catch (Exception exception) {
      Console.WriteLine($"\nError loading binary: {exception.Message}");
    }
  }

  static void LoadFileFromXml(TextEditor textEditor, ref string filePath) {

    try {

      Console.Write("\nEnter XML file path to load: ");
      string loadPath = Console.ReadLine();
      
      if (string.IsNullOrEmpty(loadPath)) {
        Console.WriteLine("\nPath cannot be empty");
        return;
      }
      
      TextFile loadedFile = TextFile.XmlDeserialize(loadPath);
      
      filePath = loadedFile.FilePath;
      
      if (textEditor.Open(filePath)) {

        Console.WriteLine("\nNote: opening file, then setting content from XML . . .");
        
        var lines = loadedFile.Content.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

        foreach (var line in lines) {
          if (!string.IsNullOrEmpty(line)) {
            textEditor.AddLine(line);
          }
        }
        
        Console.WriteLine($"Loaded from XML: {loadPath}");
      }
    } catch (Exception exception) {
      Console.WriteLine($"\nError loading XML: {exception.Message}");
    }
  }

  static void RunFileSearcher() {

    var fileSearcher = new FileSearcher();

    try {

      Console.Write("\nEnter directory to search: ");
      var directory = Console.ReadLine();
      
      if (string.IsNullOrWhiteSpace(directory)) {
        Console.WriteLine("\nDirectory not specified");
        return;
      }

      Console.Write("Enter keywords: ");
      var keywordsInput = Console.ReadLine();
      
      if (string.IsNullOrWhiteSpace(keywordsInput)) {
        Console.WriteLine("\nKeywords not specified");
        return;
      }

      var keywords = Console.ReadLine().Split(',');

      if (keywords.Length == 0) {
        Console.WriteLine("\nNo valid keywords");
        return;
      }

      var foundFiles = fileSearcher.SearchByKeywords(directory, keywords);
      Console.WriteLine($"\nFiles found: {foundFiles.Count}");

      if (foundFiles.Count > 0) {
        Console.WriteLine("File list:");
        foreach (var file in foundFiles) {
          Console.WriteLine($"{file}");
        }
      }
    } catch (Exception exception) {
      Console.WriteLine($"\nSearch error: {exception.Message}");
    }
  }

  static void RunFileIndexer() {

    var indexer = new FileIndexer();

    try {

      Console.Write("\nDirectory for indexing: ");
      var directory = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(directory)) {
        Console.WriteLine("\nDirectory not specified");
        return;
      }

      Console.Write("Enter keywords: ");
      var keywords = Console.ReadLine().Split(',');

      indexer.BuildIndexForDirectory(directory, keywords);
      Console.WriteLine("\nIndexing results:");
      indexer.DisplayIndexResults();

    } catch (Exception exception) {
      Console.WriteLine($"\nIndexing error: {exception.Message}");
    }
  }
}
