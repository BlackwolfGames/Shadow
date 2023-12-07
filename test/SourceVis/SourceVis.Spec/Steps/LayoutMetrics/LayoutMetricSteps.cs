using System.Numerics;
using NCalc;
using NUnit.Framework;
using SourceVisCore.Layout;

namespace SourceVis.Spec.Steps.LayoutMetrics;

[Binding]
public class LayoutMetricSteps
{
  private readonly ScenarioContext _scenarioContext;

  public LayoutMetricSteps(ScenarioContext scenarioContext)
  {
    _scenarioContext = scenarioContext;
  }

  [Then(@"the total distance from center is (.*) ('[^\']*')")]
  public void ThenTheTotalDistanceFromCenterIs(EqualClass eq, Expression p0)
  {
    Assert.That(_scenarioContext.Get<GraphProjection>().Nodes.Sum(node => node.Position.Length()), eq.TestAs(p0.Evaluate()));
  }

  [Then(@"the mass distance from center is (.*) ('[^\']*')")]
  public void ThenTheCentroidDistanceFromCenterIs(EqualClass eq, Expression p0)
  {
    var nodes = _scenarioContext.Get<GraphProjection>().Nodes;
    var sumX = nodes.Sum(node => node.Position.X);
    var sumY = nodes.Sum(node => node.Position.Y);
    Vector2 centerOfMass = new Vector2(sumX, sumY) / nodes.Length;
    Assert.That(centerOfMass.Length(), eq.TestAs(p0.Evaluate()));
  }

  [Then(@"the number of intersections is (.*) ('[^\']*')")]
  public void ThenThereAreIntersections(EqualClass eq, Expression p0)
  {
    var edges = _scenarioContext.Get<GraphProjection>().Edges.ToArray();
    var count = edges.SelectMany((e1, i) => edges.Skip(i + 1).Where(e2 => e1.Intersects(e2))).Count();
    Assert.That(count, eq.TestAs(p0.Evaluate()));
  }
}