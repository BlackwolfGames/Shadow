using SourceVisCore.AST;

namespace SourceVisCore.Graphing;

public class Edge : IEdge
{
  public Edge(PreEdge edge, IDependencyGraph graph)
  {
    foreach (DependencyType dependencyType in Enum.GetValues<DependencyType>()) _weights.Add(dependencyType, edge[dependencyType]);

    from = graph.Exact(edge.LhsName) ?? throw new KeyNotFoundException($"There is no node named {edge.LhsName} in the dependency graph");
    to = graph.Exact(edge.RhsName) ?? throw new KeyNotFoundException($"There is no node named {edge.RhsName} in the dependency graph");
  }

  public INode from { get; }
  public INode to { get; }
  public int LeavesNamespaces => from.Namespaces().Skip(SharedNamespaces).Except(to.Namespaces()).Count();
  public int EntersNamespaces => to.Namespaces().Skip(SharedNamespaces).Except(from.Namespaces()).Count();

  public int SharedNamespaces => from.Namespaces().Zip(to.Namespaces()).TakeWhile((tuple, _) => tuple.First == tuple.Second).Count();

  public int this[DependencyType type] => _weights[type];
  private readonly Dictionary<DependencyType, int> _weights = new();
}