namespace SourceVisCore.Graphing;

public interface INode
{
    string Name { get; }
    IEdge? this[string name] { get; }
    int Edges { get; }
    IEnumerable<IEdge> AllEdges { get; }
    bool IsInCycle { get; }
    string[] Namespaces();
    NodeType NodeType { get; }
}