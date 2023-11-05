using SourceVisCore.AST;

namespace SourceVisCore.Graphing;

public interface IEdge
{
    INode from { get; }
    INode to { get; }
    int this[DependencyType type] { get; }
}