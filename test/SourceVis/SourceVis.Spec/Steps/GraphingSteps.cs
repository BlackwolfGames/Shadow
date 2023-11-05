using NUnit.Framework;
using SourceVisCore.AST;
using SourceVisCore.Graphing;

namespace SourceVis.Spec.Steps;

[Binding]
public class GraphingSteps
{
    private readonly ScenarioContext _scenarioContext;
    public GraphingSteps(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

    [Given(@"we convert the project into a graph")]
    public void GivenWeConvertItIntoAGraph()
    {
        _scenarioContext.Set(DependencyGraph.FromProject(_scenarioContext.Get<Project>()));
    }

    [Then(@"the graph contains (\d*) nodes?")]
    public void ThenTheGraphContainsNode(int p0)
    {
        Assert.That(_scenarioContext.Get<IDependencyGraph>().Nodes, Is.EqualTo(p0));
    }

    [Then(@"there is a (node '[^']*')")]
    public void ThenGraphNodeHasName(INode node)
    {
        Assert.That(node, Is.Not.Null);
    }

    [Then(@"the (node '[^']*') has (\d*) edges?")]
    public void ThenGraphNodeHasEdges(INode? node, int p1)
    {
        Assert.That(node?.Edges, Is.EqualTo(p1));
    }
    
    [Then(@"the (node '[^']*') has an edge to '([^']*)'")]
    public void ThenGraphNodeHasEdges(INode? node, string edgeName)
    {
        Assert.That(node?['.'+edgeName], Is.Not.Null);
    }
    
    [Then(@"the edge from (node '[^']*') to '([^']*)' has (\d*) dependency of type '([^']*)'")]
    public void ThenGraphNodeHasEdges(INode? node, string edgeName, int weight, DependencyType type)
    {
        Assert.That(node?['.'+edgeName]?[type], Is.EqualTo(weight));
    }
}