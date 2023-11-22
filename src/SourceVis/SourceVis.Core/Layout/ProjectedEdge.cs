using System.Numerics;

namespace SourceVisCore.Layout;

public struct ProjectedEdge
{
    public Vector2 Start { get; }
    public Vector2 End { get; }

    public ProjectedEdge(ProjectedNode start, ProjectedNode end)
    {
        Start = start.Position;
        End = end.Position;
    }
}