using System;
namespace SourceVisSpec.TestFiles.Exceptions;

public class NegativeAgeException : Exception {
    public NegativeAgeException(string message) : base(message) { }
}
public class TestExceptions {
    public TestExceptions(int age) {
        if (age < 0) {
            throw new NegativeAgeException("Age cannot be negative");
        }
    }

    /// <exception cref="ArgumentException">Thrown when age is negative.</exception>
    public void ValidateAge(int age) {
        if (age < 0) {
            throw new ArgumentException("Age cannot be negative");
        }
    }
    public void ValidateAndHandle(int age) {
        try {
            ValidateAge(age);
        }
        catch (Exception ex) {
            throw; // re-throw the caught exception
        }
    }
    public void HandleDivideByZero() {
        try
        {
            var result = 10f / 0f;
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