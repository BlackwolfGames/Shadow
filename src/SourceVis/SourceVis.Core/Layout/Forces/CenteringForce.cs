namespace SourceVisCore.Layout.Forces;

public class CenteringForce : ForceStrength
{
  public override Forces ForceType => Forces.Centering;

  public override void ApplyTo(GraphProjection get)
  {
    foreach (ProjectedNode node in get.Nodes) node.Velocity += -node.Position / 100 * Strength;
  }
}