namespace Inheritance;

public interface ITestInterface
{
}

public class TestInheritance : ITestInterface
{
}

public abstract class TestAbstractBase : ITestInterface
{
}

public class TestDerived : TestAbstractBase
{
}