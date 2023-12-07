using System.Reflection;

namespace SourceVisCore.Layout.Forces;

public enum Forces
{
  Centering,
  Damping,
  Spacing,
  Pulling,
  Horizontal
}

public class ForceMapping
{
  private IForce[] Forces { get; } = Assembly.GetExecutingAssembly().GetTypes().Where(p => typeof(IForce).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract).Select(Activator.CreateInstance).Cast<IForce>().ToArray();

  public static ForceMapping Get { get; } = new();
  public IForce this[Forces forceType] => Forces.Single(force => force.ForceType == forceType);
}