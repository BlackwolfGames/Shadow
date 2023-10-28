using System;
namespace Injection;

public struct DependsOnStruct
{
}

public interface IDependsOnInterface
{
}

public class DependsOnClass : IDependsOnInterface
{
}

public class TestConstructorInjection
{
    public TestConstructorInjection(DependsOnStruct dep1,
        DependsOnClass dep2,
        IDependsOnInterface dep3)
    {
    }
}

public static partial class TestFunctionInject
{
    public static void Test(DependsOnStruct dep1,
        DependsOnClass dep2,
        IDependsOnInterface dep3)
    {
        throw new NotSupportedException();
    }
}

public static class TestFunctionReturn
{
    public static DependsOnStruct Test1() => new();
    public static DependsOnClass Test2() => new();
    public static IDependsOnInterface Test3() => new DependsOnClass();

    public static DependsOnStruct Dep1 => new();
    public static DependsOnClass Dep2 => new();
    public static IDependsOnInterface Dep3 => new DependsOnClass();
}

public class TestVariableDeclarations
{
    public DependsOnStruct Dep1 = new();
    public DependsOnClass Dep2 = new();
    public IDependsOnInterface Dep3 = new DependsOnClass();

    public static void Func()
    {
        DependsOnStruct dep4;
        DependsOnClass dep5;
        IDependsOnInterface dep6;
    }
}