namespace Instantiation;

public class InstantiatesSelf
{
    public InstantiatesSelf()
    {
    }

    public void Test()
    {
        var t = new InstantiatesSelf();
    }
}

public static partial class InstantiatesOther
{
    public static void Test()
    {
        var t = new InstantiatesSelf();
    }
}