using System;

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

public class TestTagAttribute : Attribute
{
}