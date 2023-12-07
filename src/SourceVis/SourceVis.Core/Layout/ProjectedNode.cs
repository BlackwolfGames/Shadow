using System.Numerics;
using SourceVisCore.Graphing;

namespace SourceVisCore.Layout;

public class ProjectedNode
{
  public ProjectedNode(INode held)
  {
    Held = held;
  }

  public INode Held { get; }
  public Vector2 Position { get; set; }
  public Vector2 Velocity { get; set; }

  public void Print(TextWriter stream)
  {
    stream.WriteLine("Name: " + Held.Name.Split('.')[^1]);
    stream.WriteLine($"Position: [{Math.Round(Position.X, 2)}, {Math.Round(Position.Y, 2)}]");
    stream.WriteLine($"Velocity: [{Math.Round(Velocity.X, 2)}, {Math.Round(Velocity.Y, 2)}]");
    foreach (IEdge edge in Held.AllEdges) stream.WriteLine($"Edge: {edge.to.Name.Split('.')[^1]}");
    stream.WriteLine();
  }
}