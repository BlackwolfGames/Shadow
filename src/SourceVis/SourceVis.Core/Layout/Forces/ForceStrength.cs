namespace SourceVisCore.Layout.Forces;

public abstract class ForceStrength : IForce
{
  protected float Strength { get; private set; }

  public abstract Forces ForceType { get; }

  public abstract void ApplyTo(GraphProjection get);

  public void SetStrength(float strength)
  {
    Strength = strength;
  }
}