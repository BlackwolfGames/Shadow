namespace SourceVis.Spec.TestFiles.Parsing.Instantiation;

public class InstantiatesSelf
{
    public void Test()
    {
        _t = new InstantiatesSelf();
        _t.doStuff();
    }

#pragma warning disable CA1822
    private void doStuff()
    {
        //test method
    }
#pragma warning restore CA1822
    private InstantiatesSelf _t = new();
}

public static partial class InstantiatesOther
{
    public static void Test()
    {
        var t = new InstantiatesSelf();
    }
}