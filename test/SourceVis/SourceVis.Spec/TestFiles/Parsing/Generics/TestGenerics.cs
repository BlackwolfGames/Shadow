using System;
namespace SourceVis.Spec.TestFiles.Parsing.Generics;

public interface IGenericInterface<T>
{
    T Prop { get; }
    T Get(T bla);
}

public class TestGenerics : IGenericInterface<string>
{
    public string Prop => "Test";
    public string Get(string bla) => throw new NotSupportedException();
}

public class GenericClass<T>
{
    public T Blah1(T usage) => usage;
    public U Blah2<U>(U usage) => usage;
}

public static class GenericsUsage
{
    public static void Blah()
    {
        GenericClass<int> gc = new();
        TestGenerics tg = new();
        gc.Blah1(4);
        gc.Blah2<double>(4.5f);
        tg.Get("hello");
    }
}