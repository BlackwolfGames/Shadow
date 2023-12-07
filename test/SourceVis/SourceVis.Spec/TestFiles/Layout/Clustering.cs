// Base classes forming one cluster

namespace SourceVis.Spec.TestFiles.Layout;

file interface IBaseClassA
{
}

file interface IBaseClassB
{
}

// Cluster 1 - Inheriting from BaseClassA
public class ClassA1 : IBaseClassA
{
  public ClassB2? ClassB2;
}

public class ClassA2 : IBaseClassA
{
  public ClassB1? ClassB1;
}

// Cluster 2 - Inheriting from BaseClassB
public class ClassB1 : IBaseClassB
{
  public ClassA1? ClassA1;
}

public class ClassB2 : IBaseClassB
{
  public ClassA2? ClassA2;
}

// Cluster 3 - Independent cluster
public class IndependentClass1
{
  public IndependentClass2? IndependentClass2;
}

public class IndependentClass2
{
  public IndependentClass1? IndependentClass1;
}

// Utility class used across clusters
public class UtilityClass
{
  public ClassA1? ClassA1;
  public ClassB1? ClassB1;
  public IndependentClass1? IndependentClass1;
}