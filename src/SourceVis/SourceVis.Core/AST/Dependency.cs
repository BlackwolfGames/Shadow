namespace SourceVisCore.AST;

public class Dependency
{
    private readonly Dictionary<DependencyType, int> _types = new();

    public Dependency()
    {
        foreach (var dependencyType in Enum.GetValues<DependencyType>())
        {
            _types.TryAdd(dependencyType, 0);
        }
    }

    public void Add(DependencyType type)
    {
        _types[type]++;
    }

    public int this[DependencyType t] => _types[t];

    public int Total => _types.Values.Sum();
}

public enum DependencyType
{
    Invalid,
    DirectInstantiation,
    ParameterInjection,
    VariableDeclaration,
    ReturnType,
    ThrownException,
    CaughtException,
    Typecast,
    SafeCast,
    ImplicitConversion,
    Extension,
    Implementation,
    InstanceInvocation,
    StaticInvocation,
    Nesting,
    EventDeclaration,
    DelegateDeclaration,
    DelegateInvocation,
    SubscribesToDelegate,
    UnsubscribesFromDelegate,
    SubscribesToEvent,
    UnsubscribesFromEvent,
    Property,
    GenericMethod,
    GenericClass,
    Attribute,
}