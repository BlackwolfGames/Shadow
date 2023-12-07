namespace SourceVisSpec.TestFiles.Oddities;

[TestTag]
public class TestNestingAndAnnotations
{
  [TestTag]
  public class EmbeddedClass
  {
    public EmbeddedClass(string _)
    {
    }
  }
}

[AttributeUsage(AttributeTargets.All)]
public class TestTagAttribute : Attribute
{
}