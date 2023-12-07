namespace SourceVis.Spec.TestFiles.Parsing.Instantiation;

public sealed class InstantiatesSelf
{
  public void Test()
  {
    _t = new InstantiatesSelf();
    _t.DoStuff();
  }

  private void DoStuff()
  {
    //test method
  }
  private InstantiatesSelf _t = new();
}

public static class InstantiatesOther
{
  public static void Test()
  {
    _ = new InstantiatesSelf();
  }
}