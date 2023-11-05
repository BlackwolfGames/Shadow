using SourceVisCore.AST;

namespace SourceVisCore.Graphing;

public class DependencyGraph : IDependencyGraph
{
    public static IDependencyGraph FromProject(Project source)
    {
        return new DependencyGraph(source.Classes.Select(node => new Node(node)));
    }

    private DependencyGraph(IEnumerable<Node> nodes)
    {
        _nodes = nodes.Select(node => (INode) node).ToList();
        foreach (var node in _nodes)
        {
            ((Node) node).ResolveEdges(this);
        }

        var cycles = _nodes.FindCycles();
        foreach (var node in _nodes)
        {
            ((Node) node).IsInCycle = cycles.Any(n => n.Contains(node));
        }
        Cycles = cycles.Count;

    }

    public int Nodes => _nodes.Count;
    public IEnumerable<IEdge> AllEdges => _nodes.SelectMany(node => node.AllEdges);
    public int Cycles { get; }

    private readonly List<INode> _nodes;

    public INode? this[string name] => _nodes.Find(node => node.Name.Contains(name));
}