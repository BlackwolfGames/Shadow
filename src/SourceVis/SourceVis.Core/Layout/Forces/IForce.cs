namespace SourceVisCore.Layout.Forces;

public interface IForce
{
  public Forces ForceType { get; }
  void ApplyTo(GraphProjection get);
  void SetStrength(float strength);
}