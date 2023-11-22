using System.Numerics;
using NUnit.Framework;
using SourceVisCore.Layout;

namespace SourceVis.Spec.Steps;

[Binding]
public class LayoutMetricSteps
{
    private readonly ScenarioContext _scenarioContext;

    public LayoutMetricSteps(ScenarioContext scenarioContext) =>
        _scenarioContext = scenarioContext;

    [Then(@"the total distance from center is ([\d\.]*)")]
    public void ThenTheTotalDistanceFromCenterIs(float p0)
    {
        Assert.That(
            _scenarioContext.Get<GraphProjection>().Nodes.Sum(node => node.Position.Length()),
            Is.EqualTo(p0).Within(0.1f)
        );
    }

    [Then(@"the average distance between nodes is ([\d\.]*)")]
    public void ThenTheAverageDistanceBetweenNodesIs(float p0)
    {
        var nodes = _scenarioContext
            .Get<GraphProjection>().Nodes
            .Select(node => node.Position).ToArray();
        
        Assert.That(
            nodes.SelectMany((point, index) => nodes.Skip(index + 1)
                    .Select(otherPoint => Vector2.Distance(point, otherPoint)))
                .Average(),
            Is.EqualTo(p0).Within(0.1f));
    }

    [Then(@"there (?:is|are) (\d*) intersections?")]
    public void ThenThereAreIntersections(int p0)
    {
        var edges = _scenarioContext
            .Get<GraphProjection>().Edges.ToArray();
        edges.SelectMany((e1, i) => edges.Skip(i + 1).Where(e2 => e1.Intersects(e2))).Count();
    }
}