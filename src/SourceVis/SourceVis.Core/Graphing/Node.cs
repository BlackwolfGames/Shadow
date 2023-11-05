using SourceVisCore.AST;

namespace SourceVisCore.Graphing;

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

    public IEdge? this[string name] =>
        _edges.Find(edge => edge.from.Name.Contains(name) || edge.to.Name.Contains(name));

    public int Edges => _edges.Count;
    public IEnumerable<IEdge> AllEdges => _edges;
    public bool IsInCycle { get; set; }
    private List<IEdge> _edges;
}