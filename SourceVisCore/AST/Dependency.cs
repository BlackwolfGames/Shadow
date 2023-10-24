namespace SourceVisCore.AST;

public class Dependency
{
    private readonly Dictionary<DependencyType, int> _types = new();

    public void Add(DependencyType type)
    {
        _types.TryAdd(type, 0);
        _types[type]++;
    }

    public int this[DependencyType t] => _types[t];
}

public enum DependencyType
{
    Invalid,
    DirectInstantiation,
    ConstructorInjection,
    MethodInjection,
    FieldInjection,
    Typecast,
    SafeCast,
    Extension,
    Implementation,
    InstanceInvocation,
    StaticInvocation,
    Nesting,
    SubscribesToEvent,
    GenericDependency,
    Annotation,
    Special,
}