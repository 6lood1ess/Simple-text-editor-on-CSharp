using System;

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
    textEditor.Open(Console.ReadLine());

    while (true) {

      try {

        Console.WriteLine("\n-- Editor --" +
                          "\nFile content:" +
                          $"\n{new string('-', 50)}" +
                          $"\n{textEditor.GetContent()}" +
                          $"\n{new string('-', 50)}");

        Console.WriteLine("\nCommands:\n1. Add new line\n2. Undo\n3. Save and exit\n4. Exit without saving");

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

  static void RunFileSearcher() {

    var fileSearcher = new FileSearcher();

    try {

      Console.Write("\nEnter directory to search: ");
      var directory = Console.ReadLine();
      
      if (string.IsNullOrWhiteSpace(directory)) {
        Console.WriteLine("Directory not specified");
        return;
      }

      Console.Write("Enter keywords: ");
      var keywordsInput = Console.ReadLine();
      
      if (string.IsNullOrWhiteSpace(keywordsInput)) {
        Console.WriteLine("Keywords not specified");
        return;
      }

      var keywords = Console.ReadLine().Split(',');

      if (keywords.Length == 0) {
        Console.WriteLine("No valid keywords");
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
      Console.WriteLine($"Search error: {exception.Message}");
    }
  }

  static void RunFileIndexer() {

    var indexer = new FileIndexer();

    try {

      Console.Write("Directory for indexing: ");
      var directory = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(directory)) {
        Console.WriteLine("Directory not specified");
        return;
      }

      Console.Write("Enter keywords: ");
      var keywords = Console.ReadLine().Split(',');

      indexer.BuildIndexForDirectory(directory, keywords);
      Console.WriteLine("Indexing results:");
      indexer.DisplayIndexResults();

    } catch (Exception exception) {
      Console.WriteLine($"Indexing error: {exception.Message}");
    }
  }
}
