﻿namespace HelloWorld;

public static class TestCallInLambda
{
  public static void RunLambda()
  {
    Action temp = () => { Console.WriteLine("Test"); };
    temp();
  }

  public static void RunLocalStaticFunction()
  {
    Temp();

    static void Temp()
    {
      Console.WriteLine("Test");
    }
  }

  public static void RunLocalInstanceFunction()
  {
    Temp();

    void Temp()
    {
      Console.WriteLine("Test");
    }
  }
}