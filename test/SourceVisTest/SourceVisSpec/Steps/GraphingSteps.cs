using SourceVisCore.Graphing;

namespace SourceVisSpec.Steps;

[Binding] public class GraphingSteps
{
    
    private readonly ScenarioContext _scenarioContext;
    public GraphingSteps(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;
    [Given(@"an empty graph")]
    public void GivenAnEmptyGraph()
    {
        _scenarioContext.Set(new DependencyGraph());
    }
}