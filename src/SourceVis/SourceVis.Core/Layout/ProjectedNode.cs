using System.Numerics;
using SourceVisCore.Graphing;

namespace SourceVisCore.Layout;

public class ProjectedNode
{
    public ProjectedNode(INode held) => Held = held;

    public INode Held { get; }
    public Vector2 Position { get; set; }

    public void Print(TextWriter stream)
    {
        stream.WriteLine("Name: "+Held.Name.Split('.')[^1]);
        stream.WriteLine("Position: "+Position);
        foreach (var edge in Held.AllEdges)
        {
            stream.WriteLine($"Edge: {edge.to.Name.Split('.')[^1]}");
        }
        stream.WriteLine();
    }
}