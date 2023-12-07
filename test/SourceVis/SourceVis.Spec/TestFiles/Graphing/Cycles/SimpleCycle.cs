namespace SourceVis.Spec.TestFiles.Graphing.Cycles;

public class ClassA
{
  public ClassB? _classB;
}

public class ClassB
{
  public ClassA? _classA;
}

public class ClassC
{
  public ClassA? _classA;
}