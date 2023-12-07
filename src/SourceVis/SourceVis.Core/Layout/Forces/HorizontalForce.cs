using System.Numerics;

namespace SourceVisCore.Layout.Forces;

public class HorizontalForce : ForceStrength
{
  public override Forces ForceType => Forces.Horizontal;

  public override void ApplyTo(GraphProjection get)
  {
    foreach (ProjectedNode node in get.Nodes) node.Velocity -= node.Position * new Vector2(0, 1) / 1000 * Strength;
  }
}