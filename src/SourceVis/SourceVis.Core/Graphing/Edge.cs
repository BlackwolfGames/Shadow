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

        from = graph.Exact(edge.LhsName) ?? throw new NullReferenceException($"There is no node named {edge.LhsName} in the dependency graph");
        to = graph.Exact(edge.RhsName) ?? throw new NullReferenceException($"There is no node named {edge.RhsName} in the dependency graph");
    }

    public INode from { get; }
    public INode to { get; }

    public int this[DependencyType type] => _weights[type];
    private readonly Dictionary<DependencyType, int> _weights = new();
}