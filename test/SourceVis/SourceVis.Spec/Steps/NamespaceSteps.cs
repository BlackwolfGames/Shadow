using NUnit.Framework;
using SourceVis.Spec.Hooks;
using SourceVisCore.Graphing;

namespace SourceVis.Spec.Steps;

[Binding]
public class NamespaceSteps : LogHelper
{
  [Then(@"(node '[^']*') (is|is not) in the same namespace as (node '[^']*')")]
  public void ThenTheNodeIsNotInTheSameNamespaceAsNode(INode classA, bool same, INode classB)
  {
    LogAssert(() => Assert.That(classA.Namespaces(), same ? Is.EqualTo(classB.Namespaces()) : Is.Not.EqualTo(classB.Namespaces())));
  }

  [Then(@"(node '[^']*') (is|is not) in namespace '(.*)'")]
  public void ThenTheNodeIsInNamespace(INode classA, bool inNamespace, string sourceVis)
  {
    LogAssert(() => Assert.That(classA.Namespaces(), inNamespace ? Has.One.EqualTo(sourceVis) : Has.None.EqualTo(sourceVis)));
  }


  [Then(@"the (edge from '[^']*' to '[^']*') leaves (\d) namespaces?")]
  public void ThenTheEdgeFromLeavesDNamespaces(IEdge edge, int leaves)
  {
    LogAssert(() => Assert.That(edge.LeavesNamespaces, Is.EqualTo(leaves)));
  }

  [Then(@"the (edge from '[^']*' to '[^']*') enters (\d) namespaces?")]
  public void ThenTheEdgeFromEntersDNamespaces(IEdge edge, int enters)
  {
    LogAssert(() => Assert.That(edge.EntersNamespaces, Is.EqualTo(enters)));
  }

  [Then(@"the (edge from '[^']*' to '[^']*') crosses (\d) namespaces?")]
  public void ThenTheEdgeFromCrossesDNamespaces(IEdge edge, int crosses)
  {
    LogAssert(() => Assert.That(edge.CrossesNamespaces, Is.EqualTo(crosses)));
  }

  [Then(@"the (edge from '[^']*' to '[^']*') shares (\d) namespaces?")]
  public void ThenTheEdgeFromSharedDNamespaces(IEdge edge, int shared)
  {
    LogAssert(() => Assert.That(edge.SharedNamespaces, Is.EqualTo(shared)));
  }
}