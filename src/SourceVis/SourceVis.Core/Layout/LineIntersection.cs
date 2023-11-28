using System.Numerics;

namespace SourceVisCore.Layout;

public static class LineIntersection
{
    private static int Orientation(Vector2 p, Vector2 q, Vector2 r)
    {
        var val = (q.Y - p.Y) * (r.X - q.X) - (q.X - p.X) * (r.Y - q.Y);
        if (val == 0) return 0; // Collinear

        return (val > 0) ? 1 : 2; // Clockwise or Counterclockwise
    }

// Given three collinear points p, q, r, check if point q lies on line segment 'pr'
    private static bool OnSegment(Vector2 p, Vector2 q, Vector2 r) =>
        q.X <= Math.Max(p.X, r.X) &&
        q.X >= Math.Min(p.X, r.X) &&
        q.Y <= Math.Max(p.Y, r.Y) &&
        q.Y >= Math.Min(p.Y, r.Y);

    public static bool Intersects(this ProjectedEdge edge1, ProjectedEdge edge2)
    {
        if (!Skip(edge1, edge2)) return false;

        var p1 = edge1.Start;
        var q1 = edge1.End;
        var p2 = edge2.Start;
        Vector2 q2 = edge2.End;

        // Find the four orientations needed for the general and special cases
        int o1 = Orientation(p1, q1, p2);
        int o2 = Orientation(p1, q1, q2);
        int o3 = Orientation(p2, q2, p1);
        int o4 = Orientation(p2, q2, q1);

        // General case
        if (o1 != o2 && o3 != o4)
            return true;

        // Special Cases
        // p1, q1, and p2 are collinear, and p2 lies on segment p1q1
        if (o1 == 0 && OnSegment(p1, p2, q1)) return true;

        // p1, q1, and q2 are collinear, and q2 lies on segment p1q1
        if (o2 == 0 && OnSegment(p1, q2, q1)) return true;

        // p2, q2, and p1 are collinear, and p1 lies on segment p2q2
        if (o3 == 0 && OnSegment(p2, p1, q2)) return true;

        // p2, q2, and q1 are collinear, and q1 lies on segment p2q2
        if (o4 == 0 && OnSegment(p2, q1, q2)) return true;

        // Doesn't fall in any of the above cases
        return false;
    }

    private static bool Skip(ProjectedEdge edge1, ProjectedEdge edge2)
    {
        if (edge1.Start == edge2.Start && edge1.End == edge2.End)
            return false;
        if (edge1.Start == edge2.End && edge1.End == edge2.Start)
            return false;
        if (edge1.Start == edge1.End || edge2.Start == edge2.End)
            return false;
        return true;
    }
}