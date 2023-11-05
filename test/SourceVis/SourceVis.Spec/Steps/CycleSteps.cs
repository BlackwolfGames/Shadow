using NUnit.Framework;
using SourceVisCore.Graphing;

namespace SourceVis.Spec.Steps;

[Binding] public class CycleSteps
{
    
    private readonly ScenarioContext _scenarioContext;
    public CycleSteps(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;
    [Then(@"the graph contains (\d*) cycles?")]
    public void ThenTheGraphContainsCycle(int p0)
    {
        Assert.That(_scenarioContext.Get<IDependencyGraph>().Cycles, Is.EqualTo(p0));
    }

    [Then(@"the (node '[^']*') (is|is not) part of a cycle")]
    public void ThenTheNodeIsPartOfACycle(INode? classA, bool isPart)
    {
        Assert.That(classA?.IsInCycle, Is.EqualTo(isPart));
    }
}