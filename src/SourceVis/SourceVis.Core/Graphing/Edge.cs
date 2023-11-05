using SourceVisCore.AST;

namespace SourceVisCore.Graphing;

public class Edge : IEdge
{
    public Edge(PreEdge edge, IDependencyGraph graph)
    {
        foreach (var dependencyType in Enum.GetValues<DependencyType>())
        {
            _weights.Add(dependencyType, edge[dependencyType]);
        }

        from = graph[edge.lhsName] ?? throw new NullReferenceException();
        to = graph[edge.rhsName] ?? throw new NullReferenceException();
    }

    public INode from { get; }
    public INode to { get; }

    public int this[DependencyType type] => _weights[type];
    private readonly Dictionary<DependencyType, int> _weights = new();
}