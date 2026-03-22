using System;

[Serializable]
public class Memento {

  public string Content;
  public DateTime Timestamp;

  public Memento(string content) {
    Content = content;
    Timestamp = DateTime.Now;
  }
}

