using System;
using System.Collections.Generic;
using System.IO;

public class FileIndexer {
 
  private Dictionary<string, List<string>> _keywordToFilesMap = new Dictionary<string, List<string>>();

  public void BuildIndexForDirectory(string targetDirectory, string[] keywordsToIndex) {

    _keywordToFilesMap.Clear();

    try {

      if (!Directory.Exists(targetDirectory)) {
        Console.WriteLine($"\nDirectory not found: {targetDirectory}");
        return;
      }

      if (keywordsToIndex == null || keywordsToIndex.Length == 0) {
        Console.WriteLine("\nNo keywords specified for indexing");
        return;
      }

      var textFilesInDirectory = Directory.GetFiles(targetDirectory, "*.txt");

      if (textFilesInDirectory.Length == 0) {
        Console.WriteLine("\nNo text files in directory");
        return;
      }

      foreach (var keyword in keywordsToIndex) {
        _keywordToFilesMap[keyword] = new List<string>();
      }

      if (_keywordToFilesMap.Count == 0) {
        Console.WriteLine("\nNo valid keywords");
        return;
      }

      foreach (var filePath in textFilesInDirectory) {

        try {
          var fileContent = File.ReadAllText(filePath);

          foreach (var keyword in keywordsToIndex) {
            if (fileContent.ToLower().Contains(keyword.ToLower())) {
              _keywordToFilesMap[keyword].Add(filePath);
            }
          }
        } catch (Exception exception) {
          Console.WriteLine($"\nError reading file {filePath}: {exception.Message}");
        }
      }

      Console.WriteLine("\nIndexing completed");

    } catch (Exception exception) {
      Console.WriteLine($"\nIndexing error: {exception.Message}");
    }
  }

  public void DisplayIndexResults() {

    if (_keywordToFilesMap.Count == 0) {
      Console.WriteLine("\nIndex is empty. Run indexing first!");
      return;
    }

    bool hasResults = false;

    foreach (var keywordEntry in _keywordToFilesMap) {
      if (keywordEntry.Value.Count > 0) {
        hasResults = true;
        Console.WriteLine($"Keyword: {keywordEntry.Key}\n");
        foreach (var filePath in keywordEntry.Value) {
          Console.WriteLine($"  - {filePath}");
        }
      }
    }

    if (!hasResults) {
      Console.WriteLine("\nNo files contain the specified keywords");
    }
  }
}
