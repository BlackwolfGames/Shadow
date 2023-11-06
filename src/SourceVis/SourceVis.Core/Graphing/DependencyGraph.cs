using System.Reflection;
using SourceVisCore.AST;

namespace SourceVisCore.Graphing;

public class DependencyGraph : IDependencyGraph
{
    public static IDependencyGraph FromProject(Project source)
    {
        return new DependencyGraph(
            source.Classes.Select(node => new Node(node))
                .Union(source.Classes.SelectMany(node => node.Value.Dependencies).Select(pair => new Node(pair)))
                .GroupBy(node => node.Name)
                .Select(nodes => nodes.OrderByDescending(node => node.Edges).First()));
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

    public INode? this[string name]
    {
        get
        {
            try
            {
                return _nodes.SingleOrDefault(node => node.Name.EndsWith(name)) ??
                       throw new KeyNotFoundException(
                           $"No node found with name {name}");
            }
            catch
            {
                throw new AmbiguousMatchException(
                    $"multiple nodes found ending with {name}, consider prefixing with '.'");
            }
        }
    }

    public INode? Exact(string name)
    {
        return _nodes.Find(node => node.Name.Equals(name)) ??
               _nodes.Find(node => node.Name.Equals('.' + name));
    }
}