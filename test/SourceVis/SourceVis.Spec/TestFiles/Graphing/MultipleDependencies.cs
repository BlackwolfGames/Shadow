namespace SourceVis.Spec.TestFiles.Graphing;

public interface IDependency { }
public class Dependency : IDependency { }
public class ConsumerClass {
    public ConsumerClass(IDependency dependency) {
        Dependency1 = dependency;
    }
    public IDependency Dependency1 { get; }
}