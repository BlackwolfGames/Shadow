using SourceVisCore.AST;

namespace SourceVisCore.Graphing;

public class Node : INode
{
  public Node(KeyValuePair<string, Class> myClass)
  {
    Name = myClass.Key.Replace("global::", "");
    _edges = myClass.Value.Dependencies.Select(pair => (IEdge)new PreEdge(Name, pair)).ToList();
  }

  public Node(KeyValuePair<string, Dependency> myClass)
  {
    Name = myClass.Key;
  }

  public void ResolveEdges(IDependencyGraph parent)
  {
    _edges = _edges.Select(edge => (IEdge)new Edge((PreEdge)edge, parent)).ToList();
  }

  public string Name { get; }

  public IEdge? this[string name] => _edges.Find(edge => edge.from.Name.Contains(name) || edge.to.Name.Contains(name));

  public int Edges => _edges.Count;
  public IEnumerable<IEdge> AllEdges => _edges;
  public bool IsInCycle { get; set; }
  public string[] Namespaces() => Name.Split('.').SkipLast(1).ToArray();

  public NodeType NodeType
  {
    get
    {
      var ns = Namespaces();
      if (ns.Length == 0)
        return NodeType.generic;

      if (ns.Contains("System"))
        return NodeType.builtin;

      return NodeType.normal;
    }
  }

  private List<IEdge> _edges = new();
}