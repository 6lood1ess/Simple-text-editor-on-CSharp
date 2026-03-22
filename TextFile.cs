using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

[Serializable]
public class TextFile {

  public string FilePath;
  public string Content;
  public DateTime LastModified;

  public TextFile() { } //for serialization

  public TextFile(string path) {
    FilePath = path;
    Load();
  }

  public void Load() {
    
    try {

      if (!File.Exists(FilePath)) {
        throw new FileNotFoundException($"\nFile not found: {FilePath}");
      }

      using (FileStream fileStream = new FileStream(FilePath, FileMode.Open, FileAccess.Read)) {
        using (StreamReader reader = new StreamReader(fileStream)) {
          Content = reader.ReadToEnd();
        }
      }

      LastModified = File.GetLastWriteTime(FilePath);

    } catch (Exception exception) {
      throw new Exception($"\nError loading file: {exception.Message}");
    }
  }

  public void Save() {

    try { 
      
      using (FileStream fileStream = new FileStream(FilePath, FileMode.Create, FileAccess.Write)) {
        using (StreamWriter writer = new StreamWriter(fileStream)) {
          writer.Write(Content);
        }
      }

      LastModified = DateTime.Now;

    } catch (Exception exception) { 
      throw new Exception($"\nError saving file: {exception.Message}");
    }
  }

  public void BinarySerialize(string path) {

    try {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      using (FileStream fileStream = new FileStream(path, FileMode.Create)) {
        binaryFormatter.Serialize(fileStream, this);
      }
    } catch (Exception exception) { 
      throw new Exception($"\nBinary serialization error: {exception.Message}");
    }
  }

  public static TextFile BinaryDeserialize(string path) {

    try {
      BinaryFormatter binaryFormatter = new BinaryFormatter();
      using (FileStream fileStream = new FileStream(path, FileMode.Open)) {
        return (TextFile)binaryFormatter.Deserialize(fileStream);
      }
    } catch (Exception exception) {
      throw new Exception($"\nBinary deserialization error: {exception.Message}");
    }
  }

  public void XmlSerialize(string path) {

    try {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(TextFile));
      using (FileStream fileStream = new FileStream(path, FileMode.Create)) {
        xmlSerializer.Serialize(fileStream, this);
      }
    } catch (Exception exception) { 
      throw new Exception($"\nXML serialization error: {exception.Message}");
    }
  }

  public static TextFile XmlDeserialize(string path) {

    try {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(TextFile));
      using (FileStream fileStream = new FileStream(path, FileMode.Open)) {
        return (TextFile)xmlSerializer.Deserialize(fileStream);
      }
    } catch (Exception exception) {
      throw new Exception($"\nXML deserialization error: {exception.Message}");
    }
  }
}