namespace SourceVisCore.Graphing;

public interface IDependencyGraph
{
    public INode? this[string name] { get; }
    public INode? Exact(string name);
    public IEnumerable<INode> AllNodes { get; }
    int Cycles { get; }
    public int Nodes => AllNodes.Count();
    public IEnumerable<IEdge> AllEdges => AllNodes.SelectMany(node => node.AllEdges);
}