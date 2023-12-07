using NUnit.Framework;
using SourceVis.Spec.Hooks;
using SourceVisCore.AST;
using SourceVisCore.Graphing;

namespace SourceVis.Spec.Steps;

[Binding]
public class GraphingSteps : LogHelper
{
  private readonly ScenarioContext _scenarioContext;

  public GraphingSteps(ScenarioContext scenarioContext)
  {
    _scenarioContext = scenarioContext;
  }

  [Given(@"we convert the project into a graph")]
  public void GivenWeConvertItIntoAGraph()
  {
    _scenarioContext.Set(DependencyGraph.FromProject(_scenarioContext.Get<Project>()));
  }

  [Then(@"the graph contains (\d*) nodes?")]
  public void ThenTheGraphContainsNode(int p0)
  {
    LogAssert(() => Assert.That(_scenarioContext.Get<IDependencyGraph>().Nodes, Is.EqualTo(p0)));
  }

  [Then(@"there is a (node '[^']*')")]
  public void ThenGraphNodeHasName(INode node)
  {
    LogAssert(() => Assert.That(node, Is.Not.Null));
  }

  [Then(@"(node '[^']*') has (\d*) edges?")]
  public void ThenGraphNodeHasEdges(INode node, int p1)
  {
    LogAssert(() => Assert.That(node.Edges, Is.EqualTo(p1)));
  }

  [Then(@"(node '[^']*') has an edge to (node '[^']*')")]
  public void ThenGraphNodeHasEdges(INode node, INode edge)
  {
    LogAssert(() => Assert.That(node[edge.Name], Is.Not.Null));
  }

  [Then(@"the (edge from '[^']*' to '[^']*') has (\d*) dependency of type '([^']*)'")]
  public void ThenGraphNodeHasEdges(IEdge edge, int weight, DependencyType type)
  {
    LogAssert(() => Assert.That(edge[type], Is.EqualTo(weight)));
  }

  [Then(@"(node '[^']*') is of '(.*)' type")]
  public void ThenNodeIsOfType(INode node, NodeType type)
  {
    LogAssert(() => Assert.That(node.NodeType, Is.EqualTo(type)));
  }
}