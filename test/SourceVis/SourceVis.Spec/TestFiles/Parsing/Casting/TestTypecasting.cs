namespace SourceVisSpec.TestFiles.Casting;

public class TypeA
{
    public static implicit operator TypeB(TypeA from) => new();
    public static explicit operator TypeA(TypeB from) => new();
}

public partial class TypeB
{
}

public partial class TypeC : TypeB
{
}

public class TestTypecasting
{
    public void TypecastTest()
    {
        var type = new TypeA();
        var tb = (TypeB) type;
        TypeB tb2 = type;
        var goBack = (TypeA) tb;
        var goBack2 = tb as TypeC;
    }
}