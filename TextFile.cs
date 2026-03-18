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

  public void Load() => Content = File.ReadAllText(FilePath);
  public void Save() => File.WriteAllText(FilePath, Content);

  public void BinarySerialize(string path) {

    using (var stream = new FileStream(path, FileMode.Create)) {
      using (var writer = new BinaryWriter(stream)) {
        writer.Write(FilePath);
        writer.Write(Content);
        writer.Write(LastModified.Ticks);
      }
    }
  }

  public static TextFile BinaryDeserialize(string path) {

    using (var stream = new FileStream(path, FileMode.Open)) {
      using (var reader = new BinaryReader(stream)) {

        return new TextFile {
          FilePath = reader.ReadString(),
          Content = reader.ReadString(),
          LastModified = new DateTime(reader.ReadInt64())
        };
      }
    }
  }

  public void XmlSerialize(string path) {

    var xml = new XmlSerializer(typeof(TextFile));

    using (var stream = new FileStream(path, FileMode.Create)) {
      xml.Serialize(stream, this);
    }
  }

  public static TextFile XmlDeserialize(string path) {

    var xml = new XmlSerializer(typeof(TextFile));

    using (var stream = new FileStream(path, FileMode.Open)) {
      return (TextFile)xml.Deserialize(stream);
    }
  }
}