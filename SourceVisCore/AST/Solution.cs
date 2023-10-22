namespace SourceVisCore.AST;

public class Solution
{
    private readonly Dictionary<string, Project> _projects = new(); 

    public Project AddProject(string name)
    {
        _projects.TryAdd(name, new Project());
        return _projects[name];
    }

    public void Print()
    {
        foreach (var project in _projects)
        {
            Console.WriteLine($"Project: {project.Key}");
            project.Value.Print();
        }
    }
}