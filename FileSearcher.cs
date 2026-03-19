using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FileSearcher {
  public List<string> SearchByKeywords(string directory, string[] keywords) {

    var result = new List<string>();

    try { 

      if (!Directory.Exists(directory)) {
        Console.WriteLine($"Directory not found: {directory}");
        return result;
      }

      if (keywords == null || keywords.Length == 0) {
        Console.WriteLine("No keywords specified for search");
        return result;
      }

      var files = Directory.GetFiles(directory, "*.txt");

      if (files.Length == 0) {
        Console.WriteLine("No text files in directory");
        return result;
      }

      foreach (var file in files) {

        try {
          var content = File.ReadAllText(file);

          if (keywords.Any(keyword => content.ToLower().Contains(keyword.ToLower()))) {
            result.Add(file);
          }
        } catch (Exception exception) {
          Console.WriteLine($"Error reading file {file}: {exception.Message}");
        }
      }
    } catch (Exception exception) {
      Console.WriteLine($"Search error: {exception.Message}");
    }

    return result;
  }
}
