using static System.Console;

namespace HelloWorld;

public class TestMultipleCalls
{
    public void Function_1(object? o, EventArgs a)
    {
        Write("asdf");
        WriteLine("asdf3");
    }
    public void Func2(object? o, EventArgs a)
    {
        Clear();
    }
}