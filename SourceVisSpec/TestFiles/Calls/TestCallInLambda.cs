using System;
namespace HelloWorld;

public class TestCallInLambda
{
    public void RunLambda()
    {
        var temp = () => { Console.WriteLine("Test"); };
        temp();
    }

    public void RunLocalStaticFunction()
    {
        Temp();

        static void Temp()
        {
            Console.WriteLine("Test");
        }
    }

    public void RunLocalInstanceFunction()
    {
        Temp();

        void Temp()
        {
            Console.WriteLine("Test");
        }
    }
}