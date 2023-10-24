﻿namespace SourceVisCore.AST;

public class Class
{
    private readonly Dictionary<string, Dependency> _deps = new();
    
    public IEnumerable<KeyValuePair<string, Dependency>> Dependencies => _deps;

    public void AddDependency(DependencyType type, string name)
    {
        _deps.TryAdd(name, new Dependency());
        _deps[name].Add(type);
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