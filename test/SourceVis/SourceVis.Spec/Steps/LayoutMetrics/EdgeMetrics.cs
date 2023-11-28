using System.Numerics;
using NCalc;
using NUnit.Framework;
using SourceVisCore.Layout;

namespace SourceVis.Spec.Steps.LayoutMetrics;

[Binding]
public class EdgeMetrics
{
  private readonly ScenarioContext _scenarioContext;

  public EdgeMetrics(ScenarioContext scenarioContext)
  {
    _scenarioContext = scenarioContext;
  }

  [Then(@"the average edge length is (.*) ('[^\']*')")]
  public void ThenTheAverageEdgeLengthIs(EqualClass eq, Expression p0)
  {
    var edges = _scenarioContext
      .Get<GraphProjection>()
      .Edges.ToArray();
    Assert.That(edges
        .Select(edge => Vector2.Distance(edge.Start, edge.End))
        .Average(),
      eq.TestAs(p0.Evaluate()));
  }

  [Then(@"the minimum edge length is (.*) ('[^\']*')")]
  public void ThenTheMinimumEdgeLengthIs(EqualClass eq, Expression p0)
  {
    var edges = _scenarioContext
      .Get<GraphProjection>()
      .Edges.ToArray();
    Assert.That(
      edges
        .Select(edge => Vector2.Distance(edge.Start, edge.End))
        .Min(),
      eq.TestAs(p0.Evaluate()));
  }

  [Then(@"the maximum edge length is (.*) ('[^\']*')")]
  public void ThenTheMaximumEdgeLengthIs(EqualClass eq, Expression p0)
  {
    var edges = _scenarioContext
      .Get<GraphProjection>()
      .Edges.ToArray();

    Assert.That(
      edges
        .Select(edge => Vector2.Distance(edge.Start, edge.End))
        .Max(),
      eq.TestAs(p0.Evaluate()));
  }

  [Then(@"the standard deviation of edge length is (.*) ('[^\']*')")]
  public void ThenTheStandardDeviationOfEdgeLengthIs(EqualClass eq,Expression p0)
  {
    var edges = _scenarioContext
      .Get<GraphProjection>()
      .Edges.ToArray();
    var distances = edges
      .Select(edge => Vector2.Distance(edge.Start, edge.End)).ToArray();

    double mean = distances.Average();
    double sumOfSquaresOfDifferences = distances.Select(val => (val - mean) * (val - mean)).Sum();
    double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / distances.Length);

    Assert.That(standardDeviation, eq.TestAs(p0.Evaluate()));
  }
}
