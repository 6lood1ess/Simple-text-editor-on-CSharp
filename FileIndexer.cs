using System;
using System.Collections.Generic;
using System.IO;

public class FileIndexer {
 
  Dictionary<string, List<string>> index = new Dictionary<string, List<string>>();

  public void BuildIndex(string targetDirectory, string[] keywordsToIndex) {

    index.Clear();

    try {

      if (!Directory.Exists(targetDirectory)) {
        Console.WriteLine($"\nDirectory not found: {targetDirectory}");
        return;
      }

      if (keywordsToIndex == null || keywordsToIndex.Length == 0) {
        Console.WriteLine("\nNo keywords specified for indexing");
        return;
      }

      foreach (var keyword in keywordsToIndex) {
        index[keyword] = new List<string>();
      }

      List<string> textFilesInDirectory = new List<string>();
      textFilesInDirectory.AddRange(Directory.GetFiles(targetDirectory, "*.txt"));
      textFilesInDirectory.AddRange(Directory.GetFiles(targetDirectory, "*.bin"));
      textFilesInDirectory.AddRange(Directory.GetFiles(targetDirectory, "*.xml"));

      if (textFilesInDirectory.Count == 0) {
        Console.WriteLine("\nNo text, binary, or XML files in directory");
        return;
      }

      if (index.Count == 0) {
        Console.WriteLine("\nNo valid keywords");
        return;
      }

      foreach (string file in textFilesInDirectory) {
         
        string fileContent;
        string lowerFileContent;

        try {
          fileContent = ReadFileContent(file);

          if (!string.IsNullOrEmpty(fileContent)) {
            lowerFileContent = fileContent.ToLower();

            foreach (var keyword in keywordsToIndex) {
              if (lowerFileContent.Contains(keyword.ToLower())) {
                index[keyword].Add(file);
              }
            }
          }
        } catch (Exception exception) {
          Console.WriteLine($"\nError reading file {file}: {exception.Message}");
        }
      }

      Console.WriteLine("\nIndexing completed");

    } catch (Exception exception) {
      Console.WriteLine($"\nIndexing error: {exception.Message}");
    }
  }

  public string ReadFileContent(string filePath) {

    string extension = Path.GetExtension(filePath).ToLower();

    try {

      switch (extension) {
        
        case ".txt":
          using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read)) {
            using (StreamReader reader = new StreamReader(fileStream)) {
              return reader.ReadToEnd();
            }
          }

        case ".xml":
          TextFile xmlFile = TextFile.XmlDeserialize(filePath);
          return xmlFile.Content;

        case ".bin":
          TextFile binaryFile = TextFile.BinaryDeserialize(filePath);
          return binaryFile.Content;

        default:
          return null;
      }
    } catch {
      return null;
    }
  }

  public void DisplayIndexResults() {

    if (index.Count == 0) {
      Console.WriteLine("\nIndex is empty");
      return;
    }

    bool hasResults = false;
    string extension;

    foreach (var keywordEntry in index) {
      if (keywordEntry.Value.Count > 0) {
        hasResults = true;
        Console.WriteLine($"Keyword: {keywordEntry.Key}\n");
        foreach (var file in keywordEntry.Value) {
          extension = Path.GetExtension(file);
          Console.WriteLine($"  - {file} [{extension}]");
        }
      }
    }

    if (!hasResults) {
      Console.WriteLine("\nNo files contain the specified keywords");
    }
  }
}
