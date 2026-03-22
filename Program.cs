using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program {
  static void Main() {

    while (true) {

      try {

        Console.WriteLine("\n--=| TEXT FILE PROCESSING PROGRAM |=--" +
                          "\n\nMain menu:\n1. File Editor\n2. File Search\n3. Index files\n4. Exit");

        Console.Write("\nYour choice: ");
        string userChoice = Console.ReadLine();

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

    TextEditor textEditor = new TextEditor();

    Console.Write("\nFile path: ");
    string filePath = Console.ReadLine();

    if (!File.Exists(filePath)) {
      Console.WriteLine($"\nFile not found: {filePath}");
      return;
    }

    TextFile temporaryFile = null;
    string temporaryPath;
    string extension = Path.GetExtension(filePath).ToLower();

    try {

      if (extension == ".xml") {
        temporaryFile = TextFile.XmlDeserialize(filePath);
        temporaryPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(filePath) + "_temp.txt");
        File.WriteAllText(temporaryPath, temporaryFile.Content);
        filePath = temporaryPath;

      } else if (extension == ".bin") {
        temporaryFile = TextFile.BinaryDeserialize(filePath);
        temporaryPath = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(filePath) + "_temp.txt");
        File.WriteAllText(temporaryPath, temporaryFile.Content);
        filePath = temporaryPath;
      }
    } catch (Exception exception) {
      Console.WriteLine($"Error loading file: {exception.Message}");
      return;
    }

    if (!textEditor.Open(filePath)) {
      return;
    }

    while (true) {

      textEditor.DisplayContent();

      Console.WriteLine("\nCommands:" +
                        "\n1. Full rewrite content" +
                        "\n2. Undo" +
                        "\n3. Save as TXT and exit" +
                        "\n4. Save as Binary and exit" +
                        "\n5. Save as XML and exit" +
                        "\n6. Exit without saving");

      Console.Write("\nYour command: ");
      string editorCommand = Console.ReadLine();

      if (editorCommand == "1") {
        Console.WriteLine("\nEnter text line by line. Press Enter on an empty line to finish:\n");
        string newContent = ReadMultilineInput();
        textEditor.SetContent(newContent);

      } else if (editorCommand == "2") {
        textEditor.Undo();

      } else if (editorCommand == "3") {
        textEditor.Save();
                
        // If it was a temporary file, save back to the original format
        if (temporaryFile != null) {
          string originalPath = filePath.Replace("_temp.txt", "");

          if (File.Exists(originalPath)) {

            if (extension == ".xml") {
              temporaryFile.Content = textEditor.GetContent();
              temporaryFile.XmlSerialize(originalPath);
              Console.WriteLine($"\nSaved back to XML: {originalPath}");

            } else if (extension == ".bin") {
              temporaryFile.Content = textEditor.GetContent();
              temporaryFile.BinarySerialize(originalPath);
              Console.WriteLine($"\nSaved back to binary: {originalPath}");
            }
          }
        }

        break;

      } else if (editorCommand == "4") {
        SaveFileAsBinary(textEditor, filePath);
        break;

      } else if (editorCommand == "5") {
        SaveFileAsXml(textEditor, filePath);
        break;

      } else if (editorCommand == "6") {
        Console.WriteLine("\nExiting without saving . . .");
        break;

      } else {
        Console.WriteLine("Invalid command. Please enter 1-6");
      }

      if (filePath.Contains("_temp.txt") && File.Exists(filePath)) {
        try { 
          File.Delete(filePath);
        } catch { }
      }
    }
  }

  static string ReadMultilineInput() {
    
    string result = "";
    
    while (true) {
      string line = Console.ReadLine();

      if (string.IsNullOrEmpty(line)) {
        break;
      }
        
      result += line + Environment.NewLine;
    }
    
    return result.TrimEnd(Environment.NewLine.ToCharArray());
  }

  static void SaveFileAsBinary(TextEditor textEditor, string originalPath) {

    string input, savePath;

    try {

      Console.Write("\nEnter binary file path (or press Enter for default): ");
      input = Console.ReadLine();
      
      savePath = string.IsNullOrEmpty(input) ? Path.ChangeExtension(originalPath, ".bin") : input;
      
      TextFile fileToSave = new TextFile();
      fileToSave.Content = textEditor.GetContent();
      fileToSave.FilePath = savePath;
      fileToSave.BinarySerialize(savePath);
      
      Console.WriteLine($"\nFile saved as binary: {savePath}");

    } catch (Exception exception) {
      Console.WriteLine($"\nError saving binary: {exception.Message}");
    }
  }

  static void SaveFileAsXml(TextEditor textEditor, string originalPath) {

    string input, savePath;

    try {

      Console.Write("\nEnter XML file path (or press Enter for default): ");
      input = Console.ReadLine();
      
      savePath = string.IsNullOrEmpty(input) ? Path.ChangeExtension(originalPath, ".xml") : input;
      
      TextFile fileToSave = new TextFile();
      fileToSave.Content = textEditor.GetContent();
      fileToSave.FilePath = savePath;
      fileToSave.XmlSerialize(savePath);
      
      Console.WriteLine($"\nFile saved as XML: {savePath}");

    } catch (Exception exception) {
      Console.WriteLine($"\nError saving XML: {exception.Message}");
    }
  }

  static void RunFileSearcher() {

    FileSearcher fileSearcher = new FileSearcher();

    try {

      Console.Write("\nEnter directory to search: ");
      string directory = Console.ReadLine();
      
      if (string.IsNullOrWhiteSpace(directory)) {
        Console.WriteLine("\nDirectory not specified");
        return;
      }

      Console.Write("Enter keywords (comma separated): ");
      string keywordsInput = Console.ReadLine();
      
      if (string.IsNullOrWhiteSpace(keywordsInput)) {
        Console.WriteLine("\nKeywords not specified");
        return;
      }

      string[] keywords = keywordsInput.Split(',').Select(keyword => keyword.Trim()).ToArray();

      if (keywords.Length == 0 || keywords.All(string.IsNullOrEmpty)) {
        Console.WriteLine("\nNo valid keywords");
        return;
      }

      var foundFiles = fileSearcher.SearchByKeywords(directory, keywords);
      Console.WriteLine($"\nFiles found: {foundFiles.Count}");

      if (foundFiles.Count > 0) {
        Console.WriteLine("File list:");
        foreach (var file in foundFiles) {
          Console.WriteLine($"  {file}");
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
      string directory = Console.ReadLine();

      if (string.IsNullOrWhiteSpace(directory)) {
        Console.WriteLine("\nDirectory not specified");
        return;
      }

      Console.Write("Enter keywords (comma separated): ");
      string keywordsInput = Console.ReadLine();
      
      if (string.IsNullOrWhiteSpace(keywordsInput)) {
        Console.WriteLine("\nKeywords not specified");
        return;
      }

      string[] keywords = keywordsInput.Split(',').Select(keyword => keyword.Trim()).ToArray();

      indexer.BuildIndex(directory, keywords);
      Console.WriteLine("\nIndexing results:");
      indexer.DisplayIndexResults();

    } catch (Exception exception) {
      Console.WriteLine($"\nIndexing error: {exception.Message}");
    }
  }
}