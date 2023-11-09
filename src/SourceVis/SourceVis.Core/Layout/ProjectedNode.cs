using System.Numerics;
using SourceVisCore.Graphing;

namespace SourceVisCore.Layout;

public class ProjectedNode
{
    public ProjectedNode(INode held) => Held = held;

    public INode Held { get; }
    public Vector2 Position { get; set; }
}