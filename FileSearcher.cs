using System.Collections.Generic;
using System.IO;
using System.Linq;

public class FileSearcher {
  public List<string> SearchByKeywords(string directory, string[] keywords) {

    var result = new List<string>();
    var files = Directory.GetFiles(directory, "*.txt");

    foreach (var file in files) {

      var content = File.ReadAllText(file);

      if (keywords.Any(keyword => content.Contains(keyword, StringComparison.OrdinalIgnoreCase))) {
        result.Add(file);
      }
    }

    return result;
  }
}
