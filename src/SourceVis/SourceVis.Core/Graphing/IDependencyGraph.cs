namespace SourceVisCore.Graphing;

public interface IDependencyGraph
{
    public INode? this[string name] { get; }
    public INode? Exact(string name);
    public int Nodes { get; }
    public IEnumerable<IEdge> AllEdges { get; }
    int Cycles { get; }
}