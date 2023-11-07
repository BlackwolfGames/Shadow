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

#pragma warning disable CA1822
    public void ShowMessage(string message)
    {
        Console.WriteLine($"ShowMessage: {message}");
    }
#pragma warning restore CA1822
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

#pragma warning disable CS8601 // Possible null reference assignment.
        delegateExample -= myClass.ShowMessage; // Delegate removal
        delegateExample -= MyClass.ShowMessageStatic; // Delegate assignment using named method
#pragma warning restore CS8601 // Possible null reference assignment.

        myClass.MyEvent += delegateExample; // Event assignment
        myClass.MyEvent -= delegateExample; // Event removal

        myClass.RaiseEvent("Hello World!"); // Raising event

#pragma warning disable CS8602 // Dereference of a possibly null reference.
        delegateExample.Invoke("Direct invoke"); // Delegate invocation
#pragma warning restore CS8602 // Dereference of a possibly null reference.

        // Anonymous method
        DelegateExample anonymousDelegateExample = static delegate(string msg)
        {
            Console.WriteLine($"Anonymous: {msg}");
        };

        anonymousDelegateExample += static delegate(string msg) { Console.WriteLine($"Anonymous: {msg}"); };

        anonymousDelegateExample("Hello from anonymous method"); // Delegate invocation using anonymous method

        Console.ReadKey();
    }
}