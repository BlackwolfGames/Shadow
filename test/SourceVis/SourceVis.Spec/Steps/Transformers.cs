using SourceVisCore.Graphing;

namespace SourceVis.Spec.Steps;

[Binding]
public class NodeTransformations
{
    private readonly ScenarioContext _scenarioContext;

    public NodeTransformations(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }
    [StepArgumentTransformation("(is|is not)")]
    public bool IsToBool(string input) => input == "is";

    [StepArgumentTransformation("node '(.*)'")]
    public INode NameToNode(string input) => 
        _scenarioContext.Get<IDependencyGraph>()[input]
        ?? throw new KeyNotFoundException("Node not found: " + input);
}