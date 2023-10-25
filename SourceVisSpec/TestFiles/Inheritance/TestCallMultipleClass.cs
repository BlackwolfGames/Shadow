using System;
namespace HelloWorld
{
   partial class TestCallMultipleClass1
    {
        protected TestCallMultipleClass1()
        {
            
        }
        static void Run(object? o, EventArgs a)
        {
            Console.WriteLine("Hello, World!");
        }
    }
    struct TestCallMultipleClass2
    {
        public TestCallMultipleClass2()
        {
            Console.WriteLine("Hello, World!");
        }
    }
}