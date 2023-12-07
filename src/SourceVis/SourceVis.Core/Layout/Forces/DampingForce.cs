namespace SourceVisCore.Layout.Forces;

public class DampingForce : ForceStrength
{
  public override Forces ForceType => Forces.Damping;

  public override void ApplyTo(GraphProjection get)
  {
    foreach (ProjectedNode node in get.Nodes)
    {
      var velocityLength = node.Velocity.Length();

      // Avoid division by zero in case of very small velocities
      if (velocityLength <= 0) continue;

      // Apply damping factor. Higher velocity results in stronger damping
      var dampingFactor = 1 + velocityLength * Strength;
      node.Velocity /= dampingFactor;
    }
  }
}