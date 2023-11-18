using System.Reflection;

namespace SourceVisCore.Layout.Forces;

public enum Forces
{
    Centering
}

public class ForceMapping
{
    private IForce[] forces { get; } = Assembly.GetExecutingAssembly().GetTypes()
        .Where(p => typeof(IForce).IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract)
        .Select(Activator.CreateInstance)
        .Cast<IForce>().ToArray();

    public static ForceMapping Get { get; } = new();
    public IForce this[Forces forceType] => forces.Single(force => force.ForceType == forceType);
}