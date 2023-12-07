using SourceVisCore.AST;

namespace SourceVisCore.Graphing;

public interface IEdge
{
  INode from { get; }
  INode to { get; }
  int LeavesNamespaces { get; }
  int EntersNamespaces { get; }
  int SharedNamespaces { get; }
  int CrossesNamespaces => LeavesNamespaces + EntersNamespaces;
  int this[DependencyType type] { get; }
}