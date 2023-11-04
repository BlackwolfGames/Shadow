using System;
namespace SourceVisSpec.TestFiles.Delegates;

public delegate void DelegateExample(string message); // Delegate declaration

public class MyClass
{
    public event DelegateExample? MyEvent; // Event declaration

    public void RaiseEvent(string message)
    {
        MyEvent?.Invoke(message);
    }

    public void ShowMessage(string message)
    {
        Console.WriteLine($"ShowMessage: {message}");
    }
    public static void ShowMessageStatic(string message)
    {
        Console.WriteLine($"ShowMessage: {message}");
    }
}

public static class DelegateTest
{
    public static void Test()
    {
        var myClass = new MyClass();
        DelegateExample delegateExample = myClass.ShowMessage; // Delegate assignment using named method
        delegateExample += MyClass.ShowMessageStatic; // Delegate assignment using named method

        delegateExample += msg => { Console.WriteLine($"Lambda: {msg}"); }; // Delegate assignment using lambda
        delegateExample += static msg => { Console.WriteLine($"Lambda: {msg}"); }; // Delegate assignment using lambda
            
        delegateExample -= myClass.ShowMessage; // Delegate removal
        delegateExample -= MyClass.ShowMessageStatic; // Delegate assignment using named method

        myClass.MyEvent += delegateExample; // Event assignment
        myClass.MyEvent -= delegateExample; // Event removal

        myClass.RaiseEvent("Hello World!"); // Raising event

        delegateExample("Direct invoke"); // Delegate invocation

        // Anonymous method
        DelegateExample anonymousDelegateExample = delegate(string msg)
        {
            Console.WriteLine($"Anonymous: {msg}");
        };
            
        anonymousDelegateExample += static delegate(string msg)
        {
            Console.WriteLine($"Anonymous: {msg}");
        };

        anonymousDelegateExample("Hello from anonymous method"); // Delegate invocation using anonymous method

        Console.ReadKey();
    }
}