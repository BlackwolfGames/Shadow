using System.Numerics;
using NUnit.Framework;
using SourceVisCore.Graphing;
using SourceVisCore.Layout;

namespace SourceVis.Spec.Steps;

[Binding] public class LayoutSteps
{
    private readonly ScenarioContext _scenarioContext;
    public LayoutSteps(ScenarioContext scenarioContext) => 
        _scenarioContext = scenarioContext;

    [Given(@"we create a projection for the graph")]
    public void GivenWeCreateAProjectionForTheGraph()
    {
        _scenarioContext.Set(GraphProjection
            .FromGraph(_scenarioContext.Get<IDependencyGraph>()));
    }

    [Then(@"the (projection of '.*') is at (\([^)]*\))")]
    public void ThenTheProjectionOfIsAt(ProjectedNode node, Vector2 expected)
    {
        Assert.That(node.Position.X, Is.EqualTo(expected.X).Within(0.01f));
        Assert.That(node.Position.Y, Is.EqualTo(expected.Y).Within(0.01f));
    }
    
    [Then(@"the (projection of '.*') is not at (\([^)]*\))")]
    public void ThenTheProjectionOfIsNotAt(ProjectedNode node, Vector2 expected)
    {
        Assert.That(node.Position.X, Is.Not.EqualTo(expected.X).Within(0.1f));
        Assert.That(node.Position.Y, Is.Not.EqualTo(expected.Y).Within(0.1f));
    }
    
    [Then(@"the (projection of '.*') is (\d*) away from (\([^)]*\))")]
    public void ThenTheProjectionOfIsWithinDFrom(ProjectedNode node, float distance, Vector2 expected)
    {
        Assert.That(Vector2.Distance(node.Position, expected), Is.EqualTo(distance).Within(0.01f));
    }

    [When(@"moving the (projection of '.*') to (\([^)]*\))")]
    public static void WhenMovingTheProjectionOfTo(
        ProjectedNode testCallConsole, Vector2 p1)
    {
        testCallConsole.Position = p1;
    }
}