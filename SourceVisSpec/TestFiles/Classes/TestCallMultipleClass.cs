using System;

namespace HelloWorld
{
    public static partial class TestCallMultipleClass1
    {
        public static void Run()
        {
            Console.WriteLine("Hello, World!");
        }
    }

    internal struct TestCallMultipleClass2
    {
        public TestCallMultipleClass2()
        {
            Console.WriteLine("Hello, World!");
        }
    }
}