using System;

namespace SourceVisSpec.TestFiles.Exceptions;

public class NegativeAgeException : Exception
{
    public NegativeAgeException(string message) : base(message)
    {
    }
}

public class TestExceptions
{
    protected TestExceptions(int age)
    {
        if (age < 0)
        {
            throw new NegativeAgeException("Age cannot be negative");
        }
    }

    /// <exception cref="ArgumentException">Thrown when age is negative.</exception>
    private static void ValidateAge(int age)
    {
        if (age < 0)
        {
            throw new ArgumentException("Age cannot be negative");
        }
    }

    public static void ValidateAndHandle(int age)
    {
        try
        {
            ValidateAge(age);
        }
        catch (Exception)
        {
            throw; // re-throw the caught exception
        }
    }

    public static void HandleDivideByZero()
    {
        try
        {
            _ = 10f / 0f;
        }
        catch (DivideByZeroException)
        {
            // named silencing
        }
        catch
        {
            //implicit silencing
        }
    }
}