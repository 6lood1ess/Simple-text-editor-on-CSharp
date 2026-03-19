using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FileSearcher {
  public List<string> SearchByKeywords(string directory, string[] keywords) {

    var result = new List<string>();

    try { 

      if (!Directory.Exists(directory)) {
        Console.WriteLine($"\nDirectory not found: {directory}");
        return result;
      }

      if (keywords == null || keywords.Length == 0) {
        Console.WriteLine("\nNo keywords specified for search");
        return result;
      }

      var files = Directory.GetFiles(directory, "*.txt");

      if (files.Length == 0) {
        Console.WriteLine("\nNo text files in directory");
        return result;
      }

      foreach (var file in files) {

        try {
          var content = File.ReadAllText(file);

          if (keywords.Any(keyword => content.ToLower().Contains(keyword.ToLower()))) {
            result.Add(file);
          }
        } catch (Exception exception) {
          Console.WriteLine($"\nError reading file {file}: {exception.Message}");
        }
      }
    } catch (Exception exception) {
      Console.WriteLine($"\nSearch error: {exception.Message}");
    }

    return result;
  }
}
