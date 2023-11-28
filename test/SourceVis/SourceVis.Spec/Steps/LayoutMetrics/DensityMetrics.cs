using System.Numerics;
using NCalc;
using NUnit.Framework;
using SourceVisCore.Layout;

namespace SourceVis.Spec.Steps.LayoutMetrics;

[Binding]
public class DensityMetrics
{
  private readonly ScenarioContext _scenarioContext;

  public DensityMetrics(ScenarioContext scenarioContext)
  {
    _scenarioContext = scenarioContext;
  }

  [Then(@"the bounding box is (.*) ('[^\']*') by ('[^\']*')")]
  public void ThenTheBoundingBoxIs(EqualClass eq, Expression expectedWidth, Expression expectedHeight)
  {
    var nodes = _scenarioContext
      .Get<GraphProjection>()
      .Nodes.Select(node => node.Position);
    var minX = float.MaxValue;
    var maxX = float.MinValue;
    var minY = float.MaxValue;
    var maxY = float.MinValue;

    foreach (var point in nodes)
    {
      if (point.X < minX)
      {
        minX = point.X;
      }

      if (point.X > maxX)
      {
        maxX = point.X;
      }

      if (point.Y < minY)
      {
        minY = point.Y;
      }

      if (point.Y > maxY)
      {
        maxY = point.Y;
      }
    }

    var width = maxX - minX;
    var height = maxY - minY;

    Assert.That(width, eq.TestAs(expectedWidth.Evaluate()));
    Assert.That(height, eq.TestAs(expectedHeight.Evaluate()));
  }

  [Then(@"the contained node density is (.*) ('[^\']*')")]
  public void ThenTheContainedNodeDensityIs(EqualClass eq, Expression expectedDensity)
  {
    var nodes = _scenarioContext
      .Get<GraphProjection>()
      .Nodes.Select(node => node.Position);
    var minX = float.MaxValue;
    var maxX = float.MinValue;
    var minY = float.MaxValue;
    var maxY = float.MinValue;

    var enumerable = nodes as Vector2[] ?? nodes.ToArray();
    foreach (var point in enumerable)
    {
      if (point.X < minX)
      {
        minX = point.X;
      }

      if (point.X > maxX)
      {
        maxX = point.X;
      }

      if (point.Y < minY)
      {
        minY = point.Y;
      }

      if (point.Y > maxY)
      {
        maxY = point.Y;
      }
    }

    var width = maxX - minX;
    var height = maxY - minY;
    var area = width * height;
    var density = area / enumerable.Length;
    Assert.That(density, eq.TestAs(expectedDensity.Evaluate()));
  }

  [Then(@"the contained edge density is (.*) ('[^\']*')")]
  public void ThenTheContainedEdgeDensityIs(EqualClass eq, Expression expectedDensity)
  {
    var nodes = _scenarioContext
      .Get<GraphProjection>()
      .Nodes.Select(node => node.Position);
    var minX = float.MaxValue;
    var maxX = float.MinValue;
    var minY = float.MaxValue;
    var maxY = float.MinValue;

    foreach (var point in nodes)
    {
      if (point.X < minX)
      {
        minX = point.X;
      }

      if (point.X > maxX)
      {
        maxX = point.X;
      }

      if (point.Y < minY)
      {
        minY = point.Y;
      }

      if (point.Y > maxY)
      {
        maxY = point.Y;
      }
    }

    var width = maxX - minX;
    var height = maxY - minY;
    var area = width * height;
    var density = area / _scenarioContext.Get<GraphProjection>().Edges.Count();
    Assert.That(density, eq.TestAs(expectedDensity.Evaluate()));
  }
}
