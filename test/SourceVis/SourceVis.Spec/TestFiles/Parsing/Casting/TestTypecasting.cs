namespace SourceVis.Spec.TestFiles.Parsing.Casting;

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

public static class TestTypecasting
{
    public static void TypecastTest()
    {
        var type = new TypeA();
        var tb = (TypeB) type;
        TypeB tb2 = type;
        var goBack = (TypeA) tb;
        var goBack2 = tb as TypeC;
    }
}