using System;
namespace SourceVisSpec.TestFiles.Generics;


public interface IGenericInterface<T>
{
    T Prop { get; }
    T Get(T bla);
}


public class TestGenerics : IGenericInterface<string>
{
    public string Prop { get; } = "Tets";
    public string Get(string bla) => throw new NotSupportedException();
}

public class GenericClass <T>
{
    public T blah1(T usage)
    {
        return usage;
    }
    public U blah2<U>(U usage)
    {
        return usage;
    }
}

public class GenericsUsage
{
    public void Blah()
    {
        GenericClass<int> gc = new();
        TestGenerics tg = new();
        gc.blah1(4);
        gc.blah2(4.5);
        tg.Get("hello");
    }
}