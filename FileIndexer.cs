using System;
using System.Collections.Generic;
using System.IO;

public class FileIndexer {
 
  private Dictionary<string, List<string>> _keywordToFilesMap = new Dictionary<string, List<string>>();

  public void BuildIndexForDirectory(string targetDirectory, string[] keywordsToIndex) {

    _keywordToFilesMap.Clear();

    var textFilesInDirectory = Directory.GetFiles(targetDirectory, "*.txt");

    foreach (var keyword in keywordsToIndex) {
      _keywordToFilesMap[keyword] = new List<string>();
    }

    foreach (var filePath in textFilesInDirectory) {

      var fileContent = File.ReadAllText(filePath);

      foreach (var keyword in keywordsToIndex) {
        if (fileContent.Contains(keyword, StringComparison.OrdinalIgnoreCase)) {
          _keywordToFilesMap[keyword].Add(filePath);
        }
      }
    }
  }

  public void DisplayIndexResults() {

    foreach (var keywordEntry in _keywordToFilesMap) {
      Console.WriteLine($"Keyword: {keywordEntry.Key}");
      foreach (var filePath in keywordEntry.Value) {
        Console.WriteLine($"  - {filePath}");
      }
    }
  }
}
