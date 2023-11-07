using System;
namespace Injection;

public struct DependsOnStruct {
    public int I;
}

public interface IDependsOnInterface {
    DependsOnStruct Name();
}

public class DependsOnClass : IDependsOnInterface {
    public DependsOnStruct Name() => throw new NotSupportedException();
}
public class TestConstructorInjection {
    public TestConstructorInjection(DependsOnStruct dep1,
        DependsOnClass dep2, IDependsOnInterface dep3) { }
}
public static partial class TestFunctionInject {
    public static void Test(DependsOnStruct dep1,
        DependsOnClass dep2, IDependsOnInterface dep3) {
        throw new NotSupportedException();
    } }
public static class TestFunctionReturn {
    public static DependsOnStruct Test1() => new();
    public static DependsOnClass Test2() => new();
    public static IDependsOnInterface Test3() => new DependsOnClass();
    public static DependsOnStruct Dep1 => new();
    public static DependsOnClass Dep2 => new();
    public static IDependsOnInterface Dep3 => new DependsOnClass();
}
public class TestVariableDeclarations {
    public DependsOnStruct Dep1 = new();
    public DependsOnClass Dep2 = new();
    public IDependsOnInterface Dep3 = new DependsOnClass();
    public static void Func() {
#pragma warning disable CS0168 // Variable is declared but never used
        DependsOnStruct dep4;
        DependsOnClass dep5;
        IDependsOnInterface dep6;
#pragma warning restore CS0168 // Variable is declared but never used
    }
}