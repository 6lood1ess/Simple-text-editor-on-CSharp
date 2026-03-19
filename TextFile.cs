using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

[Serializable]
public class TextFile {

  public string FilePath { get; set; }
  public string Content { get; set; }
  public DateTime LastModified { get; set; }

  public TextFile() { } //для сериализации

  public TextFile(string path) {
    FilePath = path;
    Load();
  }

  public void Load() {
    
    try {
      if (!File.Exists(FilePath)) {
        throw new FileNotFoundException($"File not found: {FilePath}");
      }

      Content = File.ReadAllText(FilePath);
      LastModified = File.GetLastWriteTime(FilePath);

    } catch (Exception exception) {
      throw new Exception($"Error loading file: {exception.Message}");
    }
  }

  public void Save() {

    try { 
      File.WriteAllText(FilePath, Content);
      LastModified = DateTime.Now;

    } catch (Exception exception) { 
      throw new Exception($"Error saving file: {exception.Message}");
    }
  }

  public void BinarySerialize(string path) {

    try {
      using (var stream = new FileStream(path, FileMode.Create)) {
        using (var writer = new BinaryWriter(stream)) {
          writer.Write(FilePath);
          writer.Write(Content);
          writer.Write(LastModified.Ticks);
        }
      }
    } catch (Exception exception) { 
      throw new Exception($"Binary serialization error: {exception.Message}");
    }
  }

  public static TextFile BinaryDeserialize(string path) {

    try { 
      using (var stream = new FileStream(path, FileMode.Open)) {
        using (var reader = new BinaryReader(stream)) {

          return new TextFile {
            FilePath = reader.ReadString(),
            Content = reader.ReadString(),
            LastModified = new DateTime(reader.ReadInt64())
          };
        }
      }
    } catch (Exception exception) {
      throw new Exception($"Binary deserialization error: {exception.Message}");
    }
  }

  public void XmlSerialize(string path) {

    try {
      var xmlSerializer = new XmlSerializer(typeof(TextFile));

      using (var stream = new FileStream(path, FileMode.Create)) {
        xmlSerializer.Serialize(stream, this);
      }
    } catch (Exception exception) { 
      throw new Exception($"XML serialization error: {exception.Message}");
    }
  }

  public static TextFile XmlDeserialize(string path) {

    try {
      var xmlSerializer = new XmlSerializer(typeof(TextFile));

      using (var stream = new FileStream(path, FileMode.Open)) {
        return (TextFile)xmlSerializer.Deserialize(stream);
      }
    } catch (Exception exception) {
      throw new Exception($"XML deserialization error: {exception.Message}");
    }
  }
}