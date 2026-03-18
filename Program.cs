using System;

class Program {
  static void Main() {

    while (true) {
      Console.WriteLine("\n1. File Editor\n2. File Search\n3. Indexing\n4. Exit");

      Console.Write("\nYour choice:");
      var userChoice = Console.ReadLine();

      if (userChoice == "1") {
        RunTextEditor();
      } else if (userChoice == "2") {
        RunFileSearcher();
      } else if (userChoice == "3") {
        RunFileIndexer();
      } else {
        break;
      }
    }
  }

  static void RunTextEditor() {

    var textEditor = new TextEditor();
    Console.Write("File path: ");
    textEditor.Open(Console.ReadLine());

    while (true) {

      Console.WriteLine("\nCurrent text:\n" + textEditor.GetContent());
      Console.WriteLine("\n1. Edit\n2. Undo\n3. Save and exit");

      Console.Write("Your command: ");
      var editorCommand = Console.ReadLine();

      if (editorCommand == "1") {
        Console.Write("New text: ");
        textEditor.Edit(Console.ReadLine());
      } else if (editorCommand == "2") {
        textEditor.Undo();
      } else if (editorCommand == "3") {
        textEditor.Save();
        break;
      }
    }
  }

  static void RunFileSearcher() {

    var fileSearcher = new FileSearcher();

    Console.Write("Directory: ");
    var directory = Console.ReadLine();

    Console.Write("Keywords (comma separated): ");
    var keywords = Console.ReadLine().Split(',');

    var foundFiles = fileSearcher.SearchByKeywords(directory, keywords);
    Console.WriteLine("Found files:");
    foreach (var file in foundFiles) {
      Console.WriteLine(file);
    }
  }

  static void RunFileIndexer() {

    var indexer = new FileIndexer();

    Console.Write("Directory for indexing: ");
    var directory = Console.ReadLine();

    Console.Write("Keywords: ");
    var keywords = Console.ReadLine().Split(',');

    indexer.BuildIndexForDirectory(directory, keywords);
    Console.WriteLine("Indexing results:");
    indexer.DisplayIndexResults();
  }
}
