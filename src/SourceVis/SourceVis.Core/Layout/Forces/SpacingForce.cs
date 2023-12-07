using System.Numerics;

namespace SourceVisCore.Layout.Forces;

public class SpacingForce : ForceStrength
{
  public override Forces ForceType => Forces.Spacing;

  public override void ApplyTo(GraphProjection get)
  {
    foreach (ProjectedNode node in get.Nodes)
    {
      foreach (ProjectedNode node2 in get.Nodes.Where(projectedNode => projectedNode != node))
      {
        Vector2 delta = node.Position - node2.Position;
        node.Velocity += delta / delta.Length() * Strength;
      }
    }
  }
}