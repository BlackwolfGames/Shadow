using NUnit.Framework;
using SourceVis.Spec.Hooks;
using SourceVisCore.Graphing;

namespace SourceVis.Spec.Steps;

[Binding]
public class CycleSteps : LogHelper
{
    private readonly ScenarioContext _scenarioContext;
    public CycleSteps(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

    [Then(@"the graph contains (\d*) cycles?")]
    public void ThenTheGraphContainsCycle(int p0)
    {
        LogAssert(() => Assert.That(_scenarioContext.Get<IDependencyGraph>().Cycles, Is.EqualTo(p0)));
    }

    [Then(@"the (node '[^']*') (is|is not) part of a cycle")]
    public void ThenTheNodeIsPartOfACycle(INode? classA, bool isPart)
    {
        LogAssert(() => Assert.That(classA?.IsInCycle, Is.EqualTo(isPart)));
    }
}