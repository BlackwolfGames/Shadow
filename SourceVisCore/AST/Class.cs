namespace SourceVisCore.AST;

public class Class
{
    private readonly Dictionary<string, int> _deps = new();

    public void AddDependency(string name)
    {
        _deps.TryAdd(name, 0);
        _deps[name]++;
    }

    public void Print()
    {
        foreach (var dependency in _deps)
        {
            Console.WriteLine($" -- -- Depends on:" +
                $" {dependency.Key} - {dependency.Value} times");
        }
    }
}