using static System.Console;

namespace HelloWorld;

public static class TestMultipleCalls
{
    public static void Function_1()
    {
        Write("asdf");
        WriteLine("asdf3");
    }
    public static void Func2()
    {
        Clear();
    }
}