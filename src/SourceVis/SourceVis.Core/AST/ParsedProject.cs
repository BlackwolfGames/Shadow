namespace SourceVisCore.AST;

public class Project
{
  private readonly Dictionary<string, Class> _classes = new();
  public IEnumerable<KeyValuePair<string, Class>> Classes => _classes;

  public Class AddClass(string name)
  {
    _classes.TryAdd(name, new Class());
    return _classes[name];
  }

  public void Print()
  {
    foreach (var parsedClass in _classes)
    {
      Console.WriteLine($" -- Class: {parsedClass.Key}");
      parsedClass.Value.Print();
    }
  }
}