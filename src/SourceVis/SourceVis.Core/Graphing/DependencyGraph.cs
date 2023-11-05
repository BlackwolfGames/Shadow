using SourceVisCore.AST;

namespace SourceVisCore.Graphing;

public interface IDependencyGraph
{
    public INode? this[string name] { get; }
    public int Nodes { get; }
    public IEnumerable<IEdge> AllEdges { get; }
}

public interface IEdge
{
    INode lhs { get; }
    INode rhs { get; }
    int this[DependencyType type] { get; }
}

public interface INode
{
    string Name { get; }
    IEdge? this[string name] { get; }
    int Edges { get; }
    IEnumerable<IEdge> AllEdges { get; }
}

public class PreEdge : IEdge
{
    public PreEdge(string lhs, KeyValuePair<string, Dependency> pairValue)
    {
        foreach (var dependencyType in Enum.GetValues<DependencyType>())
        {
            _weights.Add(dependencyType, pairValue.Value[dependencyType]);
        }

        lhsName = lhs;
        rhsName = pairValue.Key;
    }

    public INode lhs => throw new NotSupportedException("resolve edges first");
    public INode rhs => throw new NotSupportedException("resolve edges first");

    public string lhsName { get; }
    public string rhsName { get; }


    public int this[DependencyType type] => _weights[type];
    private readonly Dictionary<DependencyType, int> _weights = new();
}

public class Edge : IEdge
{
    public Edge(PreEdge edge, IDependencyGraph graph)
    {
        foreach (var dependencyType in Enum.GetValues<DependencyType>())
        {
            _weights.Add(dependencyType, edge[dependencyType]);
        }

        lhs = graph[edge.lhsName] ?? throw new NullReferenceException();
        rhs = graph[edge.rhsName] ?? throw new NullReferenceException();
    }

    public INode lhs { get; }
    public INode rhs { get; }

    public int this[DependencyType type] => _weights[type];
    private readonly Dictionary<DependencyType, int> _weights = new();
}

public class Node : INode
{
    public Node(KeyValuePair<string, Class> myClass)
    {
        Name = myClass.Key;
        _edges = myClass.Value.Dependencies.Select(pair => (IEdge) new PreEdge(Name, pair)).ToList();
    }

    public void ResolveEdges(IDependencyGraph parent)
    {
        _edges = _edges.Select(edge => (IEdge) new Edge((PreEdge) edge, parent)).ToList();
    }

    public string Name { get; }

    public IEdge? this[string name] => _edges.Find(edge => edge.lhs.Name.Contains(name) || edge.rhs.Name.Contains(name));
    public int Edges => _edges.Count;
    public IEnumerable<IEdge> AllEdges => _edges;
    private List<IEdge> _edges;
}

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
    }

    public int Nodes => _nodes.Count;
    public IEnumerable<IEdge> AllEdges => _nodes.SelectMany(node => node.AllEdges);
    private readonly List<INode> _nodes;

    public INode? this[string name] => _nodes.Find(node => node.Name.Contains(name));
}