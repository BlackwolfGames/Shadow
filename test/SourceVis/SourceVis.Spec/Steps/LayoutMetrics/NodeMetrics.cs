using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using NCalc;
using NUnit.Framework;
using SourceVisCore.Layout;
using TechTalk.SpecFlow;

namespace SourceVis.Spec.Steps;

[Binding]
public sealed class NodeMetrics
{
  // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

  private readonly ScenarioContext _scenarioContext;

  public NodeMetrics(ScenarioContext scenarioContext)
  {
    _scenarioContext = scenarioContext;
  }

  [Then(@"the average distance between nodes is (.*) ('[^\']*')")]
  public void ThenTheAverageDistanceBetweenNodesIsD(EqualClass eq, Expression p0)
  {
    var nodes = _scenarioContext
      .Get<GraphProjection>().Nodes
      .Select(node => node.Position).ToArray();
        
    Assert.That(
      nodes.SelectMany((point, index) => nodes.Skip(index + 1)
          .Select(otherPoint => Vector2.Distance(point, otherPoint)))
        .Average(),
      eq.TestAs(p0.Evaluate()));
  }
  

  [Then(@"the minimum distance between nodes is (.*) ('[^\']*')")]
  public void ThenTheMinimumDistanceBetweenNodesIsD(EqualClass eq, Expression p0)
  {
    var nodes = _scenarioContext
      .Get<GraphProjection>().Nodes
      .Select(node => node.Position).ToArray();
        
    Assert.That(
      nodes.SelectMany((point, index) => nodes.Skip(index + 1)
          .Select(otherPoint => Vector2.Distance(point, otherPoint)))
        .Min(),
      eq.TestAs(p0.Evaluate()));
  }

  [Then(@"the maximum distance between nodes is (.*) ('[^\']*')")]
  public void ThenTheMaximumDistanceBetweenNodesIsD(EqualClass eq, Expression p0)
  {
    var nodes = _scenarioContext
      .Get<GraphProjection>().Nodes
      .Select(node => node.Position).ToArray();
        
    Assert.That(
      nodes.SelectMany((point, index) => nodes.Skip(index + 1)
          .Select(otherPoint => Vector2.Distance(point, otherPoint)))
        .Max(),
      eq.TestAs(p0.Evaluate()));
  }
  
  [Then(@"the standard deviation of distances between nodes is (.*) ('[^\']*')")]
  public void ThenTheStandardDeviationOfDistancesBetweenNodesIsD(EqualClass eq, Expression p0)
  {
    var nodes = _scenarioContext.Get<GraphProjection>().Nodes.Select(node => node.Position).ToArray();

    var distances = nodes.SelectMany((point, index) => nodes.Skip(index + 1)
      .Select(otherPoint => Vector2.Distance(point, otherPoint))).ToArray();

    double mean = distances.Average();
    double sumOfSquaresOfDifferences = distances.Select(val => (val - mean) * (val - mean)).Sum();
    double standardDeviation = Math.Sqrt(sumOfSquaresOfDifferences / distances.Length);

    Assert.That(standardDeviation, eq.TestAs(p0.Evaluate()));
  }
}

