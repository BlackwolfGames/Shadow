namespace SourceVisCore.Layout.Forces;

public class PullingForce : ForceStrength
{
  public override Forces ForceType => Forces.Pulling;

  public override void ApplyTo(GraphProjection get)
  {
    foreach (ProjectedNode node in get.Nodes)
    {
      foreach (ProjectedEdge node2 in get.Edges.Where(edge => edge.Start == node.Position))
        node.Velocity -= (node.Position - node2.End) / 100 * Strength;
    }
  }
}