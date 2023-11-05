using NUnit.Framework;
using SourceVisCore.AST;
using SourceVisCore.Graphing;

namespace SourceVisSpec.Steps;

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

    [Then(@"there is a node named '([^']*)'")]
    public void ThenGraphNodeHasName(string nodeName)
    {
        Assert.That(_scenarioContext.Get<IDependencyGraph>()['.'+nodeName], Is.Not.Null);
    }

    [Then(@"the node '([^']*)' has (\d*) edges?")]
    public void ThenGraphNodeHasEdges(string nodeName, int p1)
    {
        Assert.That(_scenarioContext.Get<IDependencyGraph>()['.'+nodeName]?.Edges, Is.EqualTo(p1));
    }
    
    [Then(@"the node '([^']*)' has an edge to '([^']*)'")]
    public void ThenGraphNodeHasEdges(string nodeName, string edgeName)
    {
        Assert.That(_scenarioContext.Get<IDependencyGraph>()['.'+nodeName]?['.'+edgeName], Is.Not.Null);
    }
    
    [Then(@"the edge from '([^']*)' to '([^']*)' has (\d*) dependency of type '([^']*)'")]
    public void ThenGraphNodeHasEdges(string nodeName, string edgeName, int weight, DependencyType type)
    {
        Assert.That(_scenarioContext.Get<IDependencyGraph>()['.'+nodeName]?['.'+edgeName]?[type], Is.EqualTo(weight));
    }
}