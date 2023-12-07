using SourceVisCore.Layout.Forces;

namespace SourceVisCore.Layout;

public class LayoutOptimizer
{
  public List<IForce> Forces { get; } = new();

  public void Optimize(GraphProjection get)
  {
    Step++;
    foreach (IForce f in Forces) f.ApplyTo(get);

    foreach (ProjectedNode projectedNode in get.Nodes) projectedNode.Position += projectedNode.Velocity;
  }

  public int Step { get; private set; }

  public void AddForce(Forces.Forces forceType, float strength = 1)
  {
    IForce force = ForceMapping.Get[forceType];
    force.SetStrength(strength);
    Forces.Add(force);
  }

  public void RemoveForce(Forces.Forces forceType)
  {
    Forces.Remove(ForceMapping.Get[forceType]);
  }
}